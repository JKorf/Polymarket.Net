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
        #region Get Leverage
        public SharedLeverageSettingMode LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        async Task<IExchangeCallResult<SharedLeverage>> IGetLeverage.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
            => await GetLeverageAsync(request, ct).ConfigureAwait(false);

        public GetLeverageOptions GetLeverageOptions { get; } = new GetLeverageOptions(_exchangeName, true)
        {
        };
        public async Task<HttpResult<SharedLeverage>> GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = GetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedLeverage>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.Account.GetLeverageMarginConfigAsync(id.Value, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            if (result.Data.Length == 0)
                return HttpResult.Fail<SharedLeverage>(Exchange, new ServerError(ErrorType.Unknown, "No setting found for the given symbol"));

            return HttpResult.Ok(result, new SharedLeverage(result.Data[0].Leverage)
            {
                MarginMode = result.Data[0].Cross ? SharedMarginMode.Cross : SharedMarginMode.Isolated,
            });
        }

        #endregion

        #region Set Leverage

        async Task<IExchangeCallResult<SharedLeverage>> ISetLeverage.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
            => await SetLeverageAsync(request, ct).ConfigureAwait(false);

        public SetLeverageOptions SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName)
        {
            ExchangeParameterRules = [
                ExchangeParameterRule.Required("MarginMode", "Cross or isolated margin mode setting", SharedMarginMode.Cross)
            ]
        };
        public async Task<HttpResult<SharedLeverage>> SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedLeverage>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var marginMode = request.GetParamValue<SharedMarginMode>("MarginMode");
            var result = await _api.Account.SetLeverageAsync(
                id.Value,
                (int)request.Leverage,
                marginMode == SharedMarginMode.Cross,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            return HttpResult.Ok(result, new SharedLeverage(result.Data.Leverage));
        }

        #endregion

        #region Get Leverage Tiers

        async Task<IExchangeCallResult<SharedLeverageTier[]>> IGetLeverageTiers.GetLeverageTiersAsync(GetLeverageTiersRequest request, CancellationToken ct)
            => await GetLeverageTiersAsync(request, ct).ConfigureAwait(false);

        public GetLeverageTiersOptions GetLeverageTiersOptions { get; } = new GetLeverageTiersOptions(_exchangeName, true);
        public async Task<HttpResult<SharedLeverageTier[]>> GetLeverageTiersAsync(GetLeverageTiersRequest request, CancellationToken ct)
        {
            var validationError = GetLeverageTiersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverageTier[]>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedLeverageTier[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetSymbolsAsync(id, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverageTier[]>(result);

            var symbolInfo = result.Data.FirstOrDefault(x => x.SymbolId == id.Value);
            if (symbolInfo == null)
                return HttpResult.Fail<SharedLeverageTier[]>(Exchange, new ServerError(ErrorType.Unknown, "No symbol info found for the given symbol"));

            int index = symbolInfo.RiskTiers.Length;
            var resultData = new List<SharedLeverageTier>();
            foreach(var riskTier in symbolInfo.RiskTiers.OrderByDescending(x => x.LowerBound))
            {
                var last = resultData.LastOrDefault();
                resultData.Add(new SharedLeverageTier(
                    request.Symbol, 
                    symbolInfo.Symbol, 
                    index--,
                    null,
                    riskTier.LowerBound,
                    last?.MinNotional - 1,
                    null,
                    riskTier.MaxLeverage));
            }

            return HttpResult.Ok(result, resultData.OrderBy(x => x.Index).ToArray());
        }

        #endregion
    }
}
