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
        /// <summary>
        /// Create API credentials for the provided API credentials private key
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/authentication#l2-authentication" /><br />
        /// Endpoint:<br />
        /// POST /auth/api-key
        /// </para>
        /// </summary>
        /// <param name="nonce">["<c>nonce</c>"] Nonce, different nonces can be used to create different credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketCreds>> CreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default);

        /// <summary>
        /// Get previously created API credentials for the provided API credentials private key
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/authentication#l2-authentication" /><br />
        /// Endpoint:<br />
        /// GET /auth/derive-api-key
        /// </para>
        /// </summary>
        /// <param name="nonce">["<c>nonce</c>"] Nonce</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketCreds>> GetApiCredentialsAsync(long? nonce = null, CancellationToken ct = default);

        /// <summary>
        /// Get previously created API credentials, or create new credentials if no credentials are created yet
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/developers/CLOB/authentication#l2-authentication" /><br />
        /// Endpoint:<br />
        /// GET /auth/derive-api-key<br />
        /// POST /auth/api-key
        /// </para>
        /// </summary>
        /// <param name="nonce">["<c>nonce</c>"] Nonce</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketCreds>> GetOrCreateApiCredentialsAsync(long? nonce = null, CancellationToken ct = default);

        /// <summary>
        /// List API keys for the provided API credentials private key
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketApiKeys>> GetApiKeysAsync(CancellationToken ct = default);

        /// <summary>
        /// Delete an API key
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> DeleteApiKeyAsync(CancellationToken ct = default);

        /// <summary>
        /// Get closed only mode
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketClosedOnlyMode>> GetClosedOnlyModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get notifications
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketNotification[]>> GetNotificationsAsync(CancellationToken ct = default);

        /// <summary>
        /// Drop notifications
        /// </summary>
        /// <param name="ids">["<c>ids</c>"] Ids to drop</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketNotification[]>> DropNotificationsAsync(IEnumerable<string> ids, CancellationToken ct = default);

        /// <summary>
        /// Get balance allowance
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/trading/clients/l2#getbalanceallowance" /><br />
        /// Endpoint:<br />
        /// GET /balance-allowance
        /// </para>
        /// </summary>
        /// <param name="assetType">["<c>asset_type</c>"] Asset type</param>
        /// <param name="tokenId">["<c>token_id</c>"] Token id, required for AssetType.Conditional</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketBalanceAllowance>> GetBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default);

        /// <summary>
        /// Update balance allowance
        /// </summary>
        /// <param name="assetType">["<c>asset_type</c>"] Asset type</param>
        /// <param name="tokenId">["<c>token_id</c>"] Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> UpdateBalanceAllowanceAsync(AssetType assetType, string? tokenId = null, CancellationToken ct = default);

        /// <summary>
        /// Get trades for builder
        /// </summary>
        /// <param name="builderCode">["<c>builder_code</c>"] Builder code to request for</param>
        /// <param name="tradeId">["<c>id</c>"] Filter by trade id</param>
        /// <param name="marketId">["<c>market</c>"] Filter by condition id</param>
        /// <param name="tokenId">["<c>asset_id</c>"] Filter by token id</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="cursor">["<c>next_cursor</c>"] Next page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPage<PolymarketBuilderTrade>>> GetBuilderTradesAsync(
            string builderCode,
            string? tradeId = null,
            string? marketId = null,
            string? tokenId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null, 
            CancellationToken ct = default);
    }
}
