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
    public interface IPolymarketSocketClientPerpsApi : ISocketApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to trades updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-trades" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpTrade[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trades updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-trades" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpTrade[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to tickers updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-tickers" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to tickers updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-tickers" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to all ticker updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-tickers" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to book tickers updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-bbo" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpBookTickerUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to book tickers updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-bbo" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpBookTickerUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-book" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpBookUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-book" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpBookUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-klines" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            long symbolId,
            KlineInterval interval,
            Action<DataEvent<PolymarketKline[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-klines" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            IEnumerable<long> symbolIds,
            KlineInterval interval,
            Action<DataEvent<PolymarketKline[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to stat updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-statistics" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">Symbol id to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToStatisticsUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to stat updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-statistics" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolIds">Symbol ids to subscribe</param>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToStatisticsUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to all stat updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-statistics" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToAllStatisticsUpdatesAsync(
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default);
    }
}
