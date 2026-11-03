using System.Net.Http.Json;

namespace gateway.Tests
{
    /// <summary>
    /// Обертка вокруг HttpClient.
    /// Вызывает надстроенные собственные методы.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Принимает и собирает готовый HttpClient.
        /// Далее передает его в GatewayFixture.
        /// </summary>
        /// <param name="httpClient">HTTP-клиент, настроенный на тестовый хост gateway.</param>
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Отправляет GET-запрос по указанному пути.
        /// </summary>
        /// <param name="url">Путь эндпоинта.</param>
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _httpClient.GetAsync(url);
        }
    }
}
