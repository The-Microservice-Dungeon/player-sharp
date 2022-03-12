using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.EntityFrameworkCore;
using Refit;
using Sharp.Client.Client;
using Sharp.Client.Model;
using Sharp.Core;
using Sharp.Data.Context;
using Sharp.Player.Config;
using Sharp.Player.Consumers;
using Sharp.Player.Consumers.Model;
using Sharp.Player.Manager;
using Sharp.Player.Services;

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

        // TODO: Temporarly we will simply put all service configuration here. We should probably split it up in a
        //  better way. Some parts have even found their way into Program.cs ... That needs refactoring
        
        // Mapper
        services.AddAutoMapper(typeof(GameMappingProfile));
        
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
                .AddConsumer(consumer => consumer
                    .Topic("playerStatus")
                    .WithGroupId("player-sharp")
                    .WithWorkersCount(1)
                    .WithBufferSize(100)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSingleTypeSerializer<JsonCoreSerializer>(typeof(PlayerStatusEvent))
                        .AddTypedHandlers(handlers => handlers
                            .AddHandler<PlayerStatusMessageHandler>()
                        )
                    )
                )
                .AddConsumer(consumer => consumer
                    .Topic("status")
                    .WithGroupId("player-sharp")
                    .WithWorkersCount(1)
                    .WithBufferSize(100)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSingleTypeSerializer<JsonCoreSerializer>(typeof(GameStatusEvent))
                        .AddTypedHandlers(handlers => handlers
                            .AddHandler<GameStatusMessageHandler>()
                        )
                    )
                )
                .AddConsumer(consumer => consumer
                    .Topic("gameworld-created")
                    .WithGroupId("player-sharp")
                    .WithWorkersCount(1)
                    .WithBufferSize(100)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSingleTypeSerializer<JsonCoreSerializer>(typeof(GameworldCreatedEvent))
                        .AddTypedHandlers(handlers => handlers
                            .AddHandler<GameworldCreatedMessageHandler>()
                        )
                    )
                )
                .AddConsumer(consumer => consumer
                    .Topic("spacestation-created")
                    .WithGroupId("player-sharp")
                    .WithWorkersCount(1)
                    .WithBufferSize(100)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSingleTypeSerializer<JsonCoreSerializer>(typeof(SpacestationCreatedEvent))
                        .AddTypedHandlers(handlers => handlers
                            .AddHandler<SpacestationCreatedMessageHandler>()
                        )
                    )
                )
            )
        );

        services.AddEndpointsApiExplorer();

        services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SharpDbContext dbContext, IHostApplicationLifetime lifetime)
    {
        dbContext.Database.Migrate();

        var kafkaBus = app.ApplicationServices.CreateKafkaBus();
        lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));

        app.UseDeveloperExceptionPage();
        app.UseRouting();

        if (env.IsDevelopment())
        {
        }

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}