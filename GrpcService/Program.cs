using Life.Server.Core.Contracts.Services;
using Life.Server.Core.Domain;
using Life.Server.Infrastructure.Controllers;
using Life.Server.Infrastructure.Services;
using Life.Shared.Domain;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton<IGameLoop<CellOptimize>, GameLoopOptimize<CellOptimize>>();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<GameController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");



app.Run();
