namespace gateway.Tests.Helpers
{
    /// <summary>
    /// Собираем url с query параметрами.
    /// Утилита инфраструктурного уровня, используемая API классами.
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        /// Добавляет к пути query параметр, если значение задано.
        /// </summary>
        /// <param name="route">Путь эндпоинта.</param>
        /// <param name="paramName">Имя query параметра.</param>
        /// <param name="value">Значение query параметра (может быть null).</param>
        public static string WithQueryParam(string route, string paramName, string? value)
        {
            return value is null ? route : $"{route}?{paramName}={Uri.EscapeDataString(value)}";
        }
    }
}
