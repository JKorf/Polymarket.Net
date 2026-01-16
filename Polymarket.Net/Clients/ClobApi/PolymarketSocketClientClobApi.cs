using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
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
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets.Subscriptions;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <summary>
    /// Client providing access to the Polymarket Clob websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientClobApi : SocketApiClient, IPolymarketSocketClientClobApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientClobApi(ILogger logger, PolymarketSocketOptions options) :
            base(logger, options.Environment.ClobSocketClientAddress!, options, options.ClobOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(PolymarketExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PolymarketSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new PolymarketAuthenticationProvider((PolymarketCredentials)credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTokenUpdatesAsync(
            IEnumerable<string> assetIds,
            Action<DataEvent<PolymarketPriceChangeUpdate>>? onPriceChangeUpdate = null, 
            Action<DataEvent<PolymarketBookUpdate>>? onBookUpdate = null,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? onLastTradePriceUpdate = null,
            Action<DataEvent<PolymarketTickSizeUpdate>>? onTickSizeUpdate = null,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? onBestBidAskUpdate = null,
            Action<DataEvent<PolymarketNewMarketUpdate>>? onNewMarketUpdate = null,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? onMarketResolvedUpdate = null,
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
                onBestBidAskUpdate,
                onNewMarketUpdate,
                onMarketResolvedUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            return message.GetValue<string>(_idPath);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();
    }
}
