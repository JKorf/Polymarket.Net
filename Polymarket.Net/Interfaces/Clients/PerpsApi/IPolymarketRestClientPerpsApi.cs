using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using System;

namespace Polymarket.Net.Interfaces.Clients.PerpsApi
{
    /// <summary>
    /// Polymarket Perps API endpoints
    /// </summary>
    public interface IPolymarketRestClientPerpsApi : IRestApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Client options
        /// </summary>
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IPolymarketRestClientPerpsApiAccount" />
        public IPolymarketRestClientPerpsApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPolymarketRestClientPerpsApiExchangeData" />
        public IPolymarketRestClientPerpsApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IPolymarketRestClientPerpsApiTrading" />
        public IPolymarketRestClientPerpsApiTrading Trading { get; }
    }
}
