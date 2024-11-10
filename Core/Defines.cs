using Caching;
using Translator;

namespace Core;

/// <summary>
/// Класс для ссылок из других проектов для единого доступа к ядру кэширования и перевода.
/// </summary>
public static class Defines
{
    public static ICaching? Caching { get; set; }
    public static ITranslator? Translator { get; set; }
}