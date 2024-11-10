using StackExchange.Redis;

namespace Caching.Redis;

public class RedisClient : ICaching
{
    private readonly ConnectionMultiplexer _connection;
    private readonly IDatabase _database;

    public RedisClient()
    {
        _connection = ConnectionMultiplexer.Connect(Config.RedisHost);
        _database = _connection.GetDatabase();
    }

    /// <summary>
    /// Движок кэширования
    /// </summary>
    public string GetCachingCore()
        => "Redis";

    /// <summary>
    /// Получает размер кэша
    /// </summary>
    public long GetCacheSize()
    {
        var server = _connection.GetServers();
        return server.Sum(x => x.DatabaseSize());
    }

    /// <summary>
    /// Рекурсивно добавляет значение по ключу в хэш.
    /// </summary>
    /// <param name="key">Ключ для кэша</param>
    /// <param name="value">Значение кэша</param>
    public void Set(string key, string value)
    {
        _database.StringSet(key, value);
    }

    /// <summary>
    /// Получает значение по ключу из кэша.
    /// </summary>
    /// <param name="key">Ключ для кэша</param>
    /// <returns>Значение кэша в виде строки. Может быть null</returns>
    public string? Get(string key)
    {
        return _database.StringGet(key);
    }
}