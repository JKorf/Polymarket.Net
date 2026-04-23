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
    /// Polymarket Clob trading endpoints, placing and managing orders.
    /// </summary>
    public interface IPolymarketRestClientClobApiTrading
    {
        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/get-active-order" /><br />
        /// Endpoint:<br />
        /// GET /data/orders
        /// </para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="conditionId">Filter by market/condition id</param>
        /// <param name="tokenId">Asset/token id</param>
        /// <param name="cursor">Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(string? orderId = null, string? conditionId = null, string? tokenId = null, string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get an order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/get-order" /><br />
        /// Endpoint:<br />
        /// GET /data/order/{orderId}
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>order_id</c>"] Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrder>> GetOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Check if an order is eligible or scoring for Rewards purposes
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/check-scoring" /><br />
        /// Endpoint:<br />
        /// GET /order-scoring
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderScoring>> GetOrderRewardScoringAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Check if orders are eligible or scoring for Rewards purposes
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/check-scoring" /><br />
        /// Endpoint:<br />
        /// POST /orders-scoring
        /// </para>
        /// </summary>
        /// <param name="orderIds">Order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, bool>>> GetOrdersRewardScoringAsync(IEnumerable<string> orderIds, CancellationToken ct = default);

		/// <summary>
		/// Place a new limit order. Quantity represents the asset/shares amount to sell/buy.
		/// <para>
		/// Docs:<br />
		/// <a href="https://docs.polymarket.com/developers/CLOB/orders/create-order" /><br />
		/// Endpoint:<br />
		/// POST /order
		/// </para>
		/// </summary>
		Task<WebCallResult<PolymarketOrderResult>> PlaceOrderAsync(
            PolymarketOrderRequest request,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new market order. For BUY orders, amount represents the USDC spend amount.
        /// For SELL orders, amount represents the asset/shares amount.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/create-order" /><br />
        /// Endpoint:<br />
        /// POST /order
        /// </para>
        /// </summary>
        /// <param name="request">Market order request</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderResult>> PlaceMarketOrderAsync(
            PolymarketMarketOrderRequest request,
            CancellationToken ct = default);

		/// <summary>
		/// Place multiple limit orders in a single request. Quantity represents the asset/shares amount to sell/buy.
		/// <para>
		/// Docs:<br />
		/// <a href="https://docs.polymarket.com/developers/CLOB/orders/create-order-batch" /><br />
		/// Endpoint:<br />
		/// POST /orders
		/// </para>
		/// </summary>
		/// <param name="requests">Order requests</param>
		/// <param name="ct">Cancellation token</param>
		Task<WebCallResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<PolymarketOrderRequest> requests, CancellationToken ct = default);

        /// <summary>
        /// Place multiple market orders in a single request. For BUY orders, amount represents the USDC spend amount.
        /// For SELL orders, amount represents the asset/shares amount.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/create-order-batch" /><br />
        /// Endpoint:<br />
        /// POST /orders
        /// </para>
        /// </summary>
        /// <param name="requests">Market order requests</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleMarketOrdersAsync(IEnumerable<PolymarketMarketOrderRequest> requests, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /order
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderID</c>"] Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default);
        /// <summary>
        /// Cancel multiple orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /orders
        /// </para>
        /// </summary>
        /// <param name="orderIds">Ids of orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default);
        /// <summary>
        /// Cancel all orders for a specific market
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /orders
        /// </para>
        /// </summary>
        /// <param name="conditionId">["<c>market</c>"] The condition/market id</param>
        /// <param name="tokenId">["<c>asset_id</c>"] Asset/token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? conditionId = null, string? tokenId = null, CancellationToken ct = default);
        /// <summary>
        /// Cancel all open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /cancel-all
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelAllOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trades matching the filters
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/trades/trades" /><br />
        /// Endpoint:<br />
        /// GET /data/trades
        /// </para>
        /// </summary>
        /// <param name="tradeId">["<c>id</c>"] Filter by trade id</param>
        /// <param name="takerAddress">["<c>taker</c>"] Filter by taker address</param>
        /// <param name="makerAddress">["<c>maker</c>"] Filter by maker address</param>
        /// <param name="conditionId">["<c>market</c>"] Filter by condition id</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="cursor">["<c>next_cursor</c>"] Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketTrade>>> GetUserTradesAsync(
            string? tradeId = null,
            string? takerAddress = null,
            string? makerAddress = null,
            string? conditionId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null,
            CancellationToken ct = default);

        /// <summary>
        /// Send order heartbeat. Should be send every 10 seconds or all open orders will be canceled
        /// </summary>
        /// <param name="heartbeatId">["<c>heartbeat_id</c>"] The id from the previous PostOrderHeartbeatAsync response, or null for initial request</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderHeartbeat>> PostOrderHeartbeatAsync(
            string? heartbeatId,
            CancellationToken ct = default);
    }
}
