using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Polymarket Rest API. 
    /// </summary>
    public interface IPolymarketRestClient : IRestClient<PolymarketCredentials>
    {
        /// <summary>
        /// Prediction API endpoints
        /// </summary>
        /// <see cref="IPolymarketRestClientPredictionApi"/>
        public IPolymarketRestClientPredictionApi PredictionApi { get; }
        /// <summary>
        /// Perps API endpoints
        /// </summary>
        /// <see cref="IPolymarketRestClientPerpsApi"/>
        public IPolymarketRestClientPerpsApi PerpsApi { get; }

        /// <summary>
        /// Update existing credentials which specify L1 credentials (PolymarketAddress, L1PrivateKey) with L2 credentials
        /// </summary>
        /// <param name="credentials">Credentials</param>
        void UpdateL2Credentials(PolymarketCreds credentials);
    }
}
