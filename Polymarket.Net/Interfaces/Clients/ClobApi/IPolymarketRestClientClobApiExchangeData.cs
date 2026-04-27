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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/data/get-server-time" /><br />
        /// Endpoint:<br />
        /// GET /time
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get geographical restrictions for calling client
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/geoblock" /><br />
        /// Endpoint:<br />
        /// GET /api/geoblock
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get sampling simplified markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-sampling-simplified-markets" /><br />
        /// Endpoint:<br />
        /// GET /sampling-simplified-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get sampling markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-sampling-markets" /><br />
        /// Endpoint:<br />
        /// GET /sampling-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get simplified markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-simplified-markets" /><br />
        /// Endpoint:<br />
        /// GET /simplified-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/list-markets" /><br />
        /// Endpoint:<br />
        /// GET /markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketMarketDetails>>> GetMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get market by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-market-by-id" /><br />
        /// Endpoint:<br />
        /// GET /markets
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketMarketDetails>> GetMarketAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-market-price" /><br />
        /// Endpoint:<br />
        /// GET /price
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] Token id</param>
        /// <param name="side">["<c>side</c>"] Side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default);

        /// <summary>
        /// Get buy/sell prices for all markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-market-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /prices
        /// </para>
        /// </summary>
        /// <param name="tokens">Tokens to retrieve prices for</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokens, CancellationToken ct = default);

        /// <summary>
        /// Get the midpoint price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/data/get-midpoint-price" /><br />
        /// Endpoint:<br />
        /// GET /midpoint
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get mid point prices for tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-midpoint-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /midpoints
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] Tokens to request</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get price history for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-prices-history" /><br />
        /// Endpoint:<br />
        /// GET /prices-history
        /// </para>
        /// </summary>
        /// <param name="market">["<c>market</c>"] The market</param>
        /// <param name="startTime">["<c>startTs</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTs</c>"] Filter by end time</param>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="fidelity">["<c>fidelity</c>"] Fidelity in minutes</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for a a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-spread" /><br />
        /// Endpoint:<br />
        /// GET /spread
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for specified token ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-spreads" /><br />
        /// Endpoint:<br />
        /// POST /spreads
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] Token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-order-book" /><br />
        /// Endpoint:<br />
        /// GET /book
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for multiple tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-order-books-request-body" /><br />
        /// Endpoint:<br />
        /// POST /books
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get tick size for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-tick-size" /><br />
        /// Endpoint:<br />
        /// GET /tick-size
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get negative risk for a token
        /// <para>
        /// Endpoint:<br />
        /// GET /neg-risk
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get fee rate in basis points
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-fee-rate" /><br />
        /// Endpoint:<br />
        /// GET /fee-rate
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-last-trade-price" /><br />
        /// Endpoint:<br />
        /// GET /last-trade-price
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-last-trade-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /last-trades-prices
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get info on a market
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-clob-market-info" /><br />
        /// Endpoint:<br />
        /// GET /clob-markets/{tokenId}<br />
        /// </para>
        /// </summary>
        /// <param name="marketId">["<c>condition_id</c>"] Id of the market</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketMarketInfo>> GetMarketInfoAsync(string marketId, CancellationToken ct = default);


    }
}
