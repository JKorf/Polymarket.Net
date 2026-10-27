using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polymarket.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            var client = new PolymarketRestClient(opts =>
            {
                opts.AutoTimestamp = false;
            });
            var tester = new RestRequestValidator<PolymarketRestClient>(client, "Endpoints/Clob/ExchangeData", "https://clob.polymarket.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetMidpointPriceAsync("123"), "GetMidpointPrice");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetPriceHistoryAsync("123"), "GetPriceHistory", nestedJsonProperty: "history");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetBidAskSpreadsAsync(["123"]), "GetBidAskSpreads");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetOrderBookAsync("123"), "GetTokenInfo");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetOrderBooksAsync(["123"]), "GetTokenInfos");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetMarketInfoAsync("123"), "GetMarketInfo", ignoreProperties: ["r"]);
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetOpenInterestAsync(), "GetOpenInterest");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.GetEventVolumeAsync(["123"]), "GetEventVolume", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.PredictionApi.ExchangeData.LookupTokensAsync(["123"]), "LookupTokens", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new TraceLoggerProvider(LogLevel.Trace));

            var client = new PolymarketRestClient(null, loggerFactory, Options.Create(new PolymarketRestOptions
            {
                AutoTimestamp = false,
                Environment = PolymarketEnvironment.CreateCustom(
                    "UnitTest", 
                    137, 
                    "https://clob.polymarket.com",
                    "https://clob.polymarket.com", 
                    "https://data-api.polymarket.com", 
                    "wss://localhost",
                    "wss://localhost",
                    "wss://localhost",
                    "https://api.perpetuals.polymarket.com"),
                ApiCredentials = new PolymarketCredentials().WithL1(Enums.SignType.Email, "0x1212121212121212121212121212121212121212121212121212121212121212").WithL2("MTIz", "MTIz", "12")
            }));
            var tester = new RestRequestValidator<PolymarketRestClient>(client, "Endpoints/Clob/Trading", "https://clob.polymarket.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.PredictionApi.Trading.GetOrderAsync("123"), "GetOrder");
            await tester.ValidateAsync(client => client.PredictionApi.Trading.CancelOrderAsync("123"), "CancelOrder");
            await tester.ValidateAsync(client => client.PredictionApi.Trading.PlaceOrderAsync("123", Enums.OrderSide.Buy, Enums.OrderType.Limit, 5, 0.01m), "PlaceOrder");
            await tester.ValidateAsync(client => client.PredictionApi.Trading.GetUserTradesAsync("123"), "GetUserTrades");
        }

        [Test]
        public async Task ValidatePerpsExchangeDataCalls()
        {
            var client = new PolymarketRestClient(opts =>
            {
                opts.AutoTimestamp = false;
            });
            var tester = new RestRequestValidator<PolymarketRestClient>(client, "Endpoints/Perps/ExchangeData", "https://data-api.polymarket.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetSymbolsAsync(), "GetSymbols");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetExchangeInfoAsync(), "GetExchangeInfo");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetCollateralAssetsAsync(), "GetCollateralAssets");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetTickersAsync(), "GetTickers");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetStatisticsAsync(), "GetStatistics");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetExchangeStatisticsAsync(DateTime.UtcNow, DateTime.UtcNow), "GetExchangeStatistics");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetKlinesAsync(123, KlineInterval.OneMinute, DateTime.UtcNow), "GetKlines");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetMarkPriceHistoryAsync(123, KlineInterval.OneMinute, DateTime.UtcNow), "GetMarkPriceHistory");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetBookTickerAsync(123), "GetBookTicker");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetOrderBookAsync(123), "GetOrderBook");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetIndexAsync("123"), "GetIndex");
            await tester.ValidateAsync(client => client.PerpsApi.ExchangeData.GetRecentTradesAsync(123), "GetRecentTrades");
        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Contains("POLY_SIGNATURE") == true;
        }
    }
}
