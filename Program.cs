using Player.Sharp.Consumers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Player.Sharp;


var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    }).Build()
    .Run();
