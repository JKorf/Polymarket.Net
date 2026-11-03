using Polymarket.Net.Objects.Models;
using System;
using System.Linq;

namespace Polymarket.Net.Utils
{
    /// <summary>
    /// Symbol mapping cache
    /// </summary>
    public class PolymarketSymbolCache
    {
        private PolymarketSymbol[] _symbols = [];

        /// <summary>
        /// Set the cache
        /// </summary>
        /// <param name="symbols"></param>
        public void Set(PolymarketSymbol[] symbols)
        {
            _symbols = symbols;
        }

        /// <summary>
        /// Get a symbol name by symbol id
        /// </summary>
        public string? Get(long symbolId)
        {
            var symbol = _symbols.SingleOrDefault(x => x.SymbolId == symbolId);
            return symbol?.Symbol;
        }

        /// <summary>
        /// Get a symbol id by symbol name
        /// </summary>
        public long? Get(string symbol)
        {
            var symbolInfo = _symbols.SingleOrDefault(x => x.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
            return symbolInfo?.SymbolId;
        }
    }
}
