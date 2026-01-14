using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.ClobApi
{
    /// <summary>
    /// Polymarket Clob exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPolymarketRestClientClobApiExchangeData
    {
        /// <summary>
        /// Get server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get geographical restrictions for calling client
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/geoblock" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get sampling simplified markets
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get sampling markets
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get simplified markets
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get markets
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get market by condition id
        /// </summary>
        /// <param name="conditionId">Condition id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketMarketDetails>> GetMarketAsync(string conditionId, CancellationToken ct = default);

        /// <summary>
        /// Get price for a token
        /// <para><a href="https://docs.polymarket.com/api-reference/pricing/get-market-price" /></para>
        /// </summary>
        /// <param name="tokenId">Token id</param>
        /// <param name="side">Side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default);

        /// <summary>
        /// Get buy/sell prices for all markets
        /// <para><a href="https://docs.polymarket.com/api-reference/pricing/get-multiple-market-prices" /></para>
        /// </summary>
        /// <param name="tokens">Tokens to retrieve prices for</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokens, CancellationToken ct = default);

        /// <summary>
        /// Get the midpoint price for a token
        /// <para><a href="https://docs.polymarket.com/api-reference/pricing/get-midpoint-price" /></para>
        /// </summary>
        /// <param name="tokenId">The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get mid point prices for tokens
        /// </summary>
        /// <param name="tokenIds">Tokens to request</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get price history for a token
        /// <para><a href="https://docs.polymarket.com/api-reference/pricing/get-price-history-for-a-traded-token" /></para>
        /// </summary>
        /// <param name="market">The market</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="interval">Interval</param>
        /// <param name="fidelity">Fidelity in minutes</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for a a token
        /// </summary>
        /// <param name="tokenId">Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for specified token ids
        /// <para><a href="https://docs.polymarket.com/api-reference/spreads/get-bid-ask-spreads" /></para>
        /// </summary>
        /// <param name="tokenIds">Token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for a token
        /// <para><a href="https://docs.polymarket.com/api-reference/orderbook/get-order-book-summary" /></para>
        /// </summary>
        /// <param name="tokenId">The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for multiple tokens
        /// <para><a href="https://docs.polymarket.com/api-reference/orderbook/get-multiple-order-books-summaries-by-request" /></para>
        /// </summary>
        /// <param name="tokenIds">The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get tick size for a token
        /// </summary>
        /// <param name="tokenId">The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get negative risk for a token
        /// </summary>
        /// <param name="tokenId">The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get fee rate in basis points
        /// </summary>
        /// <param name="tokenId">The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for a token
        /// </summary>
        /// <param name="tokenId">The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for tokens
        /// </summary>
        /// <param name="tokenIds">The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

    }
}
