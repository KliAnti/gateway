using System.Net.Http.Json;

namespace gateway.Tests.Helpers
{
    /// <summary>
    /// Десериализация тела http ответа в нужный тип.
    /// </summary>
    public static class JsonResponseReader
    {
        /// <summary>
        /// Десериализует тело http ответа в указанный тип.
        /// </summary>
        /// <param name="response">Тело http ответа.</param>
        public static Task<TResponse?> ReadAsAsync<TResponse>(HttpResponseMessage response)
        {
            return response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}
