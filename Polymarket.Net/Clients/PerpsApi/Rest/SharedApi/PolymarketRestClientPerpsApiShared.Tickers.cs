using CryptoExchange.Net.SharedApis;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects.Errors;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketRestClientPerpsSharedApi
    {
        #region Get Ticker

        async Task<IExchangeCallResult<SharedTicker>> IGetTicker.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await ((IGetTickerRest)this).GetTickerAsync(request, ct).ConfigureAwait(false);

        public GetTickerOptions GetTickerOptions { get; } = new GetTickerOptions(_exchangeName)
        {
        };
        public async Task<HttpResult<SharedTicker>> GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTicker>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedTicker>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var stats = await _api.ExchangeData.GetStatisticsAsync(id, ct).ConfigureAwait(false);
            if (!stats.Success)
                return HttpResult.Fail<SharedTicker>(stats);

            var stat = stats.Data.SingleOrDefault(x => x.SymbolId == id);
            if (stat == null)
                return HttpResult.Fail<SharedTicker>(stats, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            // klines are 24 one hour candles
            var lastKline = stat.Klines.OrderByDescending(x => x.OpenTime).FirstOrDefault();
            return HttpResult.Ok(stats,
                new SharedTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, stat.Symbol),
                    stat.Symbol,
                    lastKline?.ClosePrice,
                    stat.Klines.Select(x => x.HighPrice).DefaultIfEmpty().Max(),
                    stat.Klines.Select(x => x.LowPrice).DefaultIfEmpty().Min(),
                    new SharedOrderQuantity(stat.Klines.Select(x => x.Volume).DefaultIfEmpty().Sum(), quoteAssetQuantity: stat.Volume),
                    stat.Volume > 0 && lastKline?.ClosePrice > 0 ? Math.Round(lastKline.ClosePrice / stat.OpenPrice * 100 - 100, 4) : null));
        }

        #endregion

        #region Get All Tickers

        async Task<IExchangeCallResult<SharedTicker[]>> IGetAllTickers.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await ((IGetAllTickersRest)this).GetAllTickersAsync(request, ct).ConfigureAwait(false);

        public GetAllTickersOptions GetAllTickersOptions { get; } = new GetAllTickersOptions(_exchangeName)
        {
        };
        public async Task<HttpResult<SharedTicker[]>> GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTicker[]>(Exchange, validationError);

            var stats = await _api.ExchangeData.GetStatisticsAsync(ct: ct).ConfigureAwait(false);
            if (!stats.Success)
                return HttpResult.Fail<SharedTicker[]>(stats);

            // klines are 24 one hour candles
            return HttpResult.Ok<SharedTicker[]>(stats, stats.Data.Select(x =>
            {
                var lastKline = x.Klines.OrderByDescending(x => x.OpenTime).FirstOrDefault();
                return new SharedTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    lastKline?.ClosePrice,
                    x.Klines.Select(x => x.HighPrice).DefaultIfEmpty().Max(),
                    x.Klines.Select(x => x.LowPrice).DefaultIfEmpty().Min(),
                    new SharedOrderQuantity(x.Klines.Select(x => x.Volume).DefaultIfEmpty().Sum(), quoteAssetQuantity: x.Volume),
                    x.Volume > 0 && lastKline?.ClosePrice > 0 ? Math.Round(lastKline.ClosePrice / x.OpenPrice * 100 - 100, 4) : null);
            }).ToArray()!);
        }

        #endregion
    }
}
