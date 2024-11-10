namespace Caching;

/// <summary>
/// Интерфейс для класса кэширования.
/// </summary>
public interface ICaching
{
    string GetCachingCore();
    long GetCacheSize();
    
    void Set(string key, string value);
    string? Get(string key);
}