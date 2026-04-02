using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Interfaces.Clients.DataApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.DataApi
{
    internal partial class PolymarketRestClientDataApi : RestApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketRestClientDataApi
    {
        #region fields 
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        public new PolymarketRestOptions ClientOptions => (PolymarketRestOptions)base.ClientOptions;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PolymarketRestMessageHandler(PolymarketErrors.Errors);
        #endregion

        #region constructor/destructor
        internal PolymarketRestClientDataApi(ILogger logger, HttpClient? httpClient, PolymarketRestOptions options)
            : base(logger, httpClient, options.Environment.DataRestClientAddress, options, options.DataOptions) {
            RequestBodyEmptyContent = "";
            ArraySerialization = ArrayParametersSerialization.MultipleValues;

            OrderParameters = false;
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);

        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

        public async Task<WebCallResult<PolymarketPosition[]>> GetPositionsAsync(string user, CancellationToken ct = default) {
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"positions?user={user}", PolymarketPlatform.RateLimiter.DataApi, 1, false);
            return await SendAsync<PolymarketPosition[]>(request, null, ct).ConfigureAwait(false);
        }
    }
}
