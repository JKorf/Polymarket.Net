using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.MessageHandlers
{
    internal class PolymarketRestPerpsMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = PolymarketPlatform._serializerContext;

        public PolymarketRestPerpsMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (error != null)
                return error;

            string? msg = null;
            if (document!.RootElement.ValueKind == JsonValueKind.Array)
            {
                msg = document!.RootElement[0].TryGetProperty("error", out var msgProp) ? msgProp.GetString() : null;
            }
            else
            {
                msg = document!.RootElement.TryGetProperty("error", out var msgProp) ? msgProp.GetString() : null;
            }
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown);

            var errorInfo = _errorMapping.GetErrorInfo(msg, msg);
            return new ServerError(msg, errorInfo);

        }
    }
}
