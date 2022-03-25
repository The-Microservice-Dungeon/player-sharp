using Serilog;
using Serilog.Formatting.Compact;
using Sharp.Player;
using Sharp.Player.Services;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new RenderedCompactJsonFormatter(), "./logs/log.ndjson")
    .CreateLogger();

Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
    .ConfigureServices(s =>
    {
        // Should run first.
        s.AddHostedService<PlayerRegistrationService>();
        s.AddHostedService<GameRegistrationService>();
    })
    .Build()
    .Run();