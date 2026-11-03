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
        #region Get Index Price

        async Task<IExchangeCallResult<SharedIndexPrice>> IGetIndexPrice.GetIndexPriceAsync(GetIndexPriceRequest request, CancellationToken ct)
            => await GetIndexPriceAsync(request, ct).ConfigureAwait(false);

        public GetIndexPriceOptions GetIndexPriceOptions { get; } = new GetIndexPriceOptions(_exchangeName, false);
        public async Task<HttpResult<SharedIndexPrice>> GetIndexPriceAsync(GetIndexPriceRequest request, CancellationToken ct)
        {
            var validationError = GetIndexPriceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedIndexPrice>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedIndexPrice>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetTickersAsync(id, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedIndexPrice>(result);

            var ticker = result.Data.SingleOrDefault();
            if (ticker == null)
                return HttpResult.Fail<SharedIndexPrice>(result, new ServerError(ErrorType.UnknownSymbol, "No ticker found for symbol"));

            return HttpResult.Ok(result, new SharedIndexPrice(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, ticker.Symbol),
                    ticker.Symbol,
                    ticker.IndexPrice));
        }


        #endregion

        #region Get All Index Prices

        async Task<IExchangeCallResult<SharedIndexPrice[]>> IGetAllIndexPrices.GetAllIndexPricesAsync(GetAllIndexPricesRequest request, CancellationToken ct)
            => await GetAllIndexPricesAsync(request, ct).ConfigureAwait(false);

        public GetAllIndexPricesOptions GetAllIndexPricesOptions { get; } = new GetAllIndexPricesOptions(_exchangeName, false);
        public async Task<HttpResult<SharedIndexPrice[]>> GetAllIndexPricesAsync(GetAllIndexPricesRequest request, CancellationToken ct)
        {
            var validationError = GetAllIndexPricesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedIndexPrice[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedIndexPrice[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedIndexPrice(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.IndexPrice)
            ).ToArray());
        }

        #endregion
    }
}
