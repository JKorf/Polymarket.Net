using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using Polymarket.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;
using System.Collections.Generic;

namespace Polymarket.Net.Interfaces.Clients.ClobApi
{
    /// <summary>
    /// Polymarket Clob streams
    /// </summary>
    public interface IPolymarketSocketClientClobApi : ISocketApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to new market and market resolved updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/websocket/market-channel#new_market-message" /><br />
        /// Endpoint:<br />
        /// WS /ws/market
        /// </para>
        /// </summary>
        /// <param name="onNewMarketUpdate">New market update handler</param>
        /// <param name="onMarketResolvedUpdate">Market resolved update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPlatformUpdatesAsync(
            Action<DataEvent<PolymarketNewMarketUpdate>>? onNewMarketUpdate = null,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? onMarketResolvedUpdate = null,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to updates for specific tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/websocket/market-channel#book-message" /><br />
        /// Endpoint:<br />
        /// WS /ws/market
        /// </para>
        /// </summary>
        /// <param name="tokenIds">Ids to subscribe</param>
        /// <param name="onPriceChangeUpdate">Price change update handler</param>
        /// <param name="onBookUpdate">Book update handler</param>
        /// <param name="onLastTradePriceUpdate">Last trade price update handler</param>
        /// <param name="onTickSizeUpdate">Tick size update handler</param>
        /// <param name="onBestBidAskUpdate">Best bid/ask update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTokenUpdatesAsync(
            IEnumerable<string> tokenIds, 
            Action<DataEvent<PolymarketPriceChangeUpdate>>? onPriceChangeUpdate = null,
            Action<DataEvent<PolymarketBookUpdate>>? onBookUpdate = null,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? onLastTradePriceUpdate = null,
            Action<DataEvent<PolymarketTickSizeUpdate>>? onTickSizeUpdate = null,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? onBestBidAskUpdate = null,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order and trade updates for the user
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/websocket/user-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/user
        /// </para>
        /// </summary>
        /// <param name="onOrderUpdate">User order update handler</param>
        /// <param name="onTradeUpdate">User trade update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserUpdatesAsync(
            Action<DataEvent<PolymarketOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<PolymarketTradeUpdate>>? onTradeUpdate = null,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to sport matches updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/sports-websocket/overview" /><br />
        /// Endpoint:<br />
        /// WS /ws
        /// </para>
        /// </summary>
        /// <param name="onSportsUpdate">Sport match update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSportsUpdatesAsync(
            Action<DataEvent<PolymarketSportsUpdate>>? onSportsUpdate = null,
            CancellationToken ct = default);
    }
}
