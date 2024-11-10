using System.Text.Json.Serialization;

namespace Translator.Deepl.Structs;

/// <summary>
/// Структура неудачного ответа от Deepl API.
/// </summary>
public readonly struct ErrorResponse
{
    [JsonPropertyName("message")]
    public string Message { get; init; }
}