namespace gateway.Features.Identity.Ping
{
    /// <summary>
    /// Входные данные из gateway.
    /// </summary>
    /// <param name="Message">Сообщение, которое отправим в identity.</param>
    public sealed record PingIdentityRequest(string Message);
}
