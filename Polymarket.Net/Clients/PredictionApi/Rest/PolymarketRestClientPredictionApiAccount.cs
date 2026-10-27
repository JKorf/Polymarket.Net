using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PredictionApi.Rest
{
    internal class PolymarketRestClientPredictionApiAccount : IPolymarketRestClientPredictionApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPredictionApi _client;

        public PolymarketRestClientPredictionApiAccount(PolymarketRestClientPredictionApi client)
        {
            _client = client;
        }


        public async Task<HttpResult<PolymarketCreds>> CreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCreds>> GetApiCredentialsAsync(long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("nonce", nonce);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "auth/derive-api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketCreds>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "auth/api-keys", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketApiKeys>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketRateLimit>> GetRateLimitTierAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "v1/user/rate-limit-tier", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketRateLimit>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketClosedOnlyMode>> GetClosedOnlyModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "auth/ban-status/closed-only", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketClosedOnlyMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult> DeleteApiKeyAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _client.ClobApiAddress, "auth/api-key", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        // TODO: read-only API keys

        public async Task<HttpResult<PolymarketNotification[]>> GetNotificationsAsync(CancellationToken ct = default)
        {
            if (_client.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketNotification[]>(_client.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_client.AuthenticationProvider.ApiCredentials.L1.SignType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(900, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketNotification[]>> DropNotificationsAsync(IEnumerable<string> ids, CancellationToken ct = default)
        {
            if (_client.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketNotification[]>(_client.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_client.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("ids", string.Join(",", ids));
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _client.ClobApiAddress, "notifications", PolymarketPlatform.RateLimiter.ClobApi, 1, true, parameterPosition: HttpMethodParameterPosition.InUri,
                limitGuard: new SingleLimitGuard(125, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketNotification[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketBalanceAllowance>> GetBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_client.AuthenticationProvider == null)
                return HttpResult.Fail<PolymarketBalanceAllowance>(_client.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_client.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("asset_type", assetType);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "balance-allowance", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketBalanceAllowance>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult> UpdateBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default)
        {
            if (_client.AuthenticationProvider == null)
                return HttpResult.Fail(_client.Exchange, new NoApiCredentialsError());

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("signature_type", (int)_client.AuthenticationProvider.ApiCredentials.L1.SignType);
            parameters.Add("asset_type", assetType);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "balance-allowance/update", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(50, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "builder/trades", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _client.SendAsync<PolymarketPage<PolymarketBuilderTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }
    }
}
