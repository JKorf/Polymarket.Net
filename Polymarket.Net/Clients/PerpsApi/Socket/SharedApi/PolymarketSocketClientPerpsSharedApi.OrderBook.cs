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

        public SubscribeOrderBookOptions SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, [20, 50])
        {
            SupportsMultipleSymbols = true,
        };

        #region Subscribe To OrderBook Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolNames(FormatSymbol).Select(x => PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, x));
            if (symbols.Any(x => !x.HasValue))
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.SubscribeToOrderBookUpdatesAsync(symbols.Select(x => x!.Value), update => handler(update.ToType(
                new SharedOrderBook(
                    SharedQuantityType.BaseAsset,
                    null,
                    update.Data.Asks,
                    update.Data.Bids
                    ))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
