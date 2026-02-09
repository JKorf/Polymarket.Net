using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class PolymarketTokenSubscription : Subscription
    {
        private readonly Action<DataEvent<PolymarketPriceChangeUpdate>>? _priceChangeHandler;
        private readonly Action<DataEvent<PolymarketBookUpdate>>? _bookHandler;
        private readonly Action<DataEvent<PolymarketLastTradePriceUpdate>>? _lastTradePriceHandler;
        private readonly Action<DataEvent<PolymarketTickSizeUpdate>>? _lastTickSizeHandler;
        private readonly Action<DataEvent<PolymarketBestBidAskUpdate>>? _bidAskUpdateHandler;
        private readonly string[] _tokenIds;

        private PolymarketSocketClientClobApi _client;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketTokenSubscription(
            ILogger logger,
            PolymarketSocketClientClobApi client,
            string[] tokenIds,
            Action<DataEvent<PolymarketPriceChangeUpdate>>? priceChangeHandler,
            Action<DataEvent<PolymarketBookUpdate>>? bookHandler,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? lastTradePriceHandler,
            Action<DataEvent<PolymarketTickSizeUpdate>>? tickSizeUpdateHandler,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? bidAskUpdateHandler
            ) : base(logger, false)
        {
            _client = client;
            _priceChangeHandler = priceChangeHandler;
            _bookHandler = bookHandler;
            _lastTradePriceHandler = lastTradePriceHandler;
            _lastTickSizeHandler = tickSizeUpdateHandler;
            _bidAskUpdateHandler = bidAskUpdateHandler;
            _tokenIds = tokenIds;

            IndividualSubscriptionCount = tokenIds.Length;

            var routes = new List<MessageRoute>();

            foreach(var item in tokenIds)
            {
                routes.Add(MessageRoute<PolymarketPriceChangeUpdate>.CreateWithoutTopicFilter("price_change", DoHandleMessage));
                routes.Add(MessageRoute<PolymarketBookUpdate>.CreateWithTopicFilter("book", item, DoHandleMessage));
                routes.Add(MessageRoute<PolymarketBookUpdate[]>.CreateWithTopicFilter("book_snapshot", item, DoHandleMessage));
                routes.Add(MessageRoute<PolymarketLastTradePriceUpdate>.CreateWithTopicFilter("last_trade_price", item, DoHandleMessage));
                routes.Add(MessageRoute<PolymarketTickSizeUpdate>.CreateWithTopicFilter("tick_size_change", item, DoHandleMessage));
                routes.Add(MessageRoute<PolymarketBestBidAskUpdate>.CreateWithTopicFilter("best_bid_ask", item, DoHandleMessage));
            }

            MessageRouter = MessageRouter.Create(routes.ToArray());
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PolymarketQuery<object>("subscribe", _tokenIds);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new PolymarketQuery<object>("unsubscribe", _tokenIds);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketPriceChangeUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            var updates = message.PriceChanges.Where(x => _tokenIds.Contains(x.AssetId)).ToArray();
            if (updates.Length == 0)
                return CallResult.SuccessResult;

            var update = message with { PriceChanges = updates };
            _priceChangeHandler?.Invoke(new DataEvent<PolymarketPriceChangeUpdate>(PolymarketPlatform.Metadata.Id, update, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketTickSizeUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _lastTickSizeHandler?.Invoke(new DataEvent<PolymarketTickSizeUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBookUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _bookHandler?.Invoke(new DataEvent<PolymarketBookUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBookUpdate[] messages)
        {
            foreach (var message in messages)
            {
                if (!_tokenIds.Contains(message.AssetId))
                    continue;

                _bookHandler?.Invoke(new DataEvent<PolymarketBookUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                            .WithUpdateType(SocketUpdateType.Snapshot)
                            .WithStreamId(message.EventType)
                            //.WithSymbol(data.Symbol)
                            .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            }

            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketLastTradePriceUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _lastTradePriceHandler?.Invoke(new DataEvent<PolymarketLastTradePriceUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBestBidAskUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _bidAskUpdateHandler?.Invoke(new DataEvent<PolymarketBestBidAskUpdate>(PolymarketPlatform.Metadata.Id, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return CallResult.SuccessResult;
        }
    }
}
