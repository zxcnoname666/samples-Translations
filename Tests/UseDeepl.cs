using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Tests;

public static class UseDeepl
{
    private static readonly HttpClient HttpClient = new();

    static UseDeepl()
    {
        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("DeepL-Auth-Key", "...");
    }

    public async static Task Run()
    {
        ApiStruct jsonObject = new(["text", "zxc", "hi! how a'u?"], "RU", "DE");
        string jsonString = JsonSerializer.Serialize(jsonObject);

        Console.WriteLine("JSON request:");
        Console.WriteLine(jsonString);

        StringContent content = new(jsonString, Encoding.UTF8, "application/json");

        try
        {
            Console.WriteLine("1.1");
            HttpResponseMessage response = await HttpClient.PostAsync("https://api-free.deepl.com/v2/translate", content);
            Console.WriteLine("1.2");
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("1.3");

            Console.WriteLine("Response:");
            Console.WriteLine("StatusCode: " + response.StatusCode);
            Console.WriteLine("IsNullOrEmpty: " + string.IsNullOrEmpty(responseString));
            Console.WriteLine(responseString);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ErrorResponse resp = JsonSerializer.Deserialize<ErrorResponse>(responseString);
                Console.WriteLine("--- ErrorResponse: ---");
                Console.WriteLine(resp.Message);
            }
            else
            {
                OkResponse resp = JsonSerializer.Deserialize<OkResponse>(responseString);
                Console.WriteLine("--- OkResponse: ---");
                
                Console.WriteLine("Translations:");
                Console.WriteLine(resp.Translations);
                
                Console.WriteLine("Translations.Length:");
                Console.WriteLine(resp.Translations.Length);
                
                if (resp.Translations.Length > 0)
                {
                    Console.WriteLine("Translations[0].Text:");
                    Console.WriteLine(resp.Translations[0].Text);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}