using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PerpsApi
{
    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IPolymarketRestClientPerpsSharedApi :
        IGetBalancesRest,
        IGetDepositHistoryRest,
        IGetFeesRest,
        IGetFundingRateHistoryRest,
        IGetFundingInfoRest,
        IGetUserFundingHistoryRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetFuturesOrderByClientOrderIdRest,
        IGetOpenFuturesOrdersRest,
        IGetClosedFuturesOrdersRest,
        ICancelFuturesOrderRest,
        ICancelFuturesOrderByClientOrderIdRest,
        ICancelAllFuturesSymbolOrdersRest,
        IEditFuturesOrderRest,
        IEditFuturesOrderByClientOrderIdRest,
        IGetFuturesUserTradeHistoryRest,
        IPlaceMultipleFuturesOrdersRest,
        IGetPositionsRest,
        IGetFuturesSymbolsRest,
        IGetAllTickersRest,
        IGetTickerRest,
        IGetIndexPriceRest,
        IGetAllIndexPricesRest,
        IGetMarkPriceRest,
        IGetAllMarkPricesRest,
        IGetOpenInterestRest,
        IGetKlinesRest,
        IGetLeverageRest,
        ISetLeverageRest,
        IGetLeverageTiersRest,
        IGetOrderBookRest,
        IGetBookTickerRest,
        IGetRecentTradesRest,
        IGetWithdrawalHistoryRest
    {
    }
}
