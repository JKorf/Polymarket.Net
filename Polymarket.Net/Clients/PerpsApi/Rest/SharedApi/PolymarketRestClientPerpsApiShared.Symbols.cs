using CryptoExchange.Net.SharedApis;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects.Errors;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketRestClientPerpsSharedApi
    {
        #region Get Futures Symbols

        async Task<IExchangeCallResult<SharedFuturesSymbol[]>> IGetFuturesSymbols.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetFuturesSymbolsAsync(request, ct).ConfigureAwait(false);

        public SharedSymbolCatalog? FuturesSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);
        public GetFuturesSymbolsOptions GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        public async Task<HttpResult<SharedFuturesSymbol[]>> GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);

            var data = result.Data
               .Select(x => ParseSymbol(x))
               .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, data);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(data, request));
        }

        private SharedFuturesSymbol ParseSymbol(PolymarketSymbol s)
        {
            var result = new SharedFuturesSymbol(
                TradingMode.PerpetualLinear,
                s.BaseAsset,
                s.QuoteAsset,
                s.Symbol,
                true)
            {
                MaxLongLeverage = s.MaxLeverage,
                MaxShortLeverage = s.MaxLeverage,
                ContractSize = 1,
                DisplayName = s.DisplaySymbol ?? s.Symbol,
                PriceDecimals = s.PriceDecimals,
                QuantityDecimals = s.QuantityDecimals,
                MinNotionalValue = s.MinNotional,
                QuoteAssetType = SharedAssetType.Crypto,
                QuoteAssetSubType = SharedAssetSubType.StableCoin
            };

            if (s.Category == Enums.Category.Commodity)
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else if (s.Category == Enums.Category.PreIpo
                || s.Category == Enums.Category.Equity
                || s.Category == Enums.Category.Index)
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
            }

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
        #endregion
    }
}
