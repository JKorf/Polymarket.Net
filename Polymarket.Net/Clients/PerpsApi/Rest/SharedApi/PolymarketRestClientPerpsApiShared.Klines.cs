using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketRestClientPerpsSharedApi
    {

        #region Get Klines

        async Task<IExchangeCallResult<SharedKline[]>> IGetKlines.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetKlinesAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetKlinesOptions GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 2000, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.SixHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek)
        {
        };

        public async Task<HttpResult<SharedKline[]>> GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;

            var validationError = GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 1000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedKline[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetKlinesAsync(
                id.Value,
                interval,
                startTime: pageParams.StartTime ?? DateTime.UtcNow.AddSeconds(-(int)request.Interval * 100),
                endTime: pageParams.EndTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => !result.Data.More ? null : Pagination.NextPageFromTime(pageParams, result.Data.Data.Min(x => x.OpenTime)),
                     result.Data.Data.Length,
                     result.Data.Data.Select(x => x.OpenTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedKline(
                            request.Symbol,
                            request.Symbol!.GetSymbol(FormatSymbol),
                            x.OpenTime,
                            x.ClosePrice,
                            x.HighPrice,
                            x.LowPrice,
                            x.OpenPrice,
                            new SharedOrderQuantity(x.Volume)))
                    .ToArray(), nextPageRequest);
        }

        #endregion
    }
}
