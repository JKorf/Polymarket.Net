using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Objects.Models;
using System;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class PolymarketSportSubscription : Subscription
    {
        private readonly Action<DataEvent<PolymarketSportsUpdate>>? _updateHandler;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketSportSubscription(
            ILogger logger,
            Action<DataEvent<PolymarketSportsUpdate>>? updateHandler
            ) : base(logger, false)
        {
            _updateHandler = updateHandler;

            MessageRouter = MessageRouter.CreateForEvent<PolymarketSportsUpdate>("sports", DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection) => null;


        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketSportsUpdate message)
        {
            _updateHandler?.Invoke(new DataEvent<PolymarketSportsUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId("sports"));
            return CallResult.Ok();
        }

    }
}
