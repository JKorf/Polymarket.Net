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
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets.Subscriptions;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <summary>
    /// Client providing access to the Polymarket Gamma websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientGammaApi : SocketApiClient, IPolymarketSocketClientGammaApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientGammaApi(ILogger logger, PolymarketSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.GammaOptions)
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToXXXUpdatesAsync(IEnumerable<string> assetIds, Action<DataEvent<PolymarketModel>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PolymarketModel>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<PolymarketModel>(PolymarketExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        //.WithStreamId(data.Stream)
                        //.WithSymbol(data.Symbol)
                        //.WithDataTimestamp(data.EventTime)
                    );
            });

            var subscription = new PolymarketSubscription<PolymarketModel>(_logger, assetIds.ToArray(), internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/market"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            return message.GetValue<string>(_idPath);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => PolymarketExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
