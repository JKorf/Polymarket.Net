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
        #region Get Balances

        async Task<IExchangeCallResult<SharedBalance[]>> IGetBalances.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
            => await GetBalancesAsync(request, ct).ConfigureAwait(false);

        public GetBalancesOptions GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Futures);

        public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await _api.Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedBalance(
                    SupportedTradingModes,
                    x.Asset,
                    x.Balance,
                    x.Balance)).ToArray());
        }

        #endregion
    }
}
