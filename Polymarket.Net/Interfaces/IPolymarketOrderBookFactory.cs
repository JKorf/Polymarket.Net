using CryptoExchange.Net.Interfaces;
using System;
using Polymarket.Net.Objects.Options;

namespace Polymarket.Net.Interfaces
{
    /// <summary>
    /// Polymarket local order book factory
    /// </summary>
    public interface IPolymarketOrderBookFactory
    {
        /// <summary>
        /// Create a new Prediction local order book instance
        /// </summary>
        ISymbolOrderBook CreatePrediction(string tokenId, Action<PolymarketOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Perps local order book instance
        /// </summary>
        ISymbolOrderBook CreatePerps(long symbolId, Action<PolymarketOrderBookOptions>? options = null);

    }
}