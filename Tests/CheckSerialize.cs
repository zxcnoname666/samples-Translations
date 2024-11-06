using System.Text;
using System.Text.Json;

namespace Tests;

public static class CheckSerialize
{
    public static void Run()
    {
        ApiStruct jsonObject = new(["text", "zxc"], "RU", null);
        string jsonString = JsonSerializer.Serialize(jsonObject);
        
        Console.WriteLine(jsonString);

        StringContent content = new(jsonString, Encoding.UTF8, "application/json");
        Console.WriteLine(content.ToString());

    }
}