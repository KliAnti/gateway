using gateway.Infrastructure.Endpoints;

namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Эндпоинт для проверки эхо сервиса voice.
    /// </summary>
    public sealed class PingVoiceEndpoint : IEndpoint
    {
        /// <summary>
        /// Регистрирует PingVoiceHandler в DI.
        /// </summary>
        /// <param name="services">Коллекция сервисов DI контейнера.</param>
        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<PingVoiceHandler>();
        }

        /// <summary>
        /// Регистрирует GET эндпоинт /api/voice/ping.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public void Map(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/voice");

            group.MapGet("/ping", async (
                PingVoiceHandler handler,
                CancellationToken cancellationToken,
                string message = "Ping") =>
            {
                var response = await handler.HandleAsync(new PingVoiceRequest(message), cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
