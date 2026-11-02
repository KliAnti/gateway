using gateway.Clients;
using gateway.Features.Identity.Ping;
using gateway.Features.Voice.Ping;
using Serilog;
using Serilog.Enrichers.Span;
using IdentityClient = identity.Grpc.Identity.IdentityClient;
using VoiceClient = voice.Grpc.Voice.VoiceClient;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .Enrich.FromLogContext()
        .Enrich.WithSpan()
        .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}"));

builder.Services.AddGrpcClient<IdentityClient>(options =>
{
    var address = builder.Configuration["Grpc:IdentityAddress"]
        ?? throw new InvalidOperationException("Missing required configuration: 'Grpc:IdentityAddress'.");
    options.Address = new Uri(address);
});

builder.Services.AddGrpcClient<VoiceClient>(options =>
{
    var address = builder.Configuration["Grpc:VoiceAddress"]
        ?? throw new InvalidOperationException("Missing required configuration: 'Grpc:VoiceAddress'.");
    options.Address = new Uri(address);
});

builder.Services.AddScoped<IdentityGrpcClient>();
builder.Services.AddScoped<VoiceGrpcClient>();

builder.Services.AddScoped<PingIdentityHandler>();
builder.Services.AddScoped<PingVoiceHandler>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");

app.MapIdentityEndpoint();
app.MapVoiceEndpoint();

app.Run();
