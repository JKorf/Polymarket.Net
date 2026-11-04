using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Objects.Internal;
using System;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    internal class PolymarketBoltSubscription<TSnapshot, TUpdate> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly string? _symbol;
        private readonly int? _twapInterval;
        private readonly Action<DataEvent<TUpdate>> _handler;
        private readonly Action<DataEvent<PolymarketBoltSnapshotUpdate<TSnapshot[]>>> _snapshotHandler;

        public PolymarketBoltSubscription(
            SocketApiClient client,
            ILogger logger,
            string topic,
            string? symbol,
            Action<DataEvent<PolymarketBoltSnapshotUpdate<TSnapshot[]>>> snapshotHandler,
            Action<DataEvent<TUpdate>> handler,
            int? twapInterval = null)
            : base(logger, true, true)
        {
            _client = client;
            _topic = topic;
            _symbol = symbol;
            _twapInterval = twapInterval;
            _snapshotHandler = snapshotHandler;
            _handler = handler;

            if (_symbol == null)
            {
                var routes = new[] {
                     MessageRoute.CreateForEvent<PolymarketBoltUpdate<TUpdate>>(_topic, HandleMessage),
                     MessageRoute.CreateForEvent<PolymarketBoltUpdate<PolymarketBoltSnapshotUpdate<TSnapshot[]>>>(_topic + "snapshot", HandleMessage)
                    };
                MessageRouter = MessageRouter.Create(routes);
            }
            else
            {
                var routes = new[] {
                     MessageRoute.CreateForEvent<PolymarketBoltUpdate<TUpdate>>(_topic, _symbol, HandleMessage),
                     MessageRoute.CreateForEvent<PolymarketBoltUpdate<PolymarketBoltSnapshotUpdate<TSnapshot[]>>>(_topic + "snapshot", _symbol, HandleMessage)
                    };
                MessageRouter = MessageRouter.Create(routes);
            }
        }

        private CallResult? HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBoltUpdate<TUpdate> data)
        {
            _client.UpdateTimeOffset(data.Timestamp);

            _handler(
                new DataEvent<TUpdate>(_client.Exchange, data.Payload, receiveTime, originalData)
                    .WithStreamId(data.Channel)
                    .WithDataTimestamp(data.Timestamp, _client.GetTimeOffset())
                );

            return CallResult.Ok();
        }
        private CallResult? HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBoltUpdate<PolymarketBoltSnapshotUpdate<TSnapshot[]>> data)
        {
            _client.UpdateTimeOffset(data.Timestamp);

            _snapshotHandler(
                new DataEvent<PolymarketBoltSnapshotUpdate<TSnapshot[]>>(_client.Exchange, data.Payload, receiveTime, originalData)
                    .WithStreamId(data.Channel)
                    .WithSymbol(data.Payload.Symbol)
                    .WithDataTimestamp(data.Timestamp, _client.GetTimeOffset())
                );
            return CallResult.Ok();
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            var filter = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            filter.Add("symbol", _symbol);
            filter.Add("window_seconds", _twapInterval);

            return new PolymarketBoltQuery(
                new Internal.PolymarketBoltSubscribeRequest
                {
                    Operation = "subscribe",
                    RequestId = ExchangeHelpers.NextId().ToString(),
                    Subscriptions = [
                        new Internal.PolymarketBoltSub
                        {
                            Channel = _topic,
                            Filters = filter
                        }
                        ]
                },
                Authenticated
                );
        }

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            var filter = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            filter.Add("symbol", _symbol);
            filter.Add("window_seconds", _twapInterval);

            return new PolymarketBoltQuery(
                new Internal.PolymarketBoltSubscribeRequest
                {
                    Operation = "unsubscribe",
                    RequestId = ExchangeHelpers.NextId().ToString(),
                    Subscriptions = [
                        new Internal.PolymarketBoltSub
                        {
                            Channel = _topic,
                            Filters = filter
                        }
                        ]
                },
                Authenticated
                );
        }
    }
}
