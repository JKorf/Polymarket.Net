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
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/get-active-order" /></para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="conditionId">Filter by market/condition id</param>
        /// <param name="tokenId">Asset/token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(string? orderId = null, string? conditionId = null, string? tokenId = null, CancellationToken ct = default);

        /// <summary>
        /// Get an order by id
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/get-order" /></para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrder>> GetOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Check if an order is eligible or scoring for Rewards purposes
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/check-scoring" /></para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderScoring>> GetOrderRewardScoringAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Check if orders are eligible or scoring for Rewards purposes
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/check-scoring" /></para>
        /// </summary>
        /// <param name="orderIds">Order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, bool>>> GetOrdersRewardScoringAsync(IEnumerable<string> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/create-order" /></para>
        /// </summary>
        /// <param name="tokenId">Token id</param>
        /// <param name="side">Side</param>
        /// <param name="orderType">Type of order</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="quantity">Quantity of shares</param>
        /// <param name="price">Price, value between 0 and 1. For example 0.001 means 0.1c in the UI, 0.5 means 50c in UI</param>
        /// <param name="nonce"></param>
        /// <param name="feeRateBps">Fee rate basis points as required by the operator</param>
        /// <param name="takerAddress">Taker/operator address</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="expiration">Expiration time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketOrderResult>> PlaceOrderAsync(
            string tokenId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            long? feeRateBps = null,
            string? takerAddress = null,
            long? clientOrderId = null,
            DateTime? expiration = null,
            long? nonce = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /></para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default);
        /// <summary>
        /// Cancel multiple orders
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /></para>
        /// </summary>
        /// <param name="orderIds">Ids of orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default);
        /// <summary>
        /// Cancel all orders for a specific market
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /></para>
        /// </summary>
        /// <param name="conditionId">The condition/market id</param>
        /// <param name="tokenId">Asset/token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? conditionId = null, string? tokenId = null, CancellationToken ct = default);
        /// <summary>
        /// Cancel all open orders
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/orders/cancel-orders" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketCancelResult>> CancelAllOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trades matching the filters
        /// <para><a href="https://docs.polymarket.com/developers/CLOB/trades/trades" /></para>
        /// </summary>
        /// <param name="tradeId">Filter by trade id</param>
        /// <param name="takerAddress">Filter by taker address</param>
        /// <param name="makerAddress">Filter by maker address</param>
        /// <param name="conditionId">Filter by condition id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="cursor">Next page cursor</param>
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
    }
}
