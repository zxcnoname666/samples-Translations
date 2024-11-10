using System.Text.Json.Serialization;

namespace Translator.Deepl.Structs;

/// <summary>
/// Структура успешного ответа от Deepl API.
/// </summary>
public readonly struct OkResponse
{
    [JsonPropertyName("translations")]
    public OkTranslations[] Translations { get; init; }
}

public readonly struct OkTranslations
{
    [JsonPropertyName("detected_source_language")]
    public string DetectedSourceLanguage { get; init; }
    
    [JsonPropertyName("text")]
    public string Text { get; init; }
}