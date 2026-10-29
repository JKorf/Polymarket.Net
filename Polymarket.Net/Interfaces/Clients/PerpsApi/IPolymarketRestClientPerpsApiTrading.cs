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
    /// Polymarket Predicting API trading endpoints, placing and managing orders.
    /// </summary>
    public interface IPolymarketRestClientPerpsApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/create-orders" /><br />
        /// Endpoint:<br />
        /// POST /v1/trade/orders<br />
        /// </para>
        /// </summary>
        /// <param name="instrumentId">["<c>iid</c>"] Instrument id</param>
        /// <param name="side">["<c>buy</c>"] Order side</param>
        /// <param name="quantity">["<c>qty</c>"] Order quantity</param>
        /// <param name="price">["<c>p</c>"] Order price</param>
        /// <param name="timeInForce">["<c>tif</c>"] Time in force</param>
        /// <param name="postOnly">["<c>po</c>"] Post only</param>
        /// <param name="reduceOnly">["<c>ro</c>"] Reduce only</param>
        /// <param name="clientOrderId">["<c>c</c>"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderResult>> PlaceOrderAsync(
            long instrumentId,
            OrderSide side,
            decimal quantity,
            decimal? price = null,
            PerpTimeInForce? timeInForce = null,
            bool? postOnly = null,
            bool? reduceOnly = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/create-orders" /><br />
        /// Endpoint:<br />
        /// POST /v1/trade/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<CallResult<PolymarketPerpOrderResult>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<PolymarketPerpsOrderRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Edit an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/modify-orders" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>oid</c>"] The order id</param>
        /// <param name="newPrice">["<c>p</c>"] New price</param>
        /// <param name="newQuantity">["<c>qty</c>"] New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpEditOrderResult>> EditOrderAsync(
            long orderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/modify-orders" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to edit</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpEditOrderResult[]>> EditOrdersAsync(
            IEnumerable<PolymarketPerpsEditRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open order by client order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/modify-orders-coid" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/orders-coid<br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">The client order id</param>
        /// <param name="newPrice">New price</param>
        /// <param name="newQuantity">New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpEditOrderResult>> EditOrderByClientOrderIdAsync(
            string clientOrderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open orders by client order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/modify-orders-coid" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/orders-coid<br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to edit</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpEditOrderResult[]>> EditOrdersByClientOrderIdAsync(
            IEnumerable<PolymarketPerpsEditCidRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/account/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderResult>> CancelOrderAsync(
            long orderId,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/cancel-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/account/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orderIds">Order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderResult[]>> CancelOrdersAsync(
            IEnumerable<long> orderIds,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel order by client order ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/cancel-orders-coid" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/trade/orders-coid<br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderResult>> CancelOrderByClientOrderIdAsync(
            string clientOrderId,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders by client order ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/cancel-orders-coid" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/trade/orders-coid<br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderIds">Client order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrderResult[]>> CancelOrdersByClientOrderIdAsync(
            IEnumerable<string> clientOrderIds,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/cancel-all-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/trade/orders/all<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> CancelAllOrdersAsync(
            long symbolId,
            CancellationToken ct = default);

        /// <summary>
        /// Get order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-orders" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/orders<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>order_id</c>"] Filter by order id</param>
        /// <param name="clientOrderId">["<c>client_order_id</c>"] Filter by client order id</param>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrder[]>> GetOrdersAsync(
            long? orderId = null,
            string? clientOrderId = null,
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-open-orders" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/open-orders<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpOrder[]>> GetOpenOrdersAsync(long? symbolId = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-fills" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/fills<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="sort">["<c>sort</c>"] Sort direction</param>
        /// <param name="cursor">["<c>cursor</c>"] Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpUserTrade>>> GetUserTradesAsync(
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            SortDirection? sort = null,
            string? cursor = null,
            CancellationToken ct = default);

    }
}
