using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.EntityFrameworkCore;
using Refit;
using Sharp.Client.Client;
using Sharp.Data.Contexts;
using Sharp.Player.Config;
using Sharp.Player.Config.Mapping;
using Sharp.Player.Events.Consumers.Game;
using Sharp.Player.Events.Consumers.Map;
using Sharp.Player.Events.Consumers.Robot;
using Sharp.Player.Events.Consumers.Trading;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Events.Models.Map;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Events.Models.Trading;
using Sharp.Player.Events.TypeResolver.Trading;
using Sharp.Player.Hubs;
using Sharp.Player.Manager;
using Sharp.Player.Middleware.Kafka;
using Sharp.Player.Provider;
using Sharp.Player.Repository;
using AutoOffsetReset = KafkaFlow.AutoOffsetReset;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Sharp.Player;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSignalR();

        services.AddCors(options =>
        {
            options.AddPolicy("ClientPermission", policy =>
            {
                // TODO i think its clear why
                policy.AllowAnyHeader()
                    .WithOrigins("http://localhost:3000", "http://localhost")
                    //.SetIsOriginAllowed((_) => true)
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
        });

        // TODO: Temporarly we will simply put all service configuration here. We should probably split it up in a
        //  better way. Some parts have even found their way into Program.cs ... That needs refactoring

        // Mapper
        services
            .AddAutoMapper(typeof(GameMappingProfile).Assembly);

        // Register custom configuration
        services.Configure<PlayerDetailsOptions>(
            Configuration.GetSection(PlayerDetailsOptions.PlayerDetails));
        services.Configure<DungeonNetworkOptions>(
            Configuration.GetSection(DungeonNetworkOptions.DungeonNetwork));

        // Data
        services.AddDbContext<SharpDbContext>();

        // Clients
        var networkOptions =
            Configuration.GetSection(DungeonNetworkOptions.DungeonNetwork).Get<DungeonNetworkOptions>();

        services.AddRefitClient<IPlayerRegistrationClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));
        services.AddRefitClient<IGameClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));
        services.AddRefitClient<IGameCommandClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));

        services.AddScoped<IPlayerDetailsProvider, PlayerDetailsProvider>();
        
        services.AddScoped<IGameManager, GameManager>();
        services.AddScoped<IMapManager, MapManager>();
        services.AddScoped<ICommandManager, CommandManager>();
        services.AddScoped<IRobotManager, RobotManager>();

        services.AddSingleton<ICurrentGameStore, MemoryCurrentGameStore>();
        services.AddSingleton<ICurrentMapStore, MemoryCurrentMapStore>();
        services.AddSingleton<IRobotFleetStore, MemoryRobotFleetStore>();
        services.AddSingleton<IWalletStore, MemoryWalletStore>();
        services.AddSingleton<ITransactionContextStore, MemoryTransactionContextStore>();

        // Kafka / Consumers / ...
        services.AddSingleton<IMessageMiddleware, FilterOldMessages>();
        // TODO (for my own interest): Why KafkaFlowHostedService and not AddKafka? Are the lifetimes different? How does
        //  it work internally with a scoped lifetime?
        services.AddKafkaFlowHostedService(kafka => kafka.UseMicrosoftLog()
            .AddCluster(cluster => cluster
                .WithBrokers(new[]
                {
                    networkOptions.KafkaAddress
                })
                .AddDefaultConsumer<GameStatusEvent, GameStatusMessageHandler>("status")
                .AddDefaultConsumer<GameworldCreatedEvent, GameworldCreatedMessageHandler>("gameworld-created")
                .AddDefaultConsumer<SpacestationCreatedEvent, SpacestationCreatedMessageHandler>("spacestation-created")
                .AddDefaultConsumer<MovementEvent, MovementEventMessageHandler>("movement")
                .AddDefaultConsumer<NeighboursEvent, NeighbourEventMessageHandler>("neighbours")
                .AddDefaultConsumer<RegenerationEvent, RegenerationEventMessageHandler>("regeneration")
                .AddDefaultConsumer<BankCreatedEvent, BankCreatedEventHandler>("bank-created")
                .AddConsumer(consumer => consumer
                    .DefaultTypedConsumer<RoundStatusEvent, RoundStatusMessageHandler>("roundStatus")
                    .AddMiddlewares(middlewares => middlewares
                        .AddAtBeginning<FilterOldMessages>()
                    )
                )
                .AddConsumer(consumer => consumer
                    .DefaultConsumer("trades")
                    .AddMiddlewares(middlewares => middlewares
                        .AddAtBeginning<FilterOldMessages>()
                        .Add<DeferUntilWalletIsCreated>()
                        .AddSerializer<JsonCoreSerializer, TradeEventTypeResolver>()
                        .AddTypedHandlers(handlers => handlers
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<TradeBuyRobotEventHandler>()
                            .AddHandler<TradeSellResourcesEventHandler>()
                            .AddHandler<TradeErrorEventHandler>()
                        )
                    )
                )
            )
        );

        services.AddEndpointsApiExplorer();

        services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SharpDbContext dbContext,
        IHostApplicationLifetime lifetime)
    {
        dbContext.Database.Migrate();

        var mapperConfiguration = app.ApplicationServices.GetRequiredService<IConfigurationProvider>();
        mapperConfiguration.CompileMappings();
        mapperConfiguration.AssertConfigurationIsValid();

        var kafkaBus = app.ApplicationServices.CreateKafkaBus();
        lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));

        app.UseDeveloperExceptionPage();
        app.UseRouting();

        app.UseCors("ClientPermission");

        // Must be called before UseStaticFiels() 
        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (env.IsDevelopment())
        {
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<MapHub>("socket/map");
        });
    }
}

public static class KafkaHelper
{
    public static IConsumerConfigurationBuilder DefaultConsumer(
        this IConsumerConfigurationBuilder builder, string topic)
    {
        return builder
            .Topic(topic)
            .WithGroupId("player-sharp")
            .WithWorkersCount(1)
            .WithBufferSize(100)
            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
            .WithConsumerConfig(new ConsumerConfig()
            {
                // GroupInstanceId = Guid.NewGuid().ToString(),
                MaxPollIntervalMs = 30000,
                SessionTimeoutMs = 10000,
            })
            .AddMiddlewares(middlewares => middlewares
                .Add<TransactionIdResolver>(MiddlewareLifetime.Scoped)
                .Add<FilterMessagesFromUnregisteredGames>()
                .Add<TransactionIdConsumptionMarker>());
    }

    public static IConsumerConfigurationBuilder DefaultTypedConsumer<TMessage, THandler>(
        this IConsumerConfigurationBuilder builder, string topic)
        where THandler : class, IMessageHandler<TMessage>
    {
        return builder.DefaultConsumer(topic)
            .AddMiddlewares(middlewares => middlewares
                .AddSingleTypeSerializer<TMessage, JsonCoreSerializer>(s =>
                    new JsonCoreSerializer(new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumMemberConverter() }
                    }))
            )
            .AddMiddlewares(middlewares => middlewares
                .AddTypedHandlers(handlers => handlers
                    .WithHandlerLifetime(InstanceLifetime.Scoped)
                    .AddHandler<THandler>()
                ));
    }

    public static IClusterConfigurationBuilder AddDefaultConsumer<TMessage, THandler>(
        this IClusterConfigurationBuilder builder,
        string topic) where THandler : class, IMessageHandler<TMessage>
    {
        return builder.AddConsumer(consumer => consumer.DefaultTypedConsumer<TMessage, THandler>(topic));
    }
}