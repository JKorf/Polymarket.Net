using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.ClobApi
{
    /// <summary>
    /// Polymarket Data streams
    /// </summary>
    public interface IPolymarketSocketClientDataApi : ISocketApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to crypto price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/websockets/live-data-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onSnapshot">Crypto price snapshot update handler</param>
        /// <param name="onUpdate">Crypto price update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToCryptoPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketPriceUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to crypto one minute time weighted average price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/websockets/live-data-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onSnapshot">Crypto price snapshot update handler</param>
        /// <param name="onUpdate">Crypto price update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToCryptoTwapPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketPriceUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to equity price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/websockets/live-data-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onSnapshot">Equity price snapshot update handler</param>
        /// <param name="onUpdate">Equity price update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToEquityPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketEquityPriceUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to equity one minute time weighted average price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/websockets/live-data-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onSnapshot">Equity price snapshot update handler</param>
        /// <param name="onUpdate">Equity price update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToEquityTwapPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketEquityPriceUpdate>> onUpdate,
            CancellationToken ct = default);
    }
}
