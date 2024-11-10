namespace Translator;

/// <summary>
/// Интерфейс для класса переводчика.
/// </summary>
public interface ITranslator
{
    string GetTranslationCore();
    
    TranslationResponse Translate(string[] text, string target, string? source);
}