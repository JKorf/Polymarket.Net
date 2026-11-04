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
        #region Get Funding Info

        async Task<IExchangeCallResult<SharedFundingInfo>> IGetFundingInfo.GetFundingInfoAsync(GetFundingInfoRequest request, CancellationToken ct)
            => await GetFundingInfoAsync(request, ct).ConfigureAwait(false);

        public GetFundingInfoOptions GetFundingInfoOptions { get; } = new GetFundingInfoOptions(_exchangeName, false, false, false, 0, false);

        public async Task<HttpResult<SharedFundingInfo>> GetFundingInfoAsync(GetFundingInfoRequest request, CancellationToken ct)
        {
            var validationError = GetFundingInfoOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingInfo>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedFundingInfo>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            // Get data
            var result = await _api.ExchangeData.GetTickersAsync(
                id.Value,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingInfo>(result);

            var fundingInfo = result.Data.SingleOrDefault();
            if (fundingInfo == null)
                return HttpResult.Fail<SharedFundingInfo>(Exchange, new ServerError(ErrorType.UnknownSymbol, "No funding info returned"));

            // Return
            return HttpResult.Ok(result, new SharedFundingInfo(
                fundingInfo.FundingRate,
                fundingInfo.NextFunding,
                null));
        }

        #endregion

        #region Get Funding Rate History

        async Task<IExchangeCallResult<SharedFundingRate[]>> IGetFundingRateHistory.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFundingRateHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetFundingRateHistoryOptions GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(_exchangeName, false, true, true, 100, false);

        public async Task<HttpResult<SharedFundingRate[]>> GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFundingRateHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingRate[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedFundingRate[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            // Get data
            var result = await _api.ExchangeData.GetFundingRateHistoryAsync(
                id.Value,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingRate[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => !result.Data.More ? null : Pagination.NextPageFromTime(pageParams, result.Data.Data.Min(x => x.Timestamp)),
                     result.Data.Data.Length,
                     result.Data.Data.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedFundingRate(x.FundingRate, x.Timestamp))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Get User Funding History

        async Task<IExchangeCallResult<SharedFundingFee[]>> IGetUserFundingHistory.GetUserFundingHistoryAsync(GetUserFundingHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetUserFundingHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetUserFundingHistoryOptions GetUserFundingHistoryOptions { get; } = new GetUserFundingHistoryOptions(_exchangeName, false, true, true, 100, false);

        public async Task<HttpResult<SharedFundingFee[]>> GetUserFundingHistoryAsync(GetUserFundingHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetUserFundingHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingFee[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var id = request.Symbol == null ? null : _api.GetSymbolIdByName(request.Symbol.GetSymbol(FormatSymbol));

            // Get data
            var result = await _api.Account.GetFundingHistoryAsync(
                id,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingFee[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => !result.Data.More ? null : Pagination.NextPageFromTime(pageParams, result.Data.Data.Min(x => x.Timestamp)),
                     result.Data.Data.Length,
                     result.Data.Data.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedFundingFee(_api.GetSymbolNameById(x.SymbolId) ?? string.Empty, x.Funding, x.Timestamp))
                    .ToArray(), nextPageRequest);
        }

        #endregion
    }
}
