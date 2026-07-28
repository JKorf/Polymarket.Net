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
using Polymarket.Net.Enums;
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
        internal PolymarketRestClientDataApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, PolymarketRestOptions options)
            : base(loggerFactory, PolymarketPlatform.Metadata.Id, httpClient, options.Environment.DataRestClientAddress, options, options.DataOptions) {
            RequestBodyEmptyContent = "";
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);

        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

        public async Task<HttpResult<PolymarketPosition[]>> GetPositionsAsync(
			string user,
			string? market = null,
			string? eventId = null,
			decimal? sizeThreshold = null,
			bool? redeemable = null,
			bool? mergeable = null,
			int? limit = null,
			int? offset = null,
			PositionSortBy? sortBy = null,
			SortDirection? sortDirection = null,
			string? title = null,
			CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("user", user);
            parameters.Add("market", market);
            parameters.Add("eventId", eventId);
            parameters.Add("sizeThreshold", sizeThreshold);
            parameters.Add("redeemable", redeemable);
            parameters.Add("mergeable", mergeable);
            parameters.Add("limit", limit);
            parameters.Add("offset", offset);
            parameters.Add("sortBy", sortBy);
			parameters.Add("sortDirection", sortDirection);
			parameters.Add("title", title);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "positions", PolymarketPlatform.RateLimiter.DataApi, 1, false);
            return await SendAsync<PolymarketPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }

		public async Task<HttpResult<PolymarketClosedPosition[]>> GetClosedPositionsAsync(
			string user,
			string? market = null,
			string? eventId = null,
			int? limit = null,
			int? offset = null,
			ClosedPositionSortBy? sortBy = null,
			SortDirection? sortDirection = null,
			string? title = null,
			CancellationToken ct = default) {
			var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
			parameters.Add("user", user);
			parameters.Add("market", market);
			parameters.Add("eventId", eventId);
			parameters.Add("limit", limit);
			parameters.Add("offset", offset);
			parameters.Add("sortBy", sortBy);
			parameters.Add("sortDirection", sortDirection);
			parameters.Add("title", title);
			var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "closed-positions", PolymarketPlatform.RateLimiter.DataApi, 1, false);
			return await SendAsync<PolymarketClosedPosition[]>(request, parameters, ct).ConfigureAwait(false);
		}
	}
}
