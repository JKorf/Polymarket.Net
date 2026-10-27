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
using CryptoExchange.Net.Sockets.Interfaces;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Internal;
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
    /// Client providing access to the Polymarket Data websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientDataApi : SocketApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketSocketClientDataApi
    {
        #region fields
        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientDataApi(ILoggerFactory? loggerFactory, PolymarketSocketOptions options) :
            base(loggerFactory, PolymarketPlatform.Metadata.Id, options.Environment.PolyBoltSocketClientAddress!, options, options.GammaOptions)
        {
            //RegisterPeriodicQuery(
            //    "Ping",
            //    TimeSpan.FromSeconds(10),
            //    x => new PolymarketPingQuery(),
            //    (connection, result) =>
            //    {
            //        if (result.Error?.ErrorType == ErrorType.Timeout)
            //        {
            //            // Ping timeout, reconnect
            //            _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
            //            _ = connection.TriggerReconnectAsync();
            //        }
            //    });
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PolymarketSocketBoltMessageHandler();

        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

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
            return await SubscribeAsync(BaseAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(BaseAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(BaseAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(BaseAddress.AppendPath("ws"), subscription, ct).ConfigureAwait(false);
        }

        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection, IMessageProcessor? processor) {
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
