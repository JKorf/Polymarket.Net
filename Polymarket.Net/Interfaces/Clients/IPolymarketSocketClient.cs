using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Interfaces.Clients.GammaApi;

namespace Polymarket.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Polymarket websocket API
    /// </summary>
    public interface IPolymarketSocketClient : ISocketClient
    {
        /// <summary>
        /// Clob API endpoints
        /// </summary>
        /// <see cref="IPolymarketSocketClientClobApi"/>
        public IPolymarketSocketClientClobApi ClobApi { get; }
        /// <summary>
        /// Gamma API endpoints
        /// </summary>
        /// <see cref="IPolymarketSocketClientGammaApi"/>
        public IPolymarketSocketClientGammaApi GammaApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
