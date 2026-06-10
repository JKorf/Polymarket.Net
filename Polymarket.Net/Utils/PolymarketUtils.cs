using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    return CallResult<PolymarketOrderBook>.Ok(new PolymarketOrderBook { TickQuantity = 0.1m });

                if (_tokenInfos.TryGetValue(envName, out var envTokens) && envTokens.TryGetValue(tokenId, out var cachedTokenInfo)
                    && DateTime.UtcNow - cachedTokenInfo.LastUpdateTime < TimeSpan.FromSeconds(2))
                {
                    // Have this token data and it's up to date
                    return CallResult<PolymarketOrderBook>.Ok(cachedTokenInfo.Book);
                }

                var result = await UpdateTokenInfoAsync(envName, [tokenId], client).ConfigureAwait(false);
                if (!result.Success)
                    return CallResult.Fail<PolymarketOrderBook>(result.Error);

                var token = result.Data.FirstOrDefault(x => x.TokenId == tokenId);
                if (token == null)
                {
                    return CallResult.Fail<PolymarketOrderBook>(
                        new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, $"Token {tokenId} not found")));
                }

                return CallResult.Ok(token);
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        /// <summary>
        /// Get token info either from cache or from the API if the cache is outdated or not present
        /// </summary>
        public static async Task<CallResult<PolymarketOrderBook[]>> GetTokenInfosAsync(IEnumerable<string> tokenIds, IPolymarketRestClientClobApi client)
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                var envName = client.ClientOptions.Environment.Name;
                if (envName.Equals("UnitTest", StringComparison.Ordinal))
                    return CallResult.Ok(tokenIds.Select(x => new PolymarketOrderBook { TickQuantity = 0.1m, TokenId = x }).ToArray());

                // Get tokens that are not in cache or outdated
                var toRequest = new List<string>();
                foreach (var tokenId in tokenIds)
                {
                    if (!_tokenInfos.TryGetValue(envName, out var envTokens) || !envTokens.TryGetValue(tokenId, out var cachedTokenInfo)
                        || DateTime.UtcNow - cachedTokenInfo.LastUpdateTime >= TimeSpan.FromSeconds(2))
                    {
                        toRequest.Add(tokenId);
                    }
                }

                // Update the tokens that are not in cache or outdated
                var result = await UpdateTokenInfoAsync(envName, toRequest, client).ConfigureAwait(false);
                if (!result.Success)
                    return result;

                // All tokens should now be in cache, return them
                return CallResult.Ok(tokenIds.Select(x => _tokenInfos[envName][x].Book).ToArray());
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        private static async Task<CallResult<PolymarketOrderBook[]>> UpdateTokenInfoAsync(string envName, IEnumerable<string> tokenIds, IPolymarketRestClientClobApi client)
        {
            var tokenInfo = await client.ExchangeData.GetOrderBooksAsync(tokenIds).ConfigureAwait(false);
            if (!tokenInfo.Success)
                return CallResult.Fail<PolymarketOrderBook[]>(tokenInfo.Error);

            if (!_tokenInfos.ContainsKey(envName))
                _tokenInfos[envName] = new Dictionary<string, PolymarketTokenCache>();

            foreach(var result in tokenInfo.Data)
                _tokenInfos[envName][result.TokenId] = new PolymarketTokenCache(DateTime.UtcNow, result);
            return CallResult.Ok(tokenInfo.Data);
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
