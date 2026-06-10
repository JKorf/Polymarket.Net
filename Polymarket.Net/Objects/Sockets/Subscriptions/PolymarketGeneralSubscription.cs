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
    internal class PolymarketGeneralSubscription : Subscription
    {
        private readonly Action<DataEvent<PolymarketNewMarketUpdate>>? _newMarketUpdate;
        private readonly Action<DataEvent<PolymarketMarketResolvedUpdate>>? _marketResolvedUpdate;

        private PolymarketSocketClientClobApi _client;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketGeneralSubscription(
            ILogger logger,
            PolymarketSocketClientClobApi client,
            Action<DataEvent<PolymarketNewMarketUpdate>>? newMarketUpdate,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? marketResolvedUpdate
            ) : base(logger, false)
        {
            _client = client;
            _newMarketUpdate = newMarketUpdate;
            _marketResolvedUpdate = marketResolvedUpdate;

            MessageRouter = MessageRouter.Create([
                MessageRoute.CreateForEvent<PolymarketNewMarketUpdate>("new_market", DoHandleMessage),
                MessageRoute.CreateForEvent<PolymarketMarketResolvedUpdate>("market_resolved", DoHandleMessage)
                ]);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PolymarketInitialQuery<object>("MARKET", true);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketNewMarketUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _newMarketUpdate?.Invoke(new DataEvent<PolymarketNewMarketUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.Ok();
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketMarketResolvedUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _marketResolvedUpdate?.Invoke(new DataEvent<PolymarketMarketResolvedUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.Ok();
        }
    }
}
