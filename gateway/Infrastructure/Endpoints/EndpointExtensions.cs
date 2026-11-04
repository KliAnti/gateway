using System.Reflection;

namespace gateway.Infrastructure.Endpoints
{
    /// <summary>
    /// Находит все классы, реализующие IEndpoint (в указанной сборке),
    /// регистрирует их зависимости в DI и маппит их маршруты.
    /// </summary>
    public static class EndpointExtensions
    {
        /// <summary>
        /// Сканирует сборку, находит все IEndpoint и дает каждому зарегистрировать свои зависимости.
        /// </summary>
        /// <param name="services">Коллекция сервисов DI контейнера.</param>
        /// <param name="assembly">Сборка, в которой искать эндпоинты.</param>
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            var endpointTypes = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false } && typeof(IEndpoint).IsAssignableFrom(t));

            foreach (var type in endpointTypes)
            {
                var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
                endpoint.AddServices(services);
                services.AddSingleton(endpoint);
            }

            return services;
        }

        /// <summary>
        /// Маппит маршруты всех найденных IEndpoint.
        /// </summary>
        /// <param name="app">Приложение, через которое добавляются маршруты.</param>
        public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach (var endpoint in app.Services.GetServices<IEndpoint>())
                endpoint.Map(app);

            return app;
        }
    }
}
