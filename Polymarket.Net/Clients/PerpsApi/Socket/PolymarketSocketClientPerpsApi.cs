using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    /// <summary>
    /// Client providing access to the Polymarket Perps websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientPerpsApi : SocketApiClient<PolymarketEnvironment, PolymarketPerpsAuthenticationProvider, PolymarketCredentials>, IPolymarketSocketClientPerpsApi
    {
        #region fields

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientPerpsApi(ILoggerFactory? loggerFactory, PolymarketSocketOptions options) :
            base(loggerFactory, PolymarketPlatform.Metadata.Id, options.Environment.PerpsSocketClientAddress!, options, options.PredictionOptions)
        {
            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(10),
                x => 
                    new PolymarketPerpsQuery<PolymarketPerpsSocketResponse>(new PolymarketPerpsOpSocketRequest
                    {
                        Id = ExchangeHelpers.NextId(),
                        Request = "post",
                        Op = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings)
                        {
                            { "type", "ping" }
                        }
                    }, false),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PolymarketSocketPerpsMessageHandler();

        /// <inheritdoc />
        protected override PolymarketPerpsAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketPerpsAuthenticationProvider(credentials);

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

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
                this,
                "trades",
                symbolIds,
                onUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
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
                this,
                "tickers",
                symbolIds,
                onUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(
            Action<DataEvent<PolymarketPerpTickerUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpTickerUpdate>(
                _logger,
                this,
                "tickers",
                null,
                onUpdate,
                true);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
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
                this,
                "bbo",
                symbolIds,
                onUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
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
                this,
                "book",
                symbolIds,
                onUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
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
                this,
                "klines",
                symbolIds,
                onUpdate,
                postFix: intervalStr);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
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
                this,
                "statistics",
                symbolIds,
                onUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllStatisticsUpdatesAsync(
            Action<DataEvent<PolymarketStatisticsUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketStatisticsUpdate>(
                _logger,
                this,
                "statistics",
                null,
                onUpdate,
                true);
            return await SubscribeAsync(BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
