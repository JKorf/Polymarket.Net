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
using System.Collections.Generic;
using System.Linq;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    internal class PolymarketPerpsSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly IEnumerable<string>? _symbolIds;
        private readonly Action<DataEvent<T>> _handler;
        private readonly bool _all;
        private readonly string? _postFix;

        public PolymarketPerpsSubscription(ILogger logger, SocketApiClient client, string topic, IEnumerable<long>? symbolIds, Action<DataEvent<T>> handler, bool all = false, string? postFix = null) : base(logger, false, true)
        {
            _client = client;
            _topic = topic;
            _symbolIds = symbolIds?.Select(x => x.ToString()).ToArray();
            _handler = handler;
            _postFix = postFix;
            _all = all;

            if (_symbolIds == null)
            {
                MessageRouter = MessageRouter.CreateForEvent<PolymarketPerpsSocketUpdate<T>>(topic, HandleMessage);
            }
            else
            {
                if (_postFix == null)
                    MessageRouter = MessageRouter.CreateForEvent<PolymarketPerpsSocketUpdate<T>>(topic, _symbolIds, HandleMessage);
                else
                    MessageRouter = MessageRouter.CreateForEvent<PolymarketPerpsSocketUpdate<T>>(topic, _symbolIds.Select(x => x + _postFix).ToArray(), HandleMessage);
            }
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PolymarketPerpsQuery<PolymarketPerpsSocketResponse>(new PolymarketPerpsChannelSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "sub",
                Channels = GetChannels()
            }, Authenticated);
        }

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new PolymarketPerpsQuery<PolymarketPerpsSocketResponse>(new PolymarketPerpsChannelSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "unsub",
                Channels = GetChannels()
            }, Authenticated);
        }

        private string[] GetChannels()
        {
            if (_all)
                return [_topic + "::all"];

            if (_symbolIds == null)
                return [_topic];

            if (_postFix == null)
                return _symbolIds.Select(x => $"{_topic}::{x}").ToArray();

            return _symbolIds.Select(x => $"{_topic}::{x}::{_postFix}").ToArray();
        }

        private CallResult? HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketPerpsSocketUpdate<T> message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _handler.Invoke(new DataEvent<T>(_client.Exchange, message.Data, receiveTime, originalData)
                .WithStreamId(message.Channel)
                //.WithSymbol(_symbolId)
                .WithSequenceNumber(message.Sequence)
                .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));

            return CallResult.Ok();
        }
    }
}
