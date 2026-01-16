using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.OrderBook;
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
            
            
            Clob = new OrderBookFactory<PolymarketOrderBookOptions>(CreateClob, Create);

        }

        
         /// <inheritdoc />
        public IOrderBookFactory<PolymarketOrderBookOptions> Clob { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<PolymarketOrderBookOptions>? options = null)
        {
            //var symbolName = symbol.GetSymbol(PolymarketExchange.FormatSymbol);
            throw new Exception();
#warning TODO

            //return Create(symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook CreateClob(string symbol, Action<PolymarketOrderBookOptions>? options = null)
            => new PolymarketClobSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IPolymarketRestClient>(),
                                                          _serviceProvider.GetRequiredService<IPolymarketSocketClient>());


    }
}
