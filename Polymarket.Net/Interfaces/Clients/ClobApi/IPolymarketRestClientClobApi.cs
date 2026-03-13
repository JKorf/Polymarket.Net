using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using System;

namespace Polymarket.Net.Interfaces.Clients.ClobApi
{
    /// <summary>
    /// Polymarket Clob API endpoints
    /// </summary>
    public interface IPolymarketRestClientClobApi : IRestApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Client options
        /// </summary>
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IPolymarketRestClientClobApiAccount" />
        public IPolymarketRestClientClobApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPolymarketRestClientClobApiExchangeData" />
        public IPolymarketRestClientClobApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IPolymarketRestClientClobApiTrading" />
        public IPolymarketRestClientClobApiTrading Trading { get; }
    }
}
