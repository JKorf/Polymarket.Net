using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Interfaces.Clients.PerpsApi;

namespace Polymarket.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of Polymarket
    /// </summary>
    public interface IPolymarketSharedApiClient : ISharedApiClientBase
    {
        /// <summary>
        /// Perps REST shared API implementations
        /// </summary>
        IPolymarketRestClientPerpsSharedApi PerpsRest { get; }

        /// <summary>
        /// Perps WebSocket shared API implementations
        /// </summary>
        IPolymarketSocketClientPerpsSharedApi PerpsSocket { get; }
    }
}
