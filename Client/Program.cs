using Client;
using Grpc.Net.Client;

GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5232");
Translator.TranslatorClient client = new(channel);


#region Information
Console.WriteLine("Получаю информацию о движке перевода...");
try
{
    InfoReply? response = client.Information(new EmptyRequest());

    Console.WriteLine("Переводчик: " + response?.Translator);
    Console.WriteLine("Кэш: " + response?.Caching);
    Console.WriteLine("Размер кэша: " + response?.CacheSize);
}
catch (Exception ex)
{
    Console.WriteLine($"Произошла ошибка: {ex.Message}");
}
#endregion


#region Translate
Console.WriteLine("Введите текст для перевода");
string? line = Console.ReadLine();

Console.WriteLine("Введите язык, на который надо перевести");
string? targetLang = Console.ReadLine();

Console.WriteLine("Введите язык, с которого надо перевести (опционально)");
string? sourceLang = Console.ReadLine();


if (string.IsNullOrWhiteSpace(line))
{
    Console.WriteLine("Вы не указали текст перевода.");
    return;
}

if (string.IsNullOrWhiteSpace(targetLang))
{
    Console.WriteLine("Вы не указали конечный язык перевода.");
    return;
}


Console.WriteLine("Перевожу...");
try
{
    TranslateReply? response = client.Translate(new TranslateRequest
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
#endregion