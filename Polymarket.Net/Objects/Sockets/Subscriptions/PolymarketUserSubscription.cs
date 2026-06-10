using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class PolymarketUserSubscription : Subscription
    {
        private readonly Action<DataEvent<PolymarketOrderUpdate>>? _orderUpdate;
        private readonly Action<DataEvent<PolymarketTradeUpdate>>? _tradeUpdate;

        private PolymarketSocketClientClobApi _client;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketUserSubscription(
            ILogger logger,
            PolymarketSocketClientClobApi client,
            Action<DataEvent<PolymarketOrderUpdate>>? orderUpdate,
            Action<DataEvent<PolymarketTradeUpdate>>? tradeUpdate
            ) : base(logger, true)
        {
            _client = client;
            _orderUpdate = orderUpdate;
            _tradeUpdate = tradeUpdate;

            MessageRouter = MessageRouter.Create([
                MessageRoute.CreateForEvent<PolymarketTradeUpdate>("trade", DoHandleMessage),
                MessageRoute.CreateForEvent<PolymarketOrderUpdate>("order", DoHandleMessage)
                ]);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketTradeUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _tradeUpdate?.Invoke(new DataEvent<PolymarketTradeUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.Ok();
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketOrderUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _orderUpdate?.Invoke(new DataEvent<PolymarketOrderUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.Ok();
        }
    }
}
