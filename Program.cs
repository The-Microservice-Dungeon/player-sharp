using Player.Sharp;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).Build()
    .Run();