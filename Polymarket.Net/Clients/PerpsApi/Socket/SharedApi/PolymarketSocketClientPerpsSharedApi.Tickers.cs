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

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName, SharedTickerType.Day24H)
        {
            SupportsMultipleSymbols = true,
        };

        #region Subscribe To Ticker Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.SymbolNames(FormatSymbol).Select(x => PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, x));
            if (symbols.Any(x => !x.HasValue))
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.SubscribeToStatisticsUpdatesAsync(symbols.Select(x => x!.Value), update =>
            {
                var lastKline = update.Data.Klines.OrderByDescending(x => x.OpenTime).FirstOrDefault();

                handler(update.ToType(
                new SharedTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, update.Data.SymbolId) ?? string.Empty,
                    lastKline?.ClosePrice,
                    update.Data.Klines.Select(x => x.HighPrice).DefaultIfEmpty().Max(),
                    update.Data.Klines.Select(x => x.LowPrice).DefaultIfEmpty().Min(),
                    new SharedOrderQuantity(update.Data.Klines.Select(x => x.Volume).DefaultIfEmpty().Sum(), quoteAssetQuantity: update.Data.Volume),
                    update.Data.Volume > 0 && lastKline?.ClosePrice > 0 ? Math.Round(lastKline.ClosePrice / update.Data.OpenPrice * 100 - 100, 4) : null)));
            }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion


        #region Subscribe To All Ticker Updates


        public SubscribeTickersOptions SubscribeAllTickersOptions { get; } = new SubscribeTickersOptions(_exchangeName, SharedTickerType.Day24H)
        {
            SupportsMultipleSymbols = true,
        };

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickersUpdatesAsync(SubscribeAllTickersRequest request, Action<DataEvent<SharedTicker[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.ExchangeData.SubscribeToAllStatisticsUpdatesAsync(update =>
            {
                var lastKline = update.Data.Klines.OrderByDescending(x => x.OpenTime).FirstOrDefault();

                handler(update.ToType(new[] {
                new SharedTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, update.Data.SymbolId) ?? string.Empty,
                    lastKline?.ClosePrice,
                    update.Data.Klines.Select(x => x.HighPrice).DefaultIfEmpty().Max(),
                    update.Data.Klines.Select(x => x.LowPrice).DefaultIfEmpty().Min(),
                    new SharedOrderQuantity(update.Data.Klines.Select(x => x.Volume).DefaultIfEmpty().Sum(), quoteAssetQuantity: update.Data.Volume),
                    update.Data.Volume > 0 && lastKline?.ClosePrice > 0 ? Math.Round(lastKline.ClosePrice / update.Data.OpenPrice * 100 - 100, 4) : null)}
                ));
            }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
