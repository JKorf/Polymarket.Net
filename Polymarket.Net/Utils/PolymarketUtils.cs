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
        private static Dictionary<string, Dictionary<string, PolymarketOrderBook>> _tokenInfos = new Dictionary<string, Dictionary<string, PolymarketOrderBook>>();

        private static readonly SemaphoreSlim _semaphoreSpot = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Update the internal spot symbol info
        /// </summary>
        public static async Task<CallResult<PolymarketOrderBook>> GetTokenInfoAsync(string tokenId, IPolymarketRestClientClobApi client)
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                var envName = client.ClientOptions.Environment.Name;
                if (envName.Equals("UnitTest", StringComparison.Ordinal))
                    return new CallResult<PolymarketOrderBook>(new PolymarketOrderBook { });

                if (_tokenInfos.TryGetValue(envName, out var envTokens) && envTokens.TryGetValue(tokenId, out var cachedTokenInfo))
                    return new CallResult<PolymarketOrderBook>(cachedTokenInfo); // Already have this token data

                var tokenInfo = await client.ExchangeData.GetOrderBookAsync(tokenId).ConfigureAwait(false);
                if (!tokenInfo)
                    return tokenInfo;

                if (!_tokenInfos.ContainsKey(envName))
                    _tokenInfos[envName] = new Dictionary<string, PolymarketOrderBook>();

                _tokenInfos[envName][tokenId] = tokenInfo.Data;
                return tokenInfo;
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }
    }
}
