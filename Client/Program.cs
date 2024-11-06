using Client;
using Grpc.Net.Client;

// Подключаемся к gRPC
GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5232");
Translator.TranslatorClient client = new(channel);


#region Information
Console.WriteLine("Получаю информацию о движке перевода...");
try
{
    // Запрашиваем информацию по "движку" на сервере и объеме кэша.
    InfoReply? response = client.Information(new EmptyRequest());

    // Выводим ответ
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
    // Отправляем запрос на перевод
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