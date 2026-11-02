namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Собирает все HTTP-эндпоинты, которые обращаются к сервису voice.
    /// </summary>
    public static class PingVoiceEndpoint
    {
        /// <summary>
        /// Регистрирует эндпоинты voice в приложении - чтобы их можно было вызвать по HTTP.
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
