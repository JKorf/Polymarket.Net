using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal class PolymarketSocketClientPerpsApiExchangeData : IPolymarketSocketClientPerpsApiExchangeData
    {
        private readonly ILogger _logger;
        private readonly PolymarketSocketClientPerpsApi _baseClient;

        public PolymarketSocketClientPerpsApiExchangeData(ILogger logger, PolymarketSocketClientPerpsApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Trades

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpTrade[]>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToTradeUpdatesAsync(new[] { symbolId }, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpTrade[]>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpTrade[]>(
                _logger,
                _baseClient,
                "trades",
                symbolIds,
                onUpdate);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Tickers

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToTickerUpdatesAsync(new[] { symbolId }, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpTickerUpdate>(
                _logger,
                _baseClient,
                "tickers",
                symbolIds,
                onUpdate);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpTickerUpdate>(
                _logger,
                _baseClient,
                "tickers",
                null,
                onUpdate,
                true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Book Ticker

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpBookTickerUpdate>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToBookTickerUpdatesAsync(new[] { symbolId }, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpBookTickerUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpBookTickerUpdate>(
                _logger,
                _baseClient,
                "bbo",
                symbolIds,
                onUpdate);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order Book

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketPerpBookUpdate>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToOrderBookUpdatesAsync(new[] { symbolId }, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketPerpBookUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpBookUpdate>(
                _logger,
                _baseClient,
                "book",
                symbolIds,
                onUpdate);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Klines

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            long symbolId,
            KlineInterval interval,
            Action<DataEvent<PolymarketKline[]>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToKlineUpdatesAsync(new[] { symbolId }, interval, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            IEnumerable<long> symbolIds,
            KlineInterval interval,
            Action<DataEvent<PolymarketKline[]>> onUpdate,
            CancellationToken ct = default)
        {
            var intervalStr = EnumConverter.GetString(interval);
            var subscription = new PolymarketPerpsSubscription<PolymarketKline[]>(
                _logger,
                _baseClient,
                "klines",
                symbolIds,
                onUpdate,
                postFix: intervalStr);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Statistics

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToStatisticsUpdatesAsync(
            long symbolId,
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default)
            => await SubscribeToStatisticsUpdatesAsync(new[] { symbolId }, onUpdate, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToStatisticsUpdatesAsync(
            IEnumerable<long> symbolIds,
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketStatisticsUpdate>(
                _logger,
                _baseClient,
                "statistics",
                symbolIds,
                onUpdate);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllStatisticsUpdatesAsync(
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketStatisticsUpdate>(
                _logger,
                _baseClient,
                "statistics",
                null,
                onUpdate,
                true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
