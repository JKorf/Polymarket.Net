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
    public interface IPolymarketSocketClientClobApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// 
        /// <para><a href="XXX" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTokenUpdatesAsync(
            IEnumerable<string> tokenIds, 
            Action<DataEvent<PolymarketPriceChangeUpdate>>? onPriceChangeUpdate = null,
            Action<DataEvent<PolymarketBookUpdate>>? onBookUpdate = null,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? onLastTradePriceUpdate = null,
            Action<DataEvent<PolymarketTickSizeUpdate>>? onTickSizeUpdate = null,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? onBestBidAskUpdate = null,
            Action<DataEvent<PolymarketNewMarketUpdate>>? onNewMarketUpdate = null,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? onMarketResolvedUpdate = null,
            CancellationToken ct = default);

    }
}
