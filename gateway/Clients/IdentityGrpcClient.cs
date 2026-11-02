using IdentityClient = identity.Grpc.Identity.IdentityClient;
using IdentityPingRequest = identity.Grpc.PingRequest;

namespace gateway.Clients
{
    /// <summary>
    /// Ходит в сервис identity по gRPC и переводит его ответы в формат, понятный внешнему миру.
    /// </summary>
    public sealed class IdentityGrpcClient
    {
        private readonly IdentityClient _grpcClient;

        /// <summary>
        /// Принимает уже готового gRPC-клиента identity — его собирает и передаёт DI-контейнер.
        /// </summary>
        /// <param name="grpcClient">gRPC-клиент, через который идут вызовы к сервису identity.</param>
        public IdentityGrpcClient(IdentityClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        /// <summary>
        /// Отправляет identity тестовое сообщение и возвращает то, что он ответил.
        /// </summary>
        /// <param name="message">Текст, который нужно отправить в identity.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Ответ identity в собсвтенном формате gateway.</returns>
        public async Task<string> PingAsync(string message, CancellationToken cancellationToken)
        {
            var grpcResponse = await _grpcClient.PingAsync(
                new IdentityPingRequest 
                { 
                    Message = message 
                },
                cancellationToken: cancellationToken);

            return grpcResponse.Message;
        }
    }
}
