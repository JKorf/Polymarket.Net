
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Clients.PerpsApi;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketSocketClientPerpsSharedApi :
        SharedApiBase,
        IPolymarketSocketClientPerpsSharedApi

    {
        private readonly PolymarketSocketClientPerpsApi _api;

        private const string _topicId = "PolymarketPerps";
        private const string _exchangeName = "Polymarket";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(PolymarketPlatform.Metadata, this);

        public PolymarketSocketClientPerpsSharedApi(PolymarketSocketClientPerpsApi api)
           : base(
                 SharedTransport.Socket,
                 api,
                 [TradingMode.PerpetualLinear, TradingMode.PerpetualInverse],
                 () => api.Authenticated,
                 api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeOrderBookOptions,
                SubscribeKlineOptions,
                SubscribeTickerOptions,
                SubscribeAllTickersOptions,
                SubscribeBalanceOptions,
                SubscribeUserTradeOptions,
                SubscribeFuturesOrderOptions,
                SubscribePositionOptions,
                PlaceFuturesOrderOptions,
                PlaceMultipleFuturesOrdersOptions,
                CancelFuturesOrderOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                CancelAllFuturesSymbolOrdersOptions,
                EditFuturesOrderOptions,
                EditFuturesOrderByClientOrderIdOptions
                );
        }

        /// <inheritdoc />
        public Task UnsubscribeAllAsync() => _api.UnsubscribeAllAsync();
    }
}
