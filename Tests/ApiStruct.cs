using System.Text.Json.Serialization;

namespace Tests;

public readonly struct ApiStruct (string[] text, string target, string? source = null)
{
    [JsonPropertyName("text")]
    public string[] Text { get; } = text;
    
    [JsonPropertyName("target_lang")]
    public string TargetLang { get; } = target;
    
    [JsonPropertyName("source_lang")]
    public string? SourceLang { get; } = source;
}

public readonly struct ErrorResponse
{
    [JsonPropertyName("message")]
    public string Message { get; init; }
}

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