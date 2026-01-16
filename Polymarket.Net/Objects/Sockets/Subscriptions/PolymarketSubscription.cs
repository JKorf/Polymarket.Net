using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Objects.Models;
using System;

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
        private readonly Action<DataEvent<PolymarketNewMarketUpdate>>? _newMarketUpdate;
        private readonly Action<DataEvent<PolymarketMarketResolvedUpdate>>? _marketResolvedUpdate;
        private readonly string[] _assetIds;

        private PolymarketSocketClientClobApi _client;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketTokenSubscription(
            ILogger logger,
            PolymarketSocketClientClobApi client,
            string[] assetIds,
            Action<DataEvent<PolymarketPriceChangeUpdate>>? priceChangeHandler,
            Action<DataEvent<PolymarketBookUpdate>>? bookHandler,
            Action<DataEvent<PolymarketLastTradePriceUpdate>>? lastTradePriceHandler,
            Action<DataEvent<PolymarketTickSizeUpdate>>? tickSizeUpdateHandler,
            Action<DataEvent<PolymarketBestBidAskUpdate>>? bidAskUpdateHandler,
            Action<DataEvent<PolymarketNewMarketUpdate>>? newMarketUpdate,
            Action<DataEvent<PolymarketMarketResolvedUpdate>>? marketResolvedUpdate
            ) : base(logger, false)
        {
            _client = client;
            _priceChangeHandler = priceChangeHandler;
            _bookHandler = bookHandler;
            _lastTradePriceHandler = lastTradePriceHandler;
            _lastTickSizeHandler = tickSizeUpdateHandler;
            _bidAskUpdateHandler = bidAskUpdateHandler;
            _newMarketUpdate = newMarketUpdate;
            _marketResolvedUpdate = marketResolvedUpdate;
            _assetIds = assetIds;

            MessageRouter = MessageRouter.Create([
                MessageRoute<PolymarketPriceChangeUpdate>.CreateWithoutTopicFilter("price_change", DoHandleMessage),
                MessageRoute<PolymarketBookUpdate>.CreateWithoutTopicFilter("book", DoHandleMessage),
                MessageRoute<PolymarketBookUpdate[]>.CreateWithoutTopicFilter("book_snapshot", DoHandleMessage),
                MessageRoute<PolymarketLastTradePriceUpdate>.CreateWithoutTopicFilter("last_trade_price", DoHandleMessage),
                MessageRoute<PolymarketLastTradePriceUpdate>.CreateWithoutTopicFilter("tick_size_change", DoHandleMessage),
                MessageRoute<PolymarketBestBidAskUpdate>.CreateWithoutTopicFilter("best_bid_ask", DoHandleMessage),
                MessageRoute<PolymarketNewMarketUpdate>.CreateWithoutTopicFilter("new_market", DoHandleMessage),
                MessageRoute<PolymarketNewMarketUpdate>.CreateWithoutTopicFilter("market_resolved", DoHandleMessage)
                ]);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PolymarketQuery<object>("subscribe", _assetIds);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new PolymarketQuery<object>("unsubscribe", _assetIds);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketPriceChangeUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _priceChangeHandler?.Invoke(new DataEvent<PolymarketPriceChangeUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketTickSizeUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _lastTickSizeHandler?.Invoke(new DataEvent<PolymarketTickSizeUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBookUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _bookHandler?.Invoke(new DataEvent<PolymarketBookUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBookUpdate[] messages)
        {
            foreach (var message in messages)
            {
                _bookHandler?.Invoke(new DataEvent<PolymarketBookUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                            .WithUpdateType(SocketUpdateType.Snapshot)
                            .WithStreamId(message.EventType)
                            //.WithSymbol(data.Symbol)
                            .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            }

            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketLastTradePriceUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _lastTradePriceHandler?.Invoke(new DataEvent<PolymarketLastTradePriceUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBestBidAskUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _bidAskUpdateHandler?.Invoke(new DataEvent<PolymarketBestBidAskUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketNewMarketUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _newMarketUpdate?.Invoke(new DataEvent<PolymarketNewMarketUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketMarketResolvedUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _marketResolvedUpdate?.Invoke(new DataEvent<PolymarketMarketResolvedUpdate>(PolymarketExchange.ExchangeName, message, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(message.EventType)
                        //.WithSymbol(data.Symbol)
                        .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset()));
            return new CallResult(null);
        }
    }
}
