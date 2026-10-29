using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.Web3;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Emit;
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

        #region Create Proxy

        /// <inheritdoc />
        public Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(DateTime expireTime, string? label = null, CancellationToken ct = default)
            => CreateProxyAsync(_baseClient.ApiCredentials.Perps.ProxyKey.PublicAddress, expireTime, label, ct);

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(string proxyAddress, DateTime expireTime, string? label = null, CancellationToken ct = default)
        {
            if (!_baseClient.ValidateSignerCredentialsAvailable(out var error))
                return HttpResult.Fail<PolymarketProxyResult>(_baseClient.Exchange, error!);

            var expireMs = DateTimeConverter.ConvertToMilliseconds(expireTime).Value;
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("owner", _baseClient.ApiCredentials!.Perps!.SignerKey!.PublicAddress);
            parameters.Add("proxy", proxyAddress);
            parameters.Add("expiry", expireMs);
            var opParameters = PolymarketUtils.GetOperationParameters("createProxy", parameters);
            opParameters.Add("label", label);
            _baseClient.AuthenticationProvider?.AuthenticateCreateProxyRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.SignerKey!, proxyAddress, expireMs);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/proxy", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketProxyResult>(request, opParameters, ct).ConfigureAwait(false);
            if (result.Success && proxyAddress == _baseClient.ApiCredentials.Perps.ProxyKey.PublicAddress)
                _baseClient.ApiCredentials.Perps.Secret = result.Data.Secret;

            return result;
        }

        #endregion

        #region Delete Proxy

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketProxyResult>> DeleteProxyAsync(string proxyAddress, CancellationToken ct = default)
        {
            if (!_baseClient.ValidateSignerCredentialsAvailable(out var error))
                return HttpResult.Fail<PolymarketProxyResult>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("proxy", proxyAddress);
            var opParameters = PolymarketUtils.GetOperationParameters("deleteProxy", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.SignerKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/account/proxy", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketProxyResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Credentials

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpsCredentials>> GetCredentialsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/credentials", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketPerpsCredentials>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Approve Builder

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketBuilderResult>> ApproveBuilderAsync(
            string builder,
            decimal maxFeeRate,
            int approvalVersion = 1,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateSignerCredentialsAvailable(out var error))
                return HttpResult.Fail<PolymarketBuilderResult>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("builder", builder);
            parameters.Add("max_fee_rate", maxFeeRate);
            parameters.Add("approval_version", approvalVersion);
            var opParameters = PolymarketUtils.GetOperationParameters("approveBuilder", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.SignerKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/builder-approvals", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketBuilderResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Builder Approvals

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketBuilderResult[]>> GetBuilderApprovalsAsync(string builder, CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketBuilderResult[]>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("builder", builder);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/builder-approvals", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketBuilderResultWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PolymarketBuilderResult[]>(result) : HttpResult.Ok(result, result.Data.Data);
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPerpBalance[]>(_baseClient.Exchange, error!);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/balances", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketPerpBalance[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Portfolio

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpPortfolio>> GetPortfolioAsync(CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPerpPortfolio>(_baseClient.Exchange, error!);

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
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpValue>>(_baseClient.Exchange, error!);

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
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpValue>>(_baseClient.Exchange, error!);

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
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketFundingHistory>>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/funding", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketFundingHistory>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketLeverageResult>> SetLeverageAsync(
            long symbolId,
            int leverage,
            bool cross,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, true, out var error))
                return HttpResult.Fail<PolymarketLeverageResult>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("iid", symbolId);
            parameters.Add("lev", leverage);
            parameters.Add("cross", cross);
            var opParameters = PolymarketUtils.GetOperationParameters("updateLeverage", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(new HttpMethod("PATCH"), _baseClient.BaseAddress, "/v1/trade/leverage", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketLeverageResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Update Isolated Margin

        /// <inheritdoc />
        public async Task<HttpResult> UpdateIsolatedMarginAsync(
            long symbolId,
            decimal quantity,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, true, out var error))
                return HttpResult.Fail(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("iid", symbolId);
            parameters.Add("amt", quantity);
            var opParameters = PolymarketUtils.GetOperationParameters("updateMargin", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(new HttpMethod("PATCH"), _baseClient.BaseAddress, "/v1/trade/margin", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketWithdrawResult>> WithdrawAsync(
            string token,
            long quantity,
            string toAddress,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateSignerCredentialsAvailable(out var error))
                return HttpResult.Fail<PolymarketWithdrawResult>(_baseClient.Exchange, error!);

            var account = _baseClient.ApiCredentials!.Perps!.SignerKey!.PublicAddress;
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("account", account);
            parameters.Add("token", token);
            parameters.Add("amount", quantity.ToString());
            parameters.Add("to", toAddress);

            var opParameters = PolymarketUtils.GetOperationParameters("withdraw", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateWithdrawRequest(
                _baseClient,
                opParameters,
                _baseClient.ApiCredentials!.Perps!.SignerKey!,
                "0xDCa4af75705dbB50f62437045afF9921947917d2", // The Polymarket withdraw address
                account,
                token,
                quantity,
                0,
                toAddress
                );
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/withdraw", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketWithdrawResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Transfer Internal

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpsTranfserResult>> TransferInternalAsync(
            string token,
            decimal quantity,
            string toAddress,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateSignerCredentialsAvailable(out var error))
                return HttpResult.Fail<PolymarketPerpsTranfserResult>(_baseClient.Exchange, error!);

            var account = _baseClient.ApiCredentials!.Perps!.SignerKey!.PublicAddress;
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("account", account);
            parameters.Add("token", token);
            parameters.Add("amount", quantity);
            parameters.Add("to", toAddress);
            var opParameters = PolymarketUtils.GetOperationParameters("internalTransfer", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.SignerKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/account/internal-transfer", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpsTranfserResult>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposits

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpsDeposit>>> GetDepositsAsync(
            DepositStatus? status = null,
            string? hash = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpsDeposit>>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("deposit_status", status);
            parameters.Add("hash", hash);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/deposits", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpsDeposit>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Internal Transfers

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpsTransfer>>> GetInternalTransfersAsync(
            TransferDirection? direction = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpsTransfer>>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("direction", direction);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/internal-transfers", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpsTransfer>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Withdrawals

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpsWithdrawal>>> GetWithdrawalsAsync(
            WithdrawStatus? status = null,
            string? hash = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpsWithdrawal>>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("withdrawal_status", status);
            parameters.Add("hash", hash);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/withdrawals", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpsWithdrawal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Account Stats

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketAccountStats>> GetAccountStatsAsync(CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketAccountStats>(_baseClient.Exchange, error!);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/stats", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketAccountStats>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Account Limits

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketAccountLimit>> GetAccountLimitsAsync(CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketAccountLimit> (_baseClient.Exchange, error!);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/limits", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketAccountLimit>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Leverage Margin Config

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketLeverageMargin[]>> GetLeverageMarginConfigAsync(long? symbolId = null, CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketLeverageMargin[]>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/config", PolymarketPlatform.RateLimiter.PerpsApi, 2, true);
            var result = await _baseClient.SendAsync<PolymarketLeverageMargin[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
