using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PerpsApi
{
    /// <summary>
    /// Polymarket Perps API exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPolymarketRestClientPerpsApiExchangeData
    {
        /// <summary>
        /// Get exchange engine info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-exchange-info" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/exchange<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-collateral-assets" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/assets<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketCollateralAsset[]>> GetCollateralAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get available instruments/symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-instruments" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/instruments<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="symbolType">["<c>instrument_type</c>"] Filter by symbol type</param>
        /// <param name="category">["<c>category</c>"] Filter by category</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSymbol[]>> GetSymbolsAsync(
            long? symbolId = null,
            InstrumentType? symbolType = null,
            Category? category = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get symbol price tickers
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-tickers" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/tickers<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTicker[]>> GetTickersAsync(long? symbolId = null, CancellationToken ct = default);

        /// <summary>
        /// Get price statistics
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-statistics" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/statistics<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketStatistics[]>> GetStatisticsAsync(long? symbolId = null, CancellationToken ct = default);

        /// <summary>
        /// Get exchange statistics
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-exchange-statistics" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/exchange-stats<br />
        /// </para>
        /// </summary>
        /// <param name="startTime">["<c>start_time</c>"] Start time</param>
        /// <param name="endTime">["<c>end_time</c>"] End time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketExchangeStatistics>> GetExchangeStatisticsAsync(
            DateTime startTime,
            DateTime endTime,
            CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-klines" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/klines<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketKline>>> GetKlinesAsync(
            long symbolId,
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-mark-price-history" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/mark-history<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketMarkPriceHistory>>> GetMarkPriceHistoryAsync(
            long symbolId,
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null, 
            CancellationToken ct = default);

        /// <summary>
        /// Get the best ask/bid prices
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-bbo" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/bbo<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketBookTicker[]>> GetBookTickerAsync(long? symbolId = null, CancellationToken ct = default);

        /// <summary>
        /// Get the order book for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-book" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/book<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="depth">["<c>depth</c>"] Book depth, 10, 100, 500 or 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderBook>> GetOrderBookAsync(
            long symbolId,
            int? depth = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get index info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-index" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/index<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset name</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketIndex>> GetIndexAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get recent trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-recent-trades" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/trades<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpTrade>>> GetRecentTradesAsync(
            long symbolId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get the portfolio overview for an address
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-public-portfolio" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/portfolio<br />
        /// </para>
        /// </summary>
        /// <param name="address">["<c>address</c>"] The address to get info for. If not provided uses authentication address</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPortfolio>> GetPortfolioAsync(string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-historical-funding" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/funding<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketFundingRate>>> GetFundingRateHistoryAsync(
            long symbolId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get trading fee schedules
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-fees" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/fees<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketFeeRate[]>> GetFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get limit tiers
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-limit-tiers" /><br />
        /// Endpoint:<br />
        /// POST /v1/info/limit-tiers<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketLimitTier[]>> GetLimitTiersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get if the address is registered for perps
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-registration" /><br />
        /// Endpoint:<br />
        /// GET /v1/info/registered<br />
        /// </para>
        /// </summary>
        /// <param name="address">["<c>address</c>"] The address to check</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketRegistered>> GetIsRegisteredAsync(string address, CancellationToken ct = default);

    }
}
