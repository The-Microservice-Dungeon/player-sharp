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
using Sharp.Player.Consumers.Model;
using Sharp.Player.Hubs;
using Sharp.Player.Manager;

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
        var playerRegistrationClient = RestService.For<IPlayerRegistrationClient>(networkOptions.GameServiceAddress);
        services.AddSingleton(playerRegistrationClient);
        var gameClient = RestService.For<IGameClient>(networkOptions.GameServiceAddress);
        services.AddSingleton(gameClient);

        services.AddSingleton<IPlayerManager, PlayerManager>();
        services.AddSingleton<IGameManager, GameManager>();
        services.AddSingleton<IMapManager, MapManager>();

        // Kafka / Consumers / ...
        services.AddKafka(kafka => kafka.UseMicrosoftLog()
            .AddCluster(cluster => cluster
                .WithBrokers(new[] { networkOptions.KafkaAddress })
                .AddDefaultConsumer<PlayerStatusEvent, PlayerStatusMessageHandler>("playerStatus")
                .AddDefaultConsumer<GameStatusEvent, GameStatusMessageHandler>("status")
                .AddDefaultConsumer<GameworldCreatedEvent, GameworldCreatedMessageHandler>("gameworld-created")
                .AddDefaultConsumer<SpacestationCreatedEvent, SpacestationCreatedMessageHandler>("spacestation-created")
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
    public static IClusterConfigurationBuilder AddDefaultConsumer<T, H>(this IClusterConfigurationBuilder builder,
        string topic) where H : class, IMessageHandler<T>
    {
        return builder.AddConsumer(consumer => consumer
            .Topic(topic)
            .WithGroupId("player-sharp")
            .WithWorkersCount(1)
            .WithBufferSize(100)
            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
            .AddMiddlewares(middlewares => middlewares
                .AddSingleTypeSerializer<JsonCoreSerializer>(typeof(T))
                .AddTypedHandlers(handlers => handlers
                    .AddHandler<H>()
                )
            )
        );
    }
}