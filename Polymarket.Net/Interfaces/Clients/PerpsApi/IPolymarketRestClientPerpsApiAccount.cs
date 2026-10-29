using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PerpsApi
{
    /// <summary>
    /// Polymarket Perps API account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IPolymarketRestClientPerpsApiAccount
    {
        /// <summary>
        /// Approve a builder address
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/approve-or-revoke-builder-fees-with-an-account-owner-signature" /><br />
        /// Endpoint:<br />
        /// POST /v1/account/builder-approvals<br />
        /// </para>
        /// </summary>
        /// <param name="builder">["<c>builder</c>"] Builder address</param>
        /// <param name="maxFeeRate">["<c>max_fee_rate</c>"] Max fee rate</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketBuilderResult>> ApproveBuilderAsync(
            string builder,
            decimal maxFeeRate,
            int approvalVersion = 1,
            CancellationToken ct = default);

        /// <summary>
        /// Get builder approvals
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/read-builder-approvals-including-retained-revoked-versions" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/builder-approvals<br />
        /// </para>
        /// </summary>
        /// <param name="builder">["<c>builder</c>"] The builder address</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketBuilderResult[]>> GetBuilderApprovalsAsync(string builder, CancellationToken ct = default);

        Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(DateTime expireTime, string? label = null, CancellationToken ct = default);
        Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(string proxyAddress, DateTime expireTime, string? label = null, CancellationToken ct = default);

        Task<HttpResult<PolymarketProxyResult>> DeleteProxyAsync(string proxyAddress, CancellationToken ct = default);

        /// <summary>
        /// Get registered proxies
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-credentials" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/credentials<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpsCredentials>> GetCredentialsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-balances" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/balances<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpBalance[]>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account portfolio
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-portfolio" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/portfolio<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpPortfolio>> GetPortfolioAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account equity history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-equity" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/equity<br />
        /// </para>
        /// </summary>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpValue>>> GetEquityHistoryAsync(
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get profit and loss history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-pnl" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/pnl<br />
        /// </para>
        /// </summary>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpValue>>> GetPnlHistoryAsync(
            KlineInterval interval,
            DateTime startTime,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get user funding history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-funding-charges" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/funding<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Symbol id</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketFundingHistory>>> GetFundingHistoryAsync(
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set leverage for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/update-leverage" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/leverage<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>iid</c>"] Symbol id</param>
        /// <param name="leverage">["<c>lev</c>"] Leverage</param>
        /// <param name="cross">["<c>cross</c>"] Cross margin or not</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketLeverageResult>> SetLeverageAsync(
            long symbolId,
            int leverage,
            bool cross,
            CancellationToken ct = default);

        /// <summary>
        /// Updated isolated margin for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/update-isolated-margin" /><br />
        /// Endpoint:<br />
        /// PATCH /v1/trade/margin<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>iid</c>"] Symbol id</param>
        /// <param name="quantity">["<c>amt</c>"] Signed isolated-margin adjustment in the instrument quote asset. Positive values add allocation; negative values remove it.</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> UpdateIsolatedMarginAsync(
            long symbolId,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Withdraw an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/withdraw" /><br />
        /// Endpoint:<br />
        /// POST /v1/account/withdraw<br />
        /// </para>
        /// </summary>
        /// <param name="token">["<c>token</c>"] Token contract address</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity in the token base unit, for example 100000000 for 100 USDC</param>
        /// <param name="toAddress">["<c>to</c>"] Target address</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketWithdrawResult>> WithdrawAsync(
            string token,
            long quantity,
            string toAddress,
            CancellationToken ct = default);

        /// <summary>
        /// Transfer internally
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/internal-transfer" /><br />
        /// Endpoint:<br />
        /// POST /v1/account/internal-transfer<br />
        /// </para>
        /// </summary>
        /// <param name="token">["<c>token</c>"] Token contract</param>
        /// <param name="quantity">["<c>amount</c>"] Token amount in decimal units (e.g. "100" for 100 USDC).</param>
        /// <param name="toAddress">["<c>to</c>"] Target address</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPerpsTranfserResult>> TransferInternalAsync(
            string token,
            decimal quantity,
            string toAddress,
            CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-deposits" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/deposits<br />
        /// </para>
        /// </summary>
        /// <param name="status">["<c>deposit_status</c>"] Filter by status</param>
        /// <param name="hash">["<c>hash</c>"] Filter by hash</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpsDeposit>>> GetDepositsAsync(
            DepositStatus? status = null,
            string? hash = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get internal transfer history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-internal-transfers" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/internal-transfers<br />
        /// </para>
        /// </summary>
        /// <param name="direction">["<c>direction</c>"] Filter by direction</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpsTransfer>>> GetInternalTransfersAsync(
            TransferDirection? direction = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-withdrawals" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/withdrawals<br />
        /// </para>
        /// </summary>
        /// <param name="status">["<c>withdrawal_status</c>"] Filter by status</param>
        /// <param name="hash">["<c>hash</c>"] Filter by hash</param>
        /// <param name="startTime">["<c>start_timestamp</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_timestamp</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPaged<PolymarketPerpsWithdrawal>>> GetWithdrawalsAsync(
            WithdrawStatus? status = null,
            string? hash = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get account stats
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-account-stats" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/stats<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketAccountStats>> GetAccountStatsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account limits
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-account-limits" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/limits<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketAccountLimit>> GetAccountLimitsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get leverage and margin mode configs
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/get-instrument-config" /><br />
        /// Endpoint:<br />
        /// GET /v1/account/config<br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>instrument_id</c>"] Filter by symbol id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketLeverageMargin[]>> GetLeverageMarginConfigAsync(long? symbolId = null, CancellationToken ct = default);

    }
}
