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
        #region Get Fees
        public GetFeeOptions GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);


        async Task<IExchangeCallResult<SharedFee>> IGetFees.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
            => await GetFeesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFee>> GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var result = await _api.ExchangeData.GetFeesAsync(
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            var data = result.Data.FirstOrDefault();
            if (data == null)
                return HttpResult.Fail<SharedFee>(result, new ServerError(ErrorType.Unknown, "No fee info"));

            // Return
            return HttpResult.Ok(result, new SharedFee(data.MakerFeeRate * 100, data.TakerFeeRate * 100));
        }

        #endregion
    }
}
