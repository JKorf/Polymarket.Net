using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using System;

namespace Polymarket.Net.Interfaces.Clients.PredictionApi
{
    /// <summary>
    /// Polymarket Prediction API endpoints
    /// </summary>
    public interface IPolymarketRestClientPredictionApi : IRestApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Client options
        /// </summary>
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IPolymarketRestClientPredictionApiAccount" />
        public IPolymarketRestClientPredictionApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPolymarketRestClientPredictionApiExchangeData" />
        public IPolymarketRestClientPredictionApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IPolymarketRestClientPredictionApiTrading" />
        public IPolymarketRestClientPredictionApiTrading Trading { get; }
    }
}
