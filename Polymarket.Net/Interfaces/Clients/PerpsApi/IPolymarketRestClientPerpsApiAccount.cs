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

        Task<HttpResult<PolymarketProxyResult>> CreateProxyAsync(string proxyAddress, DateTime expireTime, CancellationToken ct = default);
        Task<HttpResult<PolymarketProxyResult>> DeleteProxyAsync(string proxyAddress, CancellationToken ct = default);

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

    }
}
