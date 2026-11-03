namespace gateway.Infrastructure.Endpoints
{
    /// <summary>
    /// Позволяет удобно регестрировать зависимости и маппить маршруты в DI.
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// Регистрирует зависимости, нужные переданному эндпоинту.
        /// </summary>
        /// <param name="services">Коллекция сервисов DI контейнера.</param>
        void AddServices(IServiceCollection services);

        /// <summary>
        /// Маппит маршруты этого эндпоинта.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        void Map(IEndpointRouteBuilder app);
    }
}
