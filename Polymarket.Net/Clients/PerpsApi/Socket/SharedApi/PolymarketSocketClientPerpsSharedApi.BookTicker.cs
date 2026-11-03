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

        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
        };

        #region Subscribe To BookTicker Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolNames(FormatSymbol).Select(x => PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, x));
            if (symbols.Any(x => !x.HasValue))
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.SubscribeToBookTickerUpdatesAsync(symbols.Select(x => x!.Value), update => handler(update.ToType(
                new SharedBookTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, update.Data.SymbolId) ?? string.Empty,
                    update.Data.BestAskPrice,
                    new SharedOrderQuantity(update.Data.BestAskQuantity),
                    update.Data.BestBidPrice,
                    new SharedOrderQuantity(update.Data.BestBidQuantity)))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
