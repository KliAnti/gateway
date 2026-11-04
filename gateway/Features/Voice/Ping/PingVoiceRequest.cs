namespace gateway.Features.Voice.Ping
{
    /// <summary>
    /// Входные данные из gateway.
    /// </summary>
    /// <param name="Message">Сообщение, которое отправим в voice.</param>
    public sealed record PingVoiceRequest(string Message);
}
