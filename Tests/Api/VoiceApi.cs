using gateway.Tests.Helpers;
using gateway.Tests.Routes;

namespace gateway.Tests.Api
{
    /// <summary>
    /// Все эндпоинты сервиса voice.
    /// </summary>
    public class VoiceApi
    {
        private readonly ApiClient _client;

        /// <summary>
        /// Принимает общий ApiClient.
        /// </summary>
        /// <param name="client">Общий клиент хоста gateway.</param>
        public VoiceApi(ApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Дергает /api/voice/ping.
        /// </summary>
        /// <param name="message">Текст, отправляемый в voice.</param>
        public Task<HttpResponseMessage> Ping(string? message = null)
        {
            return _client.GetAsync(UrlBuilder.WithQueryParam(VoiceRoutes.Ping, "message", message));
        }
    }
}
