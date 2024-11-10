using Grpc.Net.Client;

namespace Tests.gRPC;

public static class GrpcClientTest
{
    private static TranslatorProto.TranslatorProtoClient? _client;
    
    public static void Run()
    {
        // Подключаемся к gRPC
        GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5232");
        _client = new TranslatorProto.TranslatorProtoClient(channel);

        GetInformation();
        
        (string line, string targetLang, string? sourceLang) = ReadLines();
        Translate(line, targetLang, sourceLang);
    }

    public static (string, string, string?) ReadLines()
    {
        // Запрашиваем данные для перевода

        Console.WriteLine("Введите текст для перевода");
        string? line = Console.ReadLine();

        Console.WriteLine("Введите язык, на который надо перевести");
        string? targetLang = Console.ReadLine();

        Console.WriteLine("Введите язык, с которого надо перевести (опционально)");
        string? sourceLang = Console.ReadLine();


        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("Вы не указали текст перевода.");
            throw new ArgumentNullException(nameof(line));
        }

        if (string.IsNullOrWhiteSpace(targetLang))
        {
            Console.WriteLine("Вы не указали конечный язык перевода.");
            throw new ArgumentNullException(nameof(targetLang));
        }

        return (line, targetLang, sourceLang);
    }
    
    public static void Translate(string line, string targetLang, string? sourceLang) {
        Console.WriteLine("Перевожу...");
        
        if (_client is null)
            throw new NullReferenceException(nameof(_client));
        
        try
        {
            // Отправляем запрос на перевод
            TranslateReply? response = _client.Translate(new TranslateRequest
            {
                Text = { line },
                Target = targetLang,
                Source = sourceLang
            });

            // Выводим данные из ответа
            Console.WriteLine("Is Errored: " + response.IsErrored);
            Console.WriteLine("Error Message: " + response.ErrorMessage);
            Console.WriteLine("Translations: " + string.Join(", ", response.Translations));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
        
    }

    public static void GetInformation()
    {
        Console.WriteLine("Получаю информацию о движке перевода...");
        
        if (_client is null)
            throw new NullReferenceException(nameof(_client));

        try
        {
            // Запрашиваем информацию по "движку" на сервере и объеме кэша.
            InfoReply? response = _client.Information(new EmptyRequest());

            // Выводим ответ
            Console.WriteLine("Переводчик: " + response?.Translator);
            Console.WriteLine("Кэш: " + response?.Caching);
            Console.WriteLine("Размер кэша: " + response?.CacheSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

    }
}