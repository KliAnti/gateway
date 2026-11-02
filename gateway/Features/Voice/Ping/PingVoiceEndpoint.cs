namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Маппит эндпоинт для проверки эхо сервиса voice.
    /// </summary>
    public static class PingVoiceEndpoint
    {
        /// <summary>
        /// Регистрирует GET эндпоинт /api/voice/ping.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public static void MapVoiceEndpoint(this IEndpointRouteBuilder app)
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
