using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientClobApiAccount : IPolymarketRestClientClobApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PolymarketRestClientClobApi _baseClient;

        internal PolymarketRestClientClobApiAccount(PolymarketRestClientClobApi baseClient)
        {
            _baseClient = baseClient;
        }

        public async Task<WebCallResult<PolymarketCreds>> CreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCreds>> GetApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "auth/derive-api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCreds>> GetOrCreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var getResult = await GetApiCredentialsAsync(nonce, ct).ConfigureAwait(false);
            if (getResult)
                return getResult;

            return await CreateApiCredentialsAsync(nonce, ct).ConfigureAwait(false);
        }

        public async Task<WebCallResult<PolymarketApiKeys>> GetApiKeysAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "auth/api-keys", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketApiKeys>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketClosedOnlyMode>> GetClosedOnlyModeAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "auth/ban-status/closed-only", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketClosedOnlyMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult> DeleteApiKeyAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        // TODO: read-only API keys

        public async Task<WebCallResult<PolymarketNotification[]>> GetNotificationsAsync(CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult<PolymarketNotification[]>(new NoApiCredentialsError());

            var parameters = new ParameterCollection();
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(900, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketNotification[]>> DropNotificationsAsync(IEnumerable<string> ids, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult<PolymarketNotification[]>(new NoApiCredentialsError());

            var parameters = new ParameterCollection();
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("ids", string.Join(",", ids));
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true, parameterPosition: HttpMethodParameterPosition.InUri,
                limitGuard: new SingleLimitGuard(125, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketBalanceAllowance>> GetBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult<PolymarketBalanceAllowance>(new NoApiCredentialsError());

            var parameters = new ParameterCollection();
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.AddEnum("asset_type", assetType);
            parameters.AddOptional("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "balance-allowance", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketBalanceAllowance>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult> UpdateBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return new WebCallResult(new NoApiCredentialsError());

            var parameters = new ParameterCollection();
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.AddEnum("asset_type", assetType);
            parameters.AddOptional("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "balance-allowance/update", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(50, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult> GetBuilderTradesAsync(
            string? tradeId = null,
            string? takerAddress = null,
            string? makerAddress = null,
            string? conditionId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null, 
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", cursor);
            parameters.AddOptional("taker", cursor);
            parameters.AddOptional("maker", cursor);
            parameters.AddOptional("market", cursor);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "builder/trades", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }
    }
}
