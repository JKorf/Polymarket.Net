using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;

namespace Polymarket.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class PolymarketPerpsSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly IPolymarketSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;
        private readonly long _symbolId;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbolId">The symbol ID the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public PolymarketPerpsSymbolOrderBook(long symbolId, Action<PolymarketOrderBookOptions>? optionsDelegate = null)
            : this(symbolId, optionsDelegate, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbolId">The symbol ID</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="socketClient">Socket client instance</param>
        public PolymarketPerpsSymbolOrderBook(
            long symbolId,
            Action<PolymarketOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            IPolymarketSocketClient? socketClient) : base(logger, "Polymarket", "Perps", symbolId.ToString())
        {
            var options = PolymarketOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _symbolId = symbolId;

            _strictLevels = false;

            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new PolymarketSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var result = await _socketClient.PerpsApi.ExchangeData.SubscribeToOrderBookUpdatesAsync(_symbolId, ProcessUpdate).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<UpdateSubscription>(result.Error);

            if (ct.IsCancellationRequested)
            {
                await result.Data.CloseAsync().ConfigureAwait(false);
                return CallResult.Fail<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            if (!setResult.Success)
                await result.Data.CloseAsync().ConfigureAwait(false);

            return setResult.Success ? CallResult.Ok(result.Data) : CallResult.Fail<UpdateSubscription>(setResult.Error!);
        }

        private void ProcessUpdate(DataEvent<PolymarketPerpBookUpdate> @event)
        {
            SetSnapshot(@event.SequenceNumber, @event.Data.Bids, @event.Data.Asks, @event.DataTime, @event.DataTimeLocal);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
                _socketClient?.Dispose();            

            base.Dispose(disposing);
        }
    }
}
