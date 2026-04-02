using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polymarket.Net.Clients;
using Polymarket.Net.Objects;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Objects.Options;
using Microsoft.Extensions.Options;

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
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetMidpointPriceAsync("123"), "GetMidpointPrice");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetPriceHistoryAsync("123"), "GetPriceHistory", nestedJsonProperty: "history");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(["123"]), "GetBidAskSpreads");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetOrderBookAsync("123"), "GetTokenInfo");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetOrderBooksAsync(["123"]), "GetTokenInfos");
        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new TraceLoggerProvider(LogLevel.Trace));

            var client = new PolymarketRestClient(null, loggerFactory, Options.Create(new PolymarketRestOptions
            {
                AutoTimestamp = false,
                Environment = PolymarketEnvironment.CreateCustom("UnitTest", 137, "https://clob.polymarket.com", "https://clob.polymarket.com", "https://data-api.polymarket.com", "wss://localhost", "wss://localhost"),
                ApiCredentials = new PolymarketCredentials().WithL1(Enums.SignType.Email, "0x1212121212121212121212121212121212121212121212121212121212121212").WithL2("MTIz", "MTIz", "12")
            }));
            var tester = new RestRequestValidator<PolymarketRestClient>(client, "Endpoints/Clob/Trading", "https://clob.polymarket.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.ClobApi.Trading.GetOrderAsync("123"), "GetOrder");
            await tester.ValidateAsync(client => client.ClobApi.Trading.CancelOrderAsync("123"), "CancelOrder");
            await tester.ValidateAsync(client => client.ClobApi.Trading.PlaceOrderAsync("123", Enums.OrderSide.Buy, Enums.OrderType.Limit, 5, 0.01m), "PlaceOrder");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders?.Contains("POLY_SIGNATURE") == true;
        }
    }
}
