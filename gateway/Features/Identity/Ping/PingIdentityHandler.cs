using gateway.Clients;

namespace gateway.Features.Identity.Ping
{
    /// <summary>
    /// Необходим для вызова всех эндпоинтов (необходимых) для выполнения необходимого сценария.
    /// </summary>
    public sealed class PingIdentityHandler
    {
        private readonly IdentityGrpcClient _identityClient;

        /// <summary>
        /// Принимает клиента identity, далее собирает его и передает в DI.
        /// </summary>
        /// <param name="identityClient">Обертка над gRPC-клиентом identity.</param>
        public PingIdentityHandler(IdentityGrpcClient identityClient)
        {
            _identityClient = identityClient;
        }

        /// <summary>
        /// Отправляет identity сообщение и возвращает то, что он ответил.
        /// </summary>
        /// <param name="request">Входные данные.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Результат сценария.</returns>
        public async Task<PingIdentityResponse> HandleAsync(PingIdentityRequest request, CancellationToken cancellationToken)
        {
            var message = await _identityClient.PingAsync(request.Message, cancellationToken);
            return new PingIdentityResponse(message);
        }
    }
}
