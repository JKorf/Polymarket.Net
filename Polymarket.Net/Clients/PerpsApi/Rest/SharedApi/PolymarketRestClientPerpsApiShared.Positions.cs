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
        #region Get All Positions

        async Task<IExchangeCallResult<SharedPosition[]>> IGetPositions.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
            => await ((IGetPositionsRest)this).GetPositionsAsync(request, ct).ConfigureAwait(false);

        public GetPositionsOptions GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        public async Task<HttpResult<SharedPosition[]>> GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            var result = await _api.Account.GetPortfolioAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            return HttpResult.Ok(result, result.Data.Positions.Select(x =>
                new SharedPosition(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    new SharedOrderQuantity(Math.Abs(x.Quantity)),
                    null
                    )
                {
                    AverageOpenPrice = x.EntryPrice,
                    Leverage = x.Leverage,
                    LiquidationPrice = x.LiquidationPrice,
                    UnrealizedPnl = x.UnrealizedPnl,
                    PositionSide = x.Quantity > 0 ? SharedPositionSide.Long : SharedPositionSide.Short
                }
            ).ToArray());
        }

        #endregion
    }
}
