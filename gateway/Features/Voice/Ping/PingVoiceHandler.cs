using gateway.Clients;

namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Необходим для вызова всех эндпоинтов (необходимых) для выполнения необходимого сценария.
    /// </summary>
    public sealed class PingVoiceHandler
    {
        private readonly VoiceGrpcClient _voiceClient;

        /// <summary>
        /// Принимает клиента voice, далее собирает его и передает в DI.
        /// </summary>
        /// <param name="voiceClient">Обертка над gRPC-клиентом voice.</param>
        public PingVoiceHandler(VoiceGrpcClient voiceClient)
        {
            _voiceClient = voiceClient;
        }

        /// <summary>
        /// Отправляет voice сообщение и возвращает то, что он ответил.
        /// </summary>
        /// <param name="request">Входные данные.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Результат сценария.</returns>
        public async Task<PingVoiceResponse> HandleAsync(PingVoiceRequest request, CancellationToken cancellationToken)
        {
            var message = await _voiceClient.PingAsync(request.Message, cancellationToken);
            return new PingVoiceResponse(message);
        }
    }
}
