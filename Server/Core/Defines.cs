using Server.Core.Caching;
using Server.Core.Translator;
using RedisClient = Server.Core.Caching.Redis.Client;
using DeeplClient = Server.Core.Translator.Deepl.Client;

namespace Server.Core;

/// <summary>
/// Единый класс с интерфейсами кэширования и перевода.
/// </summary>
public class Defines : ICaching, ITranslator
{
    /// <summary>
    /// Статичный инстанс для доступа из статичных классов.
    /// </summary>
    public static Defines? Instance { get; private set; }
    
    public ICaching Caching { get; }
    public ITranslator Translator { get; }

    public Defines()
    {
        Caching = new RedisClient();
        Translator = new DeeplClient(Config.DeeplKey);

        Instance = this;
    }

    /// <summary>
    /// Статичный метод, создающий новый экземпляр данного класса.
    /// </summary>
    public static void Create()
    {
        Instance = new Defines();
    }
    
    
    /// <summary>
    /// Метод, получающий название движка для кэширования.
    /// </summary>
    public string GetCachingCore()
        => Caching.GetCachingCore();

    /// <summary>
    /// Метод, получающий название движка переводчика.
    /// </summary>
    public long GetCacheSize()
        => Caching.GetCacheSize();
    
    
    /// <summary>
    /// Метод кэширования.
    /// Рекурсивно добавляет значение по ключу в хэш.
    /// </summary>
    /// <param name="key">Ключ для кэша</param>
    /// <param name="value">Значение кэша</param>
    public void Set(string key, string value)
    => Caching.Set(key, value);
    
    /// <summary>
    /// Метод кэширования.
    /// Получает значение по ключу из кэша.
    /// </summary>
    /// <param name="key">Ключ для кэша</param>
    /// <returns>Значение кэша в виде строки. Может быть null</returns>
    public string? Get(string key)
    => Caching.Get(key);
    
    
    /// <summary>
    /// Метод перевода.
    /// Переводит массив строк на указанный язык.
    /// </summary>
    /// <param name="text">Массив строк для перевода.</param>
    /// <param name="target">Язык, на который будет переведены изыскания. У Deepl он должен быть в ISO2</param>
    /// <param name="source">Язык, с которого переводится текст. В ISO2</param>
    /// <returns>Возвращает структуру с переводом.</returns>
    public TranslationResponse Translate(string[] text, string target, string? source)
    => Translator.Translate(text, target, source);
    
    /// <summary>
    /// Метод, получающий название движка переводчика.
    /// </summary>
    public string GetTranslationCore()
    => Translator.GetTranslationCore();
}