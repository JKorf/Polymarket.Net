using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PredictionApi
{
    /// <summary>
    /// Polymarket Perps WebSocket API
    /// </summary>
    public interface IPolymarketSocketClientPerpsApiAccount
    {        
        /// <summary>
        /// Subscribe to user balance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-balances" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(
            Action<DataEvent<PolymarketPerpBalance>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user funding updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-funding" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToUserFundingUpdatesAsync(
            Action<DataEvent<PolymarketUserFundingUpdate>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user funding updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-funding" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToPortfolioUpdatesAsync(
            Action<DataEvent<PolymarketPerpPortfolio>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to deposit updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-deposits" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(
            Action<DataEvent<PolymarketPerpsDepositUpdate[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to withdrawal updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-withdrawals" /><br />
        /// </para>
        /// </summary>
        /// <param name="onUpdate">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToWithdrawalUpdatesAsync(
            Action<DataEvent<PolymarketPerpsWithdrawalUpdate[]>> onUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Set new leverage
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-update-leverage" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>iid</c>"] Symbol id</param>
        /// <param name="leverage">["<c>lev</c>"] Leverage</param>
        /// <param name="cross">["<c>cross</c>"] Cross margin or not</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult> SetLeverageAsync(
            long symbolId,
            int leverage,
            bool cross,
            CancellationToken ct = default);

        /// <summary>
        /// Update isolated margin for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/wss/perps-update-margin" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolId">["<c>iid</c>"] Symbol id</param>
        /// <param name="quantity">["<c>amt</c>"] Signed isolated-margin adjustment in the instrument quote asset. Positive values add allocation; negative values remove it.</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult> UpdateIsolatedMarginAsync(
            long symbolId,
            decimal quantity,
            CancellationToken ct = default);
    }
}
