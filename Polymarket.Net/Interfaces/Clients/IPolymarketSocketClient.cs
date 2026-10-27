using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Polymarket websocket API
    /// </summary>
    public interface IPolymarketSocketClient : ISocketClient<PolymarketCredentials>
    {
        /// <summary>
        /// Prediction API endpoints
        /// </summary>
        /// <see cref="IPolymarketSocketClientPredictionApi"/>
        public IPolymarketSocketClientPredictionApi PredictionApi { get; }

        /// <summary>
        /// Update existing credentials which specify L1 credentials (PolymarketAddress, L1PrivateKey) with L2 credentials
        /// </summary>
        /// <param name="credentials">Credentials</param>
        void UpdateL2Credentials(PolymarketCreds credentials);
    }
}
