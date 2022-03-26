using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Sharp.Player;
using Sharp.Player.Services;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new RenderedCompactJsonFormatter(), "./logs/log.ndjson")
    .CreateBootstrapLogger();

Host.CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Console())
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
    .ConfigureServices(s =>
    {
        // Should run first.
        s.AddHostedService<PlayerRegistrationService>();
        s.AddHostedService<GameRegistrationService>();
    })
    .Build()
    .Run();