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
        /// Create a new Clob local order book instance
        /// </summary>
        ISymbolOrderBook CreateClob(string tokenId, Action<PolymarketOrderBookOptions>? options = null);

    }
}