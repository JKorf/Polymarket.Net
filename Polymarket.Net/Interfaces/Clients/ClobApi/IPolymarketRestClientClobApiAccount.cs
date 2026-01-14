using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.ClobApi
{
    /// <summary>
    /// Polymarket Clob account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IPolymarketRestClientClobApiAccount
    {
        Task<WebCallResult<PolymarketCreds>> CreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default);

        Task<WebCallResult<PolymarketCreds>> GetApiCredentialsAsync(long? nonce = null, CancellationToken ct = default);

        Task<WebCallResult<PolymarketCreds>> GetOrCreateApiCredentialsAsync(long? nonce = null);

        Task<WebCallResult<PolymarketApiKeys>> GetApiKeysAsync(CancellationToken ct = default);

        Task<WebCallResult> DeleteApiKeyAsync(CancellationToken ct = default);

        Task<WebCallResult<PolymarketClosedOnlyMode>> GetClosedOnlyModeAsync(CancellationToken ct = default);


        /// <summary>
        /// Get notifications
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketNotification[]>> GetNotificationsAsync(CancellationToken ct = default);

        /// <summary>
        /// Drop notifications
        /// </summary>
        /// <param name="ids">Ids to drop</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketNotification[]>> DropNotificationsAsync(IEnumerable<string> ids, CancellationToken ct = default);

        /// <summary>
        /// Get balance allowance
        /// </summary>
        /// <param name="assetType">Asset type</param>
        /// <param name="tokenId">Token id, required for AssetType.Conditional</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketBalanceAllowance>> GetBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default);

        /// <summary>
        /// Update balance allowance
        /// </summary>
        /// <param name="assetType">Asset type</param>
        /// <param name="tokenId">Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> UpdateBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default);

        /// <summary>
        /// Get trades for builder
        /// </summary>
        /// <param name="tradeId">Filter by trade id</param>
        /// <param name="takerAddress">Filter by taker address</param>
        /// <param name="makerAddress">Filter by maker address</param>
        /// <param name="conditionId">Filter by condition id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="cursor">Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> GetBuilderTradesAsync(
            string? tradeId = null,
            string? takerAddress = null,
            string? makerAddress = null,
            string? conditionId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null, 
            CancellationToken ct = default);
    }
}
