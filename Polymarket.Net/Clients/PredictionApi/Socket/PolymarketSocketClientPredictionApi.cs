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

namespace Polymarket.Net.Clients.PredictionApi
{
    /// <summary>
    /// Client providing access to the Polymarket Prediction websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientPredictionApi : SocketApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketSocketClientPredictionApi
    {
        #region fields

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        internal string ClobAddress => ClientOptions.Environment.ClobSocketClientAddress!;
        internal string PolyboltAddress => ClientOptions.Environment.PolyBoltSocketClientAddress!;
        internal string SportAddress => ClientOptions.Environment.SportSocketClientAddress!;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientPredictionApi(ILoggerFactory? loggerFactory, PolymarketSocketOptions options) :
            base(loggerFactory, PolymarketPlatform.Metadata.Id, options.Environment.ClobSocketClientAddress!, options, options.PredictionOptions)
        {
            AddSystemSubscription(new PolymarketGeneralSystemSubscription(_logger));

            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(10),
                x => new PolymarketPingQuery(),
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
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PolymarketSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPlatformUpdatesAsync(
            Action<DataEvent<PolymarketNewMarketUpdate>>? onNewMarketUpdate = null,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? onMarketResolvedUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketGeneralSubscription(
                _logger,
                this,
                onNewMarketUpdate,
                onMarketResolvedUpdate);
            return await SubscribeAsync(ClobAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTokenUpdatesAsync(
            IEnumerable<string> assetIds,
            Action<DataEvent<PolymarketPriceChangeUpdate>>? onPriceChangeUpdate = null, 
            Action<DataEvent<PolymarketBookUpdate>>? onBookUpdate = null,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? onLastTradePriceUpdate = null,
            Action<DataEvent<PolymarketTickSizeUpdate>>? onTickSizeUpdate = null,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? onBestBidAskUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketTokenSubscription(
                _logger,
                this,
                assetIds.ToArray(),
                onPriceChangeUpdate,
                onBookUpdate,
                onLastTradePriceUpdate,
                onTickSizeUpdate,
                onBestBidAskUpdate);
            return await SubscribeAsync(ClobAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserUpdatesAsync(
            Action<DataEvent<PolymarketOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<PolymarketTradeUpdate>>? onTradeUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketUserSubscription(
                _logger,
                this,
                onOrderUpdate,
                onTradeUpdate);
            return await SubscribeAsync(ClobAddress.AppendPath("ws/user"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSportsUpdatesAsync(
            Action<DataEvent<PolymarketSportsUpdate>>? onSportsUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketSportSubscription(
                _logger,
                onSportsUpdate);
            return await SubscribeAsync(SportAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToCryptoPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketPriceUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketBoltSubscription<PolymarketPriceUpdate, PolymarketPriceUpdate>(
                this,
                _logger,
                "price.crypto",
                symbol,
                onSnapshot,
                onUpdate);
            return await SubscribeAsync(PolyboltAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToCryptoTwapPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketPriceUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketBoltSubscription<PolymarketPriceUpdate, PolymarketPriceUpdate>(
                this,
                _logger,
                "price.crypto.twap",
                symbol,
                onSnapshot,
                onUpdate,
                60);
            return await SubscribeAsync(PolyboltAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToEquityPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketEquityPriceUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketBoltSubscription<PolymarketPriceUpdate, PolymarketEquityPriceUpdate>(
                this,
                _logger,
                "price.equity",
                symbol,
                onSnapshot,
                onUpdate);
            return await SubscribeAsync(PolyboltAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToEquityTwapPriceUpdatesAsync(
            string symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>> onSnapshot,
            Action<DataEvent<PolymarketEquityPriceUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketBoltSubscription<PolymarketPriceUpdate, PolymarketEquityPriceUpdate>(
                this,
                _logger,
                "price.equity.twap",
                symbol,
                onSnapshot,
                onUpdate,
                60);
            return await SubscribeAsync(PolyboltAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection, IMessageProcessor? processor)
        {
            var msg = new PolymarketBoltAuthRequest
            {
                Operation = "auth",
                RequestId = ExchangeHelpers.NextId().ToString(),
                Auth = new PolymarketBoltAuth
                {
                    ApiKey = AuthenticationProvider.ApiCredentials.L2.Key,
                    Secret = AuthenticationProvider.ApiCredentials.L2.Secret,
                    Passphrase = AuthenticationProvider.ApiCredentials.L2.Pass
                }
            };
            return Task.FromResult<Query?>(new PolymarketBoltQuery(msg, false));
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();
    }
}
