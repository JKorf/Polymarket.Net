using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketSocketClientPerpsSharedApi
    {

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
        };

        #region Subscribe To Trade Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolNames(FormatSymbol).Select(x => PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, x));
            if (symbols.Any(x => !x.HasValue))
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.SubscribeToTradeUpdatesAsync(symbols.Select(x => x!.Value), update => handler(update.ToType(update.Data.Select(x =>
                new SharedTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, x.SymbolId) ?? string.Empty,
                    new SharedOrderQuantity(Math.Abs(x.Quantity)),
                    x.Price,
                    x.Timestamp)
                {
                    Side = x.Side == Enums.PositionSide.Long ? SharedOrderSide.Buy : SharedOrderSide.Sell
                }).ToArray())), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
