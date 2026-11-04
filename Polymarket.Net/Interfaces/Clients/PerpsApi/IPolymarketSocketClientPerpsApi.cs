using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
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
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        IPolymarketSocketClientPerpsSharedApi SharedApi { get; }

        /// <summary>
        /// Account requests and streams
        /// </summary>
        IPolymarketSocketClientPerpsApiAccount Account { get; }
        /// <summary>
        /// Exchange data streams
        /// </summary>
        IPolymarketSocketClientPerpsApiExchangeData ExchangeData { get; }
        /// <summary>
        /// Trading requests and streams
        /// </summary>
        IPolymarketSocketClientPerpsApiTrading Trading { get; }
    }
}
