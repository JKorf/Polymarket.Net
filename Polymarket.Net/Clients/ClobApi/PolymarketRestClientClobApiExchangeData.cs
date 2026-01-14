using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
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
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "time", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<long>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Success ? DateTimeConverter.ConvertFromSeconds(result.Data) : default);
        }

        #endregion

        #region Get Geographic Restrictions

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/geoblock", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendToAddressAsync<PolymarketGeoRestriction>("https://polymarket.com", request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Simplified Markets

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sampling-simplified-markets", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarket>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Markets

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sampling-markets", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Simplified Markets

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "simplified-markets", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarket>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Markets

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "markets", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketMarketDetails>> GetMarketAsync(string conditionId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "markets/" + conditionId, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketMarketDetails>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            parameters.AddEnum("side", side);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "price", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Prices

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokenIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(tokenIds.Select(x =>            
                new PolymarketPriceRequest
                {
                    TokenId = x.Key,
                    Side = x.Value
                }
            ).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "prices", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<Dictionary<string, PolymarketBuySellPrice>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Midpoint Price

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "midpoint", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<PolymarketMidPrice>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Midpoint Prices

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "midpoints", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price History

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("market", market);
            parameters.AddOptionalMilliseconds("startTs", startTime);
            parameters.AddOptionalMilliseconds("endTs", endTime);
            parameters.AddOptionalEnum("interval", interval);
            parameters.AddOptional("fidelity", fidelity);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/prices-history", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<PolymarketPriceHistoryWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<PolymarketPriceHistory[]>(result.Data?.History);
        }

        #endregion

        #region Get Bid Ask Spread

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "spread", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<PolymarketSpread>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bid Ask Spreads

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "spreads", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Info

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "book", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<PolymarketOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Infos

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "books", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await _baseClient.SendAsync<PolymarketOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tick Size

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "tick-size", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketTickSize>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Negative Risk

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "neg-risk", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketNegRisk>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Rate Bps

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fee-rate", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketFeeRateBps>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "last-trade-price", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketTradePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Prices

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "last-trades-prices", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketTradePrice[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
