namespace Translator;

/// <summary>
/// Единая структура ответа для всех движков перевода.
/// </summary>
public readonly struct TranslationResponse
{
    public bool IsErrored { get; init; }
    public string? ErrorMessage { get; init; }
    public string[] Translations { get; init; }
}