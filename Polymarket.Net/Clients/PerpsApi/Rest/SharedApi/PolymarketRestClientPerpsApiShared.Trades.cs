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
        #region Get Recent Trades

        async Task<IExchangeCallResult<SharedTrade[]>> IGetRecentTrades.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
            => await GetRecentTradesAsync(request, ct).ConfigureAwait(false);

        public GetRecentTradesOptions GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 100, false);
        public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetRecentTradesAsync(
                id.Value,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Data.Select(x =>
                new SharedTrade(request.Symbol, request.SymbolName(FormatSymbol), new SharedOrderQuantity(contractQuantity: Math.Abs(x.Quantity)), x.Price, x.Timestamp)).ToArray());
        }

        #endregion
    }
}
