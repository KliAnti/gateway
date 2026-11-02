namespace gateway.Features.Identity.Ping
{
    /// <summary>
    /// Собирает все HTTP-эндпоинты, которые обращаются к сервису identity.
    /// </summary>
    public static class PingIdentityEndpoint
    {
        /// <summary>
        /// Регистрирует эндпоинты identity в приложении - чтобы их можно было вызвать по HTTP.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public static void MapIdentityEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/identity");

            group.MapGet("/ping", async (
                PingIdentityHandler handler,
                CancellationToken cancellationToken,
                string message = "Ping") =>
            {
                var response = await handler.HandleAsync(new PingIdentityRequest(message), cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
