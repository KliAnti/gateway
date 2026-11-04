using gateway.Tests.Helpers;
using gateway.Tests.Routes;

namespace gateway.Tests.Api
{
    /// <summary>
    /// Все эндпоинты сервиса identity.
    /// </summary>
    public class IdentityApi
    {
        private readonly ApiClient _client;

        /// <summary>
        /// Принимает общий ApiClient.
        /// </summary>
        /// <param name="client">Общий клиент хоста gateway.</param>
        public IdentityApi(ApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Дергает /api/identity/ping.
        /// </summary>
        /// <param name="message">Текст, отправляемый в identity.</param>
        public Task<HttpResponseMessage> Ping(string? message = null)
        {
            return _client.GetAsync(UrlBuilder.WithQueryParam(IdentityRoutes.Ping, "message", message));
        }
    }
}
