using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Interfaces.Clients.PerpsApi;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketRestClientPerpsSharedApi : 
        SharedApiBase,
        IPolymarketRestClientPerpsSharedApi
    {
        private readonly PolymarketRestClientPerpsApi _api;

        private const string _exchangeName = "Polymarket";
        private const string _topicId = "PolymarketPerps";
        
        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(PolymarketPlatform.Metadata, this);

        public PolymarketRestClientPerpsSharedApi(PolymarketRestClientPerpsApi api)
            : base(
                  SharedTransport.Rest,
                  api,
                  [TradingMode.PerpetualLinear, TradingMode.PerpetualInverse],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBalancesOptions,
                GetDepositHistoryOptions,
                GetFeeOptions,
                GetFundingRateHistoryOptions,
                GetFundingInfoOptions,
                PlaceFuturesOrderOptions,
                PlaceMultipleFuturesOrdersOptions,
                GetFuturesOrderOptions,
                GetFuturesOrderByClientOrderIdOptions,
                GetOpenFuturesOrdersOptions,
                GetClosedFuturesOrdersOptions,
                CancelFuturesOrderOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                CancelAllFuturesSymbolOrdersOptions,
                EditFuturesOrderOptions,
                EditFuturesOrderByClientOrderIdOptions,
                GetFuturesUserTradeHistoryOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetMarkPriceOptions,
                GetAllMarkPricesOptions,
                GetIndexPriceOptions,
                GetAllIndexPricesOptions,
                GetOpenInterestOptions,
                GetKlinesOptions,
                GetLeverageOptions,
                SetLeverageOptions,
                GetLeverageTiersOptions,
                GetOrderBookOptions,
                GetBookTickerOptions,
                GetRecentTradesOptions,
                GetWithdrawalHistoryOptions,
                GetFuturesSymbolsOptions,
                GetPositionsOptions,
                GetUserFundingHistoryOptions
                );
        }
    }
}
