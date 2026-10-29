using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Polymarket.Net.Clients;
using Polymarket.Net.Clients.PerpsApi;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
        internal static readonly ConcurrentDictionary<string, BuilderFeeStatus> _builderFeeStatus = new ConcurrentDictionary<string, BuilderFeeStatus>();

        internal static Parameters GetOperationParameters(string type, Parameters parameters)
        {
            var result = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            var operation = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            operation.Add("type", type);
            operation.Add("args", parameters.BodyValue ?? parameters);
            result.Add("op", operation);
            return result;
        }
        internal static Parameters GetOperationParameters(string type, Parameters[] parameters)
        {
            var result = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            var operation = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            operation.Add("type", type);
            operation.Add("args", parameters);
            result.Add("op", operation);
            return result;
        }

        /// <summary>
        /// Get token info either from cache or from the API if the cache is outdated or not present
        /// </summary>
        public static async Task<CallResult<PolymarketOrderBook>> GetTokenInfoAsync(string tokenId, IPolymarketRestClientPredictionApi client)
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
        public static async Task<CallResult<PolymarketOrderBook[]>> GetTokenInfosAsync(IEnumerable<string> tokenIds, IPolymarketRestClientPredictionApi client)
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

        private static async Task<CallResult<PolymarketOrderBook[]>> UpdateTokenInfoAsync(string envName, IEnumerable<string> tokenIds, IPolymarketRestClientPredictionApi client)
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

        internal static async Task<ICallResult> CheckPerpBuilderAsync(PolymarketRestClientPerpsApi client, CancellationToken cancellationToken)
        {
            if (client.ApiCredentials?.Perps == null)
                // No credentials provided, no need to check builder fee
                return CallResult.Ok();

            var envName = client.ClientOptions.Environment.Name;
            if (envName.Equals("UnitTest", StringComparison.Ordinal))
                return CallResult.Ok();

            var key = client.ApiCredentials.Perps.ProxyKey.PublicAddress;
            var builderStatus = _builderFeeStatus.GetOrAdd(key, (key) => new BuilderFeeStatus());
            if (builderStatus.Checked)
                // Already checked, no need to check again
                return CallResult.Ok();

            var options = (PolymarketRestOptions)client.ClientOptions;

            if (options.PerpsOptions.BuilderFeePercentage == null
                || options.PerpsOptions.BuilderFeePercentage == 0
                || string.IsNullOrEmpty(options.PerpsOptions.BuilderCode))
            {
                // Builder disabled
                return CallResult.Ok();
            }

            await builderStatus.Semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                builderStatus.Checked = true;

                var approvals = await client.Account.GetBuilderApprovalsAsync(options.PerpsOptions.BuilderCode!, cancellationToken).ConfigureAwait(false);
                if (!approvals.Success)
                    // Failed to get approvals
                    return approvals;

                //0.0001
                var target = options.PerpsOptions.BuilderFeePercentage.Value / 100;
                var approval = approvals.Data.SingleOrDefault();
                if (approval != null && approval.MaxFeeRate >= target)
                {
                    // Builder already approved
                    builderStatus.Success = true;
                    return CallResult.Ok();
                }

                if (client.ApiCredentials?.Perps?.SignerKey?.PrivateKey == null)
                    // No private key available, can't approve
                    return CallResult.Ok();

                var nextApproval = (approval?.ApprovalVersion + 1) ?? 1;
                var approveResult = await client.Account.ApproveBuilderAsync(options.PerpsOptions.BuilderCode!, target, nextApproval, cancellationToken).ConfigureAwait(false);
                if (!approveResult.Success)
                    // Failed to approve
                    return approveResult;

                // Approved
                builderStatus.Success = true;
                return CallResult.Ok();
            }
            finally
            {
                builderStatus.Semaphore.Release();
            }
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

        internal class BuilderFeeStatus
        {
            /// <summary>
            /// Whether builder fee was checked
            /// </summary>
            public bool Checked { get; set; }
            /// <summary>
            /// Whether builder fee is approved and can be applied
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// Key-specific semaphore
            /// </summary>
            public SemaphoreSlim Semaphore { get; set; } = new SemaphoreSlim(1, 1);
        }
    }
}
