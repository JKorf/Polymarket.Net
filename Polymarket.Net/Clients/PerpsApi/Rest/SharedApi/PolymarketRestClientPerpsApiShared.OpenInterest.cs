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
        #region Get Open Interest

        async Task<IExchangeCallResult<SharedOpenInterest>> IGetOpenInterest.GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
            => await GetOpenInterestAsync(request, ct).ConfigureAwait(false);

        public GetOpenInterestOptions GetOpenInterestOptions { get; } = new GetOpenInterestOptions(_exchangeName, false);
        public async Task<HttpResult<SharedOpenInterest>> GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
        {
            var validationError = GetOpenInterestOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOpenInterest>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedOpenInterest>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetTickersAsync(id, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOpenInterest>(result);

            var ticker = result.Data.SingleOrDefault();
            if (ticker == null)
                return HttpResult.Fail<SharedOpenInterest>(result, new ServerError(ErrorType.UnknownSymbol, "No ticker found for symbol"));

            return HttpResult.Ok(result, new SharedOpenInterest(new SharedOrderQuantity(ticker.OpenInterest)));
        }


        #endregion

    }
}
