using CryptoExchange.Net.Objects;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Utils
{
    /// <summary>
    /// Util methods for the Polymarket API
    /// </summary>
    public static class PolymarketUtils
    {
        private static Dictionary<string, Dictionary<string, PolymarketTokenCache>> _tokenInfos = new Dictionary<string, Dictionary<string, PolymarketTokenCache>>();
        private static readonly SemaphoreSlim _semaphoreSpot = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Get token info either from cache or from the API if the cache is outdated or not present
        /// </summary>
        public static async Task<CallResult<PolymarketOrderBook>> GetTokenInfoAsync(string tokenId, IPolymarketRestClientClobApi client)
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                var envName = client.ClientOptions.Environment.Name;
                if (envName.Equals("UnitTest", StringComparison.Ordinal))
                    return new CallResult<PolymarketOrderBook>(new PolymarketOrderBook { TickQuantity = 0.1m });

                if (_tokenInfos.TryGetValue(envName, out var envTokens) && envTokens.TryGetValue(tokenId, out var cachedTokenInfo)
                    && DateTime.UtcNow - cachedTokenInfo.LastUpdateTime < TimeSpan.FromSeconds(2))
                {
                    // Have this token data and it's up to date
                    return new CallResult<PolymarketOrderBook>(cachedTokenInfo.Book);
                }

                return await UpdateTokenInfoAsync(envName, tokenId, client).ConfigureAwait(false);
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        private static async Task<CallResult<PolymarketOrderBook>> UpdateTokenInfoAsync(string envName, string tokenId, IPolymarketRestClientClobApi client)
        {
            var tokenInfo = await client.ExchangeData.GetOrderBookAsync(tokenId).ConfigureAwait(false);
            if (!tokenInfo)
                return tokenInfo;

            if (!_tokenInfos.ContainsKey(envName))
                _tokenInfos[envName] = new Dictionary<string, PolymarketTokenCache>();

            _tokenInfos[envName][tokenId] = new PolymarketTokenCache(DateTime.UtcNow, tokenInfo.Data);
            return tokenInfo;
        }

        private class PolymarketTokenCache
        {
            public DateTime LastUpdateTime { get; set; } = default; 
            public PolymarketOrderBook Book { get; set; }

            public PolymarketTokenCache(DateTime timestamp, PolymarketOrderBook book)
            {
                LastUpdateTime = timestamp;
                Book = book;
            }
        }
    }
}
