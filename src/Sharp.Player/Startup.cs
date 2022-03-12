using Microsoft.EntityFrameworkCore;
using Refit;
using Sharp.Client.Client;
using Sharp.Data.Context;
using Sharp.Player.Config;
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

        services.AddSingleton<IPlayerDetailsProvider, PlayerDetailsProvider>();

        services.AddEndpointsApiExplorer();

        services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SharpDbContext dbContext)
    {
        dbContext.Database.Migrate();

        app.UseDeveloperExceptionPage();
        app.UseRouting();
        
        if (env.IsDevelopment())
        {
        }

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}