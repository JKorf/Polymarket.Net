using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientClobApiExchangeData : IPolymarketRestClientClobApiExchangeData
    {
        private readonly PolymarketRestClientClobApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal PolymarketRestClientClobApiExchangeData(ILogger logger, PolymarketRestClientClobApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "time", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _baseClient.SendAsync<long>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, DateTimeConverter.ConvertFromSeconds(result.Data)!);
        }

        #endregion

        #region Get Geographic Restrictions

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "https://polymarket.com", "api/geoblock", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketGeoRestriction>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Simplified Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "sampling-simplified-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarket>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "sampling-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Simplified Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "simplified-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarket>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarketDetails>>> GetMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMarketDetails>> GetMarketAsync(string marketId, CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "markets/" + marketId, PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketMarketDetails>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            parameters.Add("side", side);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "price", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<PolymarketPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Prices

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketPriceRequest
                {
                    TokenId = x.Key,
                    Side = x.Value
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "prices", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<Dictionary<string, PolymarketBuySellPrice>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Midpoint Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "midpoint", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketMidPrice>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Midpoint Prices

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "midpoints", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("market", market);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            parameters.Add("interval", interval);
            parameters.Add("fidelity", fidelity);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/prices-history", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketPriceHistoryWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<PolymarketPriceHistory[]>(result);

            return HttpResult.Ok(result, result.Data.History);
        }

        #endregion

        #region Get Bid Ask Spread

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "spread", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketSpread>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bid Ask Spreads

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "spreads", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Info

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "book", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Infos

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "books", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tick Size

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "tick-size", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<PolymarketTickSize>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Negative Risk

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "neg-risk", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketNegRisk>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Rate Bps

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "fee-rate", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketFeeRateBps>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "last-trade-price", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketTradePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Prices

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "last-trades-prices", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _baseClient.SendAsync<PolymarketTradePrice[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market Info

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMarketInfo>> GetMarketInfoAsync(string marketId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/clob-markets/{marketId}", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketMarketInfo>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
