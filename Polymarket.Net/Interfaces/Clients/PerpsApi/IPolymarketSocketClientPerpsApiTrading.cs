using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PredictionApi
{
    /// <summary>
    /// Polymarket Perps WebSocket API
    /// </summary>
    public interface IPolymarketSocketClientPerpsApiTrading
    {

        /// <summary>
        /// Subscribe to user order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<PolymarketPerpOrder>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-fills" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
            Action<DataEvent<PolymarketPerpUserTradeUpdate[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to TP/SL updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-tpsl" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTpSlUpdatesAsync(
            Action<DataEvent<PolymarketPerpsTpSlUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-place-orders" /><br />
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
        /// <param name="triggerOrderType">["<c>tr.market</c>"] Trigger order type</param>
        /// <param name="triggerPrice">["<c>tr.trp</c>"] Trigger price</param>
        /// <param name="tpslType">["<c>tr.tpsl</c>"] Tp/Sl type</param>
        /// <param name="grouping">["<c>grp</c>"] Order grouping</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpOrderResult>> PlaceOrderAsync(
            long instrumentId,
            OrderSide side,
            decimal quantity,
            decimal? price = null,
            PerpTimeInForce? timeInForce = null,
            bool? postOnly = null,
            bool? reduceOnly = null,
            string? clientOrderId = null,
            OrderType? triggerOrderType = null,
            decimal? triggerPrice = null,
            TpSlType? tpslType = null,
            TpSlGrouping? grouping = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-place-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="grouping">["<c>grp</c>"] Order grouping</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<CallResult<PolymarketPerpOrderResult>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<PolymarketPerpsOrderRequest> orders,
            TpSlGrouping? grouping = null,
            CancellationToken ct = default);

        /// <summary>
        /// Edit an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-modify-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>oid</c>"] The order id</param>
        /// <param name="newPrice">["<c>p</c>"] New price</param>
        /// <param name="newQuantity">["<c>qty</c>"] New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpEditOrderResult>> EditOrderAsync(
            long orderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-modify-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to edit</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpEditOrderResult[]>> EditOrdersAsync(
            IEnumerable<PolymarketPerpsEditRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open order by client order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-modify-orders-coid" /><br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">The client order id</param>
        /// <param name="newPrice">New price</param>
        /// <param name="newQuantity">New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpEditOrderResult>> EditOrderByClientOrderIdAsync(
            string clientOrderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default);

        /// <summary>
        /// Edit open orders by client order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-modify-orders-coid" /><br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to edit</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpEditOrderResult[]>> EditOrdersByClientOrderIdAsync(
            IEnumerable<PolymarketPerpsEditCidRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-cancel-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpOrderResult>> CancelOrderAsync(
            long orderId,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-cancel-orders" /><br />
        /// </para>
        /// </summary>
        /// <param name="orderIds">Order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpOrderResult[]>> CancelOrdersAsync(
            IEnumerable<long> orderIds,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel order by client order ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-cancel-orders-coid" /><br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpOrderResult>> CancelOrderByClientOrderIdAsync(
            string clientOrderId,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders by client order ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-cancel-orders-coid" /><br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderIds">Client order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<PolymarketPerpOrderResult[]>> CancelOrdersByClientOrderIdAsync(
            IEnumerable<string> clientOrderIds,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-cancel-all" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult> CancelAllOrdersAsync(
            long symbolId,
            CancellationToken ct = default);
    }
}
