using Core;
using Grpc.Core;
using Server.Protos;
using Translator;

namespace Server.Services;

public class TranslatorService : TranslatorProto.TranslatorProtoBase
{
    /// <summary>
    /// Метод gRPC, который вызывается при запросе перевода.
    /// </summary>
    public override Task<TranslateReply> Translate(TranslateRequest request, ServerCallContext context)
    {
        TranslationResponse response = Defines.Translator?.Translate([..request.Text], request.Target, request.Source) ??
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
            Translator = Defines.Translator?.GetTranslationCore() ?? "Unknown",
            Caching = Defines.Caching?.GetCachingCore() ?? "Unknown",
            CacheSize = Defines.Caching?.GetCacheSize() ?? 0
        });
    }
}