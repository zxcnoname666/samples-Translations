using Caching.Redis;
using Core;
using Translator.Deepl;

namespace Initializer;

public static class Run
{
    /// <summary>
    /// Создает классы перевода и кэширования и объявляет их в Defines.
    /// </summary>
    public static void DefineCore()
    {
        Defines.Translator = new DeeplClient();
        Defines.Caching = new RedisClient();
    }
}