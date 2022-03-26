using System.Text.Json;
using System.Text.Json.Serialization;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.EntityFrameworkCore;
using Refit;
using Sharp.Client.Client;
using Sharp.Data.Context;
using Sharp.Player.Config;
using Sharp.Player.Consumers;
using Sharp.Player.Consumers.TypeResolver.Trading;
using Sharp.Player.Events.Consumers.Game;
using Sharp.Player.Events.Consumers.Map;
using Sharp.Player.Events.Consumers.Robot;
using Sharp.Player.Events.Consumers.Trading;
using Sharp.Player.Events.Models.Game;
using Sharp.Player.Events.Models.Map;
using Sharp.Player.Events.Models.Robot;
using Sharp.Player.Hubs;
using Sharp.Player.Manager;
using Sharp.Player.Middleware.Kafka;

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
        services.AddAutoMapper(typeof(GameMappingProfile), typeof(MapMappingProfile));

        // Register custom configuration
        services.Configure<PlayerDetailsOptions>(
            Configuration.GetSection(PlayerDetailsOptions.PlayerDetails));
        services.Configure<DungeonNetworkOptions>(
            Configuration.GetSection(DungeonNetworkOptions.DungeonNetwork));

        // Data
        services.AddDbContext<SharpDbContext>(ServiceLifetime.Singleton, ServiceLifetime.Singleton);

        // Clients
        var networkOptions =
            Configuration.GetSection(DungeonNetworkOptions.DungeonNetwork).Get<DungeonNetworkOptions>();

        services.AddRefitClient<IPlayerRegistrationClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));
        services.AddRefitClient<IGameClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));
        services.AddRefitClient<IGameCommandClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(networkOptions.GameServiceAddress));

        services.AddSingleton<IPlayerManager, PlayerManager>();
        services.AddSingleton<IGameManager, GameManager>();
        services.AddSingleton<IMapManager, MapManager>();
        services.AddSingleton<ICommandManager, CommandManager>();

        // Kafka / Consumers / ...
        services.AddSingleton<IMessageMiddleware, FilterOldMessages>();
        services.AddKafka(kafka => kafka.UseMicrosoftLog()
            .AddCluster(cluster => cluster
                .WithBrokers(new[] { networkOptions.KafkaAddress })
                .AddDefaultConsumer<PlayerStatusEvent, PlayerStatusMessageHandler>("playerStatus")
                .AddDefaultConsumer<GameStatusEvent, GameStatusMessageHandler>("status")
                .AddDefaultConsumer<GameworldCreatedEvent, GameworldCreatedMessageHandler>("gameworld-created")
                .AddDefaultConsumer<SpacestationCreatedEvent, SpacestationCreatedMessageHandler>("spacestation-created")
                .AddDefaultConsumer<MovementEvent, MovementEventMessageHandler>("movement")
                .AddDefaultConsumer<NeighboursEvent, NeighbourEventMessageHandler>("neighbours")
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
                        .AddSerializer<JsonCoreSerializer, TradeEventTypeResolver>()
                        .AddTypedHandlers(handlers => handlers
                            .WithHandlerLifetime(InstanceLifetime.Singleton)
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
            .WithGroupId("player-sharp-1")
            .WithWorkersCount(2)
            .WithBufferSize(100)
            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
            .WithMaxPollIntervalMs(45000);
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