using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi.Rest
{
    internal class PolymarketRestClientPerpsApiAccount : IPolymarketRestClientPerpsApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPerpsApi _baseClient;

        public PolymarketRestClientPerpsApiAccount(PolymarketRestClientPerpsApi client)
        {
            _baseClient = client;
        }

        #region Approve Builder

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketBuilderResult>> ApproveBuilderAsync(
            string builder,
            decimal maxFeeRate,
            int approvalVersion = 1,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("builder", builder);
            parameters.Add("max_fee_rate", maxFeeRate);
            parameters.Add("approval_version", approvalVersion);
            var opParameters = PolymarketUtils.GetOperationParameters("approveBuilder", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.SignerKey);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/builder-approvals", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketBuilderResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Builder Approvals

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketBuilderResult[]>> GetBuilderApprovalsAsync(string builder, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("builder", builder);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/builder-approvals", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketBuilderResultWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PolymarketBuilderResult[]>(result) : HttpResult.Ok(result, result.Data.Data);
        }

        #endregion

        #region Create Proxy

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(string proxyAddress, DateTime expireTime, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("owner", _baseClient.ApiCredentials?.Perps.SignerKey.PublicAddress);
            parameters.Add("proxy", proxyAddress);
            parameters.Add("expiry", expireTime);
            var opParameters = PolymarketUtils.GetOperationParameters("createProxy", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateCreateProxyRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.SignerKey, proxyAddress);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/proxy", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketProxyResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Delete Proxy

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketProxyResult>> DeleteProxyAsync(string proxyAddress, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("proxy", proxyAddress);
            var opParameters = PolymarketUtils.GetOperationParameters("deleteProxy", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.SignerKey);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/account/proxy", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketProxyResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/balances", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketPerpBalance[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Portfolio

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpPortfolio>> GetPortfolioAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/portfolio", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketPerpPortfolio>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Equity History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpValue>>> GetEquityHistoryAsync(
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("interval", interval);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/equity", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpValue>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Pnl History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpValue>>> GetPnlHistoryAsync(
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("interval", interval);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/pnl", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpValue>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Funding History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketFundingHistory>>> GetFundingHistoryAsync(
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/funding", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketFundingHistory>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
