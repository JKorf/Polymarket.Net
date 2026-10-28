using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static CryptoExchange.Net.SharedApis.SharedCapabilities;

namespace Polymarket.Net.Clients.PerpsApi.Rest
{
    internal class PolymarketRestClientPerpsApiExchangeData : IPolymarketRestClientPerpsApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPerpsApi _baseClient;

        public PolymarketRestClientPerpsApiExchangeData(PolymarketRestClientPerpsApi client)
        {
            _baseClient = client;
        }

        #region Get Exchange Info

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/exchange", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketExchangeInfo>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Collateral Assets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketCollateralAsset[]>> GetCollateralAssetsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/assets", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketCollateralAsset[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSymbol[]>> GetSymbolsAsync(
            long? symbolId = null,
            InstrumentType? symbolType = null,
            Category? category = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("instrument_type", symbolType);
            parameters.Add("category", category);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/instruments", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTicker[]>> GetTickersAsync(long? symbolId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/tickers", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Statistics

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketStatistics[]>> GetStatisticsAsync(long? symbolId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/statistics", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketStatistics[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Exchange Statistics

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketExchangeStatistics>> GetExchangeStatisticsAsync(
            DateTime startTime,
            DateTime endTime,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/exchange-stats", PolymarketPlatform.RateLimiter.PerpsApi, 10, false);
            var result = await _baseClient.SendAsync<PolymarketExchangeStatistics>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketKline>>> GetKlinesAsync(
            long symbolId,
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("interval", interval);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/klines", PolymarketPlatform.RateLimiter.PerpsApi, 5, false);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketKline>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Mark Price History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketMarkPriceHistory>>> GetMarkPriceHistoryAsync(
            long symbolId,
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("interval", interval);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/mark-history", PolymarketPlatform.RateLimiter.PerpsApi, 5, false);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketMarkPriceHistory>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketBookTicker[]>> GetBookTickerAsync(long? symbolId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/bbo", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketBookTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderBook>> GetOrderBookAsync(
            long symbolId,
            int? depth = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("depth", depth);
            var weight = depth <= 10 ? 2 :
                depth <= 100 ? 5 :
                depth <= 500 ? 10 :
                20;
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/book", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderBook>(request, parameters, ct, weight: weight).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketIndex>> GetIndexAsync(string asset, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("asset", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/index", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketIndex>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpTrade>>> GetRecentTradesAsync(
            long symbolId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/trades", PolymarketPlatform.RateLimiter.PerpsApi, 10, false);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Portfolio

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPortfolio>> GetPortfolioAsync(string? address = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("address", address ?? _baseClient.AuthenticationProvider?.ApiCredentials.L1?.Key);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/portfolio", PolymarketPlatform.RateLimiter.PerpsApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketPortfolio>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketFundingRate>>> GetFundingRateHistoryAsync(
            long symbolId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/funding", PolymarketPlatform.RateLimiter.PerpsApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketFundingRate>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Fees

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketFeeRate[]>> GetFeesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/fees", PolymarketPlatform.RateLimiter.PerpsApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketFeeRatesWrapper>(request, null, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PolymarketFeeRate[]>(result) : HttpResult.Ok(result, result.Data.FeeSchedule);
        }

        #endregion

        #region Get Limit Tiers

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketLimitTier[]>> GetLimitTiersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/limit-tiers", PolymarketPlatform.RateLimiter.PerpsApi, 2, false);
            var result = await _baseClient.SendAsync<PolymarketLimitTier[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Is Registered

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketRegistered>> GetIsRegisteredAsync(string address, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("address", address);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/info/registered", PolymarketPlatform.RateLimiter.PerpsApi, 1, false);
            var result = await _baseClient.SendAsync<PolymarketRegistered>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
