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
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        IPolymarketRestClientPerpsSharedApi SharedApi { get; }
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

        /// <summary>
        /// Get a symbol ID by its name. Only returns data when ExchangeData.GetSymbolsAsync has been called
        /// </summary>
        /// <param name="name">Symbol name, for example ETH-USD</param>
        long? GetSymbolIdByName(string name);

        /// <summary>
        /// Get a symbol name by its ID. Only returns data when ExchangeData.GetSymbolsAsync has been called
        /// </summary>
        /// <param name="id">The symbol id</param>
        string? GetSymbolNameById(long id);
    }
}
