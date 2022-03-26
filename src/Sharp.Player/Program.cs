using Serilog;
using Serilog.Exceptions;
using Sharp.Player;
using Sharp.Player.Services;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

Host.CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services))
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
    .ConfigureServices(s =>
    {
        // Should run first.
        s.AddHostedService<PlayerRegistrationService>();
        s.AddHostedService<GameRegistrationService>();
        s.AddHostedService<CommandTransactionCleanupService>();
    })
    .Build()
    .Run();