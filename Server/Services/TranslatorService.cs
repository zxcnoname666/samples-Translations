using Grpc.Core;
using Server.Core;
using Server.Core.Translator;
using Server.Protos;

namespace Server.Services;

public class TranslatorService : Translator.TranslatorBase
{
    /// <summary>
    /// Метод gRPC, который вызывается при запросе перевода.
    /// </summary>
    public override Task<TranslateReply> Translate(TranslateRequest request, ServerCallContext context)
    {
        TranslationResponse response = Defines.Instance?.Translate([..request.Text], request.Target, request.Source) ??
                                       new TranslationResponse
                                       {
                                           IsErrored = true,
                                           ErrorMessage = "Defines Instance is not defined",
                                           Translations = []
                                       };

        return Task.FromResult(new TranslateReply
        {
            IsErrored = response.IsErrored,
            ErrorMessage = response.ErrorMessage ?? string.Empty,
            Translations = { response.Translations }
        });
    }


    /// <summary>
    /// Метод gRPC, который вызывается при запросе информации о ядре перевода.
    /// </summary>
    public override Task<InfoReply> Information(EmptyRequest request, ServerCallContext context)
    {
        return Task.FromResult(new InfoReply
        {
            Translator = Defines.Instance?.GetTranslationCore() ?? "Unknown",
            Caching = Defines.Instance?.GetCachingCore() ?? "Unknown",
            CacheSize = Defines.Instance?.GetCacheSize() ?? 0
        });
    }
}