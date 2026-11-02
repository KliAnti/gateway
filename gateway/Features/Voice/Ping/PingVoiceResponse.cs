namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Контракт для предоставления ответа на запрос из gateway.
    /// </summary>
    /// <param name="Message">Сообщение, возвращенное из сервиса voice.</param>
    public sealed record PingVoiceResponse(string Message);
}
