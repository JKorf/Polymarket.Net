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

        public async Task<HttpResult<PolymarketCreds>> CreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCreds>> GetApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "auth/derive-api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCreds>> GetOrCreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var getResult = await GetApiCredentialsAsync(nonce, ct).ConfigureAwait(false);
            if (getResult.Success)
                return getResult;

            return await CreateApiCredentialsAsync(nonce, ct).ConfigureAwait(false);
        }

        public async Task<HttpResult<PolymarketApiKeys>> GetApiKeysAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "auth/api-keys", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketApiKeys>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketClosedOnlyMode>> GetClosedOnlyModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "auth/ban-status/closed-only", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketClosedOnlyMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult> DeleteApiKeyAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        // TODO: read-only API keys

        public async Task<HttpResult<PolymarketNotification[]>> GetNotificationsAsync(CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketNotification[]>(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(900, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketNotification[]>> DropNotificationsAsync(IEnumerable<string> ids, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketNotification[]>(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("ids", string.Join(",", ids));
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true, parameterPosition: HttpMethodParameterPosition.InUri,
                limitGuard: new SingleLimitGuard(125, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketBalanceAllowance>> GetBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketBalanceAllowance>(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("asset_type", assetType);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "balance-allowance", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketBalanceAllowance>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult> UpdateBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_baseClient.AuthenticationProvider == null)
                return HttpResult.Fail(_baseClient.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_baseClient.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("asset_type", assetType);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "balance-allowance/update", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(50, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketPage<PolymarketBuilderTrade>>> GetBuilderTradesAsync(
            string builderCode,
            string? tradeId = null,
            string? marketId = null,
            string? tokenId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null, 
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("builder_code", builderCode);
            parameters.Add("id", tradeId);
            parameters.Add("market", marketId);
            parameters.Add("asset_id", tokenId);
            parameters.Add("after", startTime);
            parameters.Add("before", endTime);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "builder/trades", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketBuilderTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }
    }
}
