using gateway.Infrastructure.Endpoints;

namespace gateway.Features.Identity.Ping
{
    /// <summary>
    /// Эндпоинт для проверки эхо сервиса identity.
    /// </summary>
    public sealed class PingIdentityEndpoint : IEndpoint
    {
        /// <summary>
        /// Регистрирует PingIdentityHandler в DI.
        /// </summary>
        /// <param name="services">Коллекция сервисов DI контейнера.</param>
        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<PingIdentityHandler>();
        }

        /// <summary>
        /// Регистрирует GET эндпоинт /api/identity/ping.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public void Map(IEndpointRouteBuilder app)
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
