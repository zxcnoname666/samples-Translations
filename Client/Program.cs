using Core;


Console.WriteLine("Инициализация...");
Initializer.Run.DefineCore();

#region Information
Console.WriteLine("Получаю информацию о движке перевода...");
try
{
    // Выводим ответ
    Console.WriteLine("Переводчик: " + Defines.Translator?.GetTranslationCore());
    Console.WriteLine("Кэш: " + Defines.Caching?.GetCachingCore());
    Console.WriteLine("Размер кэша: " + Defines.Caching?.GetCacheSize());
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
    var response = Defines.Translator?.Translate([line], targetLang, sourceLang);

    // Выводим данные из ответа
    Console.WriteLine("Is Errored: " + response?.IsErrored);
    Console.WriteLine("Error Message: " + response?.ErrorMessage);
    Console.WriteLine("Translations: " + string.Join(", ", response?.Translations ?? []));
}
catch (Exception ex)
{
    Console.WriteLine($"Произошла ошибка: {ex.Message}");
}
#endregion