using Life.Server.Infrastructure.Controllers;
using Life.Shared.Domain;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc( options =>
    {
        options.MaxReceiveMessageSize = 8192 * 8192;
        options.MaxSendMessageSize = 8192 * 8192;
    }
);


var app = builder.Build();
//http://46.72.251.132:7355;
// Configure the HTTP request pipeline.
app.MapGrpcService<GameController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");



app.Run();
