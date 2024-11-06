namespace Server;

public static class Config
{
    /// <summary>
    /// API ключ для доступа к API Deepl.
    /// </summary>
    public static string DeeplKey => "TOKEN HERE";
    
    /// <summary>
    /// Host сервера Redis для подключения.
    /// </summary>
    public static string RedisHost => "localhost";
}