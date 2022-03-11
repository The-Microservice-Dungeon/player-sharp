using Sharp.Player;
using Sharp.Player.Services;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
    .ConfigureServices(s =>
    {
        // Should run first.
        s.AddHostedService<PlayerRegistrationService>();
    })
    .Build()
    .Run();