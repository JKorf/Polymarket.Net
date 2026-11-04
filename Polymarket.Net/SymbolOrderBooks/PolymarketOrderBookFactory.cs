using CryptoExchange.Net.Interfaces;
using System;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using Polymarket.Net.Interfaces.Clients;
using Microsoft.Extensions.Logging;

namespace Polymarket.Net.SymbolOrderBooks
{
    /// <summary>
    /// Polymarket order book factory
    /// </summary>
    public class PolymarketOrderBookFactory : IPolymarketOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public PolymarketOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreatePrediction(string tokenId, Action<PolymarketOrderBookOptions>? options = null)
            => new PolymarketPredictionSymbolOrderBook(tokenId, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IPolymarketSocketClient>());

        /// <inheritdoc />
        public ISymbolOrderBook CreatePerps(long symbolId, Action<PolymarketOrderBookOptions>? options = null)
            => new PolymarketPerpsSymbolOrderBook(symbolId, options,
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IPolymarketSocketClient>());


    }
}
