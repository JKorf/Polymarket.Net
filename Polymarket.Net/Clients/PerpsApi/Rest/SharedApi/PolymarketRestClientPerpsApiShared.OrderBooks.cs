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
        #region Get Order Book

        async Task<IExchangeCallResult<SharedOrderBook>> IGetOrderBook.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
            => await GetOrderBookAsync(request, ct).ConfigureAwait(false);

        public GetOrderBookOptions GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, [10, 100, 500, 1000], false);
        public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.ExchangeData.GetOrderBookAsync(
                id.Value,
                depth: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(SharedQuantityType.Contracts, result.Data.Sequence, result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Get Book Ticker

        async Task<IExchangeCallResult<SharedBookTicker>> IGetBookTicker.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
            => await ((IGetBookTickerRest)this).GetBookTickerAsync(request, ct).ConfigureAwait(false);

        public GetBookTickerOptions GetBookTickerOptions { get; } = new GetBookTickerOptions(_exchangeName, false)
        {
        };
        public async Task<HttpResult<SharedBookTicker>> GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var stats = await _api.ExchangeData.GetBookTickerAsync(id, ct).ConfigureAwait(false);
            if (!stats.Success)
                return HttpResult.Fail<SharedBookTicker>(stats);

            var ticker = stats.Data.SingleOrDefault(x => x.SymbolId == id);
            if (ticker == null)
                return HttpResult.Fail<SharedBookTicker>(stats, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(stats,
                new SharedBookTicker(
                    request.Symbol,
                    request.SymbolName(FormatSymbol),
                    ticker.BestAskPrice,
                    new SharedOrderQuantity(ticker.BestAskQuantity),
                    ticker.BestBidPrice,
                    new SharedOrderQuantity(ticker.BestBidQuantity)));
        }

        #endregion
    }
}
