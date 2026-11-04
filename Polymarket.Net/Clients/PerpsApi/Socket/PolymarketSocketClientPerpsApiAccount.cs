using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal class PolymarketSocketClientPerpsApiAccount : IPolymarketSocketClientPerpsApiAccount
    {
        private readonly ILogger _logger;
        private readonly PolymarketSocketClientPerpsApi _baseClient;

        public PolymarketSocketClientPerpsApiAccount(ILogger logger, PolymarketSocketClientPerpsApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Balances

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(
            Action<DataEvent<PolymarketPerpBalance>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpBalance>(
                _logger,
                _baseClient,
                "balances",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region User Funding

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserFundingUpdatesAsync(
            Action<DataEvent<PolymarketUserFundingUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketUserFundingUpdate>(
                _logger,
                _baseClient,
                "funding",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Portfolio

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPortfolioUpdatesAsync(
            Action<DataEvent<PolymarketPerpPortfolio>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpPortfolio>(
                _logger,
                _baseClient,
                "portfolio",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Deposits

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(
            Action<DataEvent<PolymarketPerpsDepositUpdate[]>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpsDepositUpdate[]>(
                _logger,
                _baseClient,
                "deposits",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdrawals

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToWithdrawalUpdatesAsync(
            Action<DataEvent<PolymarketPerpsWithdrawalUpdate[]>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpsWithdrawalUpdate[]>(
                _logger,
                _baseClient,
                "withdrawals",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<QueryResult> SetLeverageAsync(
            long symbolId,
            int leverage,
            bool cross,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, true, out var error))
                return QueryResult.Fail(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("iid", symbolId);
            parameters.Add("lev", leverage);
            parameters.Add("cross", cross);
            return await _baseClient.Query<PolymarketStatus>("updateLeverage", parameters).ConfigureAwait(false);
        }

        #endregion

        #region Update Isolated Margin

        /// <inheritdoc />
        public async Task<QueryResult> UpdateIsolatedMarginAsync(
            long symbolId,
            decimal quantity,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, true, out var error))
                return QueryResult.Fail(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("iid", symbolId);
            parameters.Add("amt", quantity);
            return await _baseClient.Query<PolymarketStatus>("updateMargin", parameters).ConfigureAwait(false);
        }

        #endregion
    }
}
