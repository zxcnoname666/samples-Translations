using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Core;
using Translator.Deepl.Structs;

namespace Translator.Deepl;

public class DeeplClient : ITranslator
{
    private readonly HttpClient _httpClient;
    
    public DeeplClient()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DeepL-Auth-Key", Config.DeeplKey);
    }

    /// <summary>
    /// Движок переводчика
    /// </summary>
    public string GetTranslationCore()
        => "Deepl";

    /// <summary>
    /// Метод, который вызывается при переводе. Вызывается за интерфейсом.
    /// </summary>
    /// <param name="text">Массив строк для перевода.</param>
    /// <param name="target">Язык, на который будет переведены изыскания. У Deepl он должен быть в ISO2</param>
    /// <param name="source">Язык, с которого переводится текст. В ISO2</param>
    /// <returns>Возвращает структуру с переводом.</returns>
    public TranslationResponse Translate(string[] text, string target, string? source)
    {
        List<string> viaApi = [];
        List<string> translations = [];

        // ищем строки в кэше, если есть - добавляем в один массив, если нет - в другой
        foreach (string search in text)
        {
            string? inCache = Defines.Caching?.Get($"Deepl:{source}:{target}:{Md5(search)}");

            if (string.IsNullOrEmpty(inCache))
                viaApi.Add(search);
            else
                translations.Add(inCache);
        }

        // возвращаем результат, если через апи переводить нечего
        if (viaApi.Count == 0)
            return new TranslationResponse
            {
                IsErrored = false,
                Translations = [..translations]
            };

        // переводим текст через апи deepl
        var task = TranslateViaApi([..viaApi], target, source);
        task.Wait();

        if (task.IsFaulted)
            return new TranslationResponse
            {
                IsErrored = true,
                ErrorMessage = task.Exception?.Message,
                Translations = [..translations]
            };

        // сохраняем кэш
        for (int i = 0; i < task.Result.Translations.Length; i++)
        {
            string translated = task.Result.Translations[i];
            string search = viaApi[i];
            Defines.Caching?.Set($"Deepl:{source}:{target}:{Md5(search)}", translated);
        }

        translations.AddRange(task.Result.Translations);
        return task.Result with { Translations = [..translations] };
    }

    /// <summary>
    /// Метод для перевода текста через API от Deepl.
    /// Документация - https://developers.deepl.com/docs/api-reference/translate
    /// </summary>
    /// <param name="text">Массив строк для перевода.</param>
    /// <param name="target">Язык, на который будет переведены изыскания. У Deepl он должен быть в ISO2</param>
    /// <param name="source">Язык, с которого переводится текст. В ISO2</param>
    /// <returns>Возвращает таск со структурой перевода</returns>
    private async Task<TranslationResponse> TranslateViaApi(string[] text, string target, string? source)
    {
        // создаем body запроса
        ApiRequest jsonObject = new(text, target, source);
        string jsonString = JsonSerializer.Serialize(jsonObject);
        
        StringContent content = new(jsonString, Encoding.UTF8, "application/json");
        
        // создаем запрос
        HttpResponseMessage response = await _httpClient.PostAsync("https://api-free.deepl.com/v2/translate", content);
        string responseString = await response.Content.ReadAsStringAsync();
        
        // возвращаем ответ в зависимости от успешности запроса
        if (response.StatusCode is HttpStatusCode.OK)
        {
            OkResponse resp = JsonSerializer.Deserialize<OkResponse>(responseString);
            
            return new TranslationResponse
            {
                IsErrored = false,
                Translations = [..resp.Translations.Select(x => x.Text)]
            };
        }
        else
        {
            ErrorResponse resp = JsonSerializer.Deserialize<ErrorResponse>(responseString);
            
            return new TranslationResponse
            {
                IsErrored = true,
                ErrorMessage = resp.Message,
                Translations = []
            };
        } // end if-else
    } // end void

    /// <summary>
    /// Extension для создания MD5 хэша
    /// </summary>
    /// <param name="source">Строка, из которой надо создать MD5 hash</param>
    /// <returns>MD5 hash</returns>
    private static string Md5(string source)
    {
        return string.Concat(MD5.HashData(Encoding.UTF8.GetBytes(source))
            .Select(b => b.ToString("x2")));
    }
} // end class