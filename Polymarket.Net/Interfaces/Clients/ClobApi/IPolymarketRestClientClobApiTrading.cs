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
        /// <a href="https://docs.polymarket.com/api-reference/trade/get-user-orders" /><br />
        /// Endpoint:<br />
        /// GET /data/orders
        /// </para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="marketId">Filter by market/condition id</param>
        /// <param name="tokenId">Asset/token id</param>
        /// <param name="cursor">Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(string? orderId = null, string? marketId = null, string? tokenId = null, string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get an order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/get-single-order-by-id" /><br />
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
        /// <a href="https://docs.polymarket.com/api-reference/trade/get-order-scoring-status" /><br />
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
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/post-a-new-order" /><br />
        /// Endpoint:<br />
        /// POST /order
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>order.tokenId</c>"] Token id</param>
        /// <param name="side">["<c>order.side</c>"] Side</param>
        /// <param name="orderType">Type of order</param>
        /// <param name="timeInForce">["<c>orderType</c>"] Time in force</param>
        /// <param name="quantity">Quantity of shares</param>
        /// <param name="price">Price, value between 0 and 1. For example 0.001 means 0.1c in the UI, 0.5 means 50c in UI</param>
        /// <param name="postOnly">["<c>postOnly</c>"] Post only order</param>
        /// <param name="clientOrderId">["<c>order.salt</c>"] Client order id</param>
        /// <param name="expiration">["<c>order.expiration</c>"] Expiration time</param>
        /// <param name="quantityType">Type of quantity for an order, either in shares (default) or in value (USD). Value is only available for market buy orders</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderResult>> PlaceOrderAsync(
            string tokenId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            bool? postOnly = null,
            long? clientOrderId = null,
            DateTime? expiration = null,
            QuantityType? quantityType = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple orders in a single request
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/post-multiple-orders" /><br />
        /// Endpoint:<br />
        /// POST /orders
        /// </para>
        /// </summary>
        /// <param name="requests">Order requests</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<PolymarketOrderRequest> requests, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/cancel-single-order" /><br />
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
        /// <a href="https://docs.polymarket.com/api-reference/trade/cancel-multiple-orders" /><br />
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
        /// <a href="https://docs.polymarket.com/api-reference/trade/cancel-orders-for-a-market" /><br />
        /// Endpoint:<br />
        /// DELETE /orders
        /// </para>
        /// </summary>
        /// <param name="marketId">["<c>market</c>"] The condition/market id</param>
        /// <param name="tokenId">["<c>asset_id</c>"] Asset/token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? marketId = null, string? tokenId = null, CancellationToken ct = default);
        /// <summary>
        /// Cancel all open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/cancel-all-orders" /><br />
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
        /// <a href="https://docs.polymarket.com/api-reference/trade/get-trades" /><br />
        /// Endpoint:<br />
        /// GET /data/trades
        /// </para>
        /// </summary>
        /// <param name="tradeId">["<c>id</c>"] Filter by trade id</param>
        /// <param name="makerAddress">["<c>maker</c>"] Filter by maker address</param>
        /// <param name="marketId">["<c>market</c>"] Filter by market id</param>
        /// <param name="tokenId">["<c>asset_id</c>"] Filter by token id</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="cursor">["<c>next_cursor</c>"] Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketTrade>>> GetUserTradesAsync(
            string? tradeId = null,
            string? makerAddress = null,
            string? marketId = null,
            string? tokenId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null,
            CancellationToken ct = default);

        /// <summary>
        /// Send order heartbeat. Should be send every 10 seconds or all open orders will be canceled
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/trade/send-heartbeat#send-heartbeat" /><br />
        /// Endpoint:<br />
        /// GET /heartbeats
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> PostOrderHeartbeatAsync(CancellationToken ct = default);
    }
}
