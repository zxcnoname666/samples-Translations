using System.Text.Json.Serialization;

namespace Translator.Deepl.Structs;

/// <summary>
/// Структура тела запроса на Deepl API.
/// </summary>
public readonly struct ApiRequest (string[] text, string target, string? source = null)
{
    [JsonPropertyName("text")]
    public string[] Text { get; } = text;
    
    [JsonPropertyName("target_lang")]
    public string TargetLang { get; } = target;
    
    [JsonPropertyName("source_lang")]
    public string? SourceLang { get; } = source;
}