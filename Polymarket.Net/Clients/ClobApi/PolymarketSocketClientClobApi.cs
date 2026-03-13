using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <summary>
    /// Client providing access to the Polymarket Clob websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientClobApi : SocketApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketSocketClientClobApi
    {
        #region fields
        private string _sportUri;

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientClobApi(ILogger logger, PolymarketSocketOptions options) :
            base(logger, options.Environment.ClobSocketClientAddress!, options, options.ClobOptions)
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

            _sportUri = options.Environment.SportSocketClientAddress;
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToPlatformUpdatesAsync(
            Action<DataEvent<PolymarketNewMarketUpdate>>? onNewMarketUpdate = null,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? onMarketResolvedUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketGeneralSubscription(
                _logger,
                this,
                onNewMarketUpdate,
                onMarketResolvedUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTokenUpdatesAsync(
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
            return await SubscribeAsync(BaseAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserUpdatesAsync(
            Action<DataEvent<PolymarketOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<PolymarketTradeUpdate>>? onTradeUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketUserSubscription(
                _logger,
                this,
                onOrderUpdate,
                onTradeUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/user"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSportsUpdatesAsync(
            Action<DataEvent<PolymarketSportsUpdate>>? onSportsUpdate = null,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketSportSubscription(
                _logger,
                onSportsUpdate);
            return await SubscribeAsync(_sportUri.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();
    }
}
