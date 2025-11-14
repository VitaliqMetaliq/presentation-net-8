using TrueCode.BackgroundService.Main.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

WebApplication app = builder.Build();

await app.RunAsync();
