using Player.Sharp.Client;
using Player.Sharp.Consumers;
using Player.Sharp.Consumers;
using Player.Sharp.Data;
using Player.Sharp.Gameplay;
using Player.Sharp.Services;
using Player.Sharp.Util;
using Refit;

namespace Player.Sharp
{
    public class Startup { 
        public Startup(IConfiguration configuration)
        {
            // TODO: Hacky Last-Minute hack to include the provided environment variables
            var gameServiceEnv = Environment.GetEnvironmentVariable("GAME_SERVICE");
            if (gameServiceEnv != null)
                configuration["Refit:Client:GameServiceAddress"] = gameServiceEnv;
            var kafkaAddressEnv = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_ADDRESS");
                configuration["Kafka:ConsumerSettings:BootstrapServers"] = kafkaAddressEnv;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Repositories
            services.AddSingleton<IGameRepository, InMemGameRepository>();
            services.AddSingleton<IPlayerCredentialsRepository, InMemPlayerCredentialsRepository>();
            services.AddSingleton<IMapRepository, InMemMapRepository>();
            services.AddSingleton<IRobotRepository, InMemRobotRepository>();
            services.AddSingleton<ITransactionRepository, InMemTransactionRepository>();

            // Clients
            IConfigurationSection refitSection = Configuration.GetSection("Refit:Client");
            var gameServiceAddress = refitSection.GetValue<string>("GameServiceAddress");
            
            var httpClient = new HttpClient(new HttpLoggingHandler()) { BaseAddress = new Uri(gameServiceAddress, UriKind.Absolute) };
            var gameClient = RestService.For<IGameClient>(httpClient);
            services.AddSingleton(gameClient);

            // Consumers
            services.AddSingleton<IHostedService, GamePlayerStatusConsumer>();
            services.AddSingleton<IHostedService, GameStatusConsumer>();
            services.AddSingleton<IHostedService, GameRoundStatusConsumer>();
            services.AddSingleton<IHostedService, MapGameworldCreatedConsumer>();
            services.AddSingleton<IHostedService, RobotDestroyedEventConsumer>();
            services.AddSingleton<IHostedService, TradingTradesBuyRobotEventConsumer>();

            // Other services
            services.AddSingleton<IHostedService, PlayerRegistrationService>();
            services.AddSingleton<GameService>();
            services.AddSingleton<RobotService>();
            services.AddSingleton<MapService>();
            services.AddSingleton<TransactionService>();
            services.AddSingleton<CommandService>();
            services.AddSingleton<IRoundLifecycleHandler, NoRobotLifecycleHandler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
