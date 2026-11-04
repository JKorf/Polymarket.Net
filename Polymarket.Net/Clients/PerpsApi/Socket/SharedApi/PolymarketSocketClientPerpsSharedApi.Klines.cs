using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Enums;
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

        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
        };

        #region Subscribe To Kline Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolNames(FormatSymbol).Select(x => PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, x));
            if (symbols.Any(x => !x.HasValue))
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.SubscribeToKlineUpdatesAsync(
                symbols.Select(x => x!.Value),
                (KlineInterval)request.Interval, 
                update =>
                {
                    foreach(var kline in update.Data)
                    {
                        handler(update.ToType(
                            new SharedKline(
                                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                                update.Symbol ?? string.Empty,
                                kline.OpenTime,
                                kline.ClosePrice,
                                kline.HighPrice,
                                kline.LowPrice,
                                kline.OpenPrice,
                                new SharedOrderQuantity(kline.Volume)
                            )
                            ));
                    }
                }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
