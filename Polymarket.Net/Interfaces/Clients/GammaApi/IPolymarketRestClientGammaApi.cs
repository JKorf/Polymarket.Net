using CryptoExchange.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using System;

namespace Polymarket.Net.Interfaces.Clients.GammaApi
{
    /// <summary>
    /// Polymarket Gamma API endpoints
    /// </summary>
    public interface IPolymarketRestClientGammaApi : IRestApiClient, IDisposable
    {
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IPolymarketRestClientGammaApiAccount" />
        public IPolymarketRestClientGammaApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPolymarketRestClientGammaApiExchangeData" />
        public IPolymarketRestClientGammaApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IPolymarketRestClientGammaApiTrading" />
        public IPolymarketRestClientGammaApiTrading Trading { get; }
    }
}
