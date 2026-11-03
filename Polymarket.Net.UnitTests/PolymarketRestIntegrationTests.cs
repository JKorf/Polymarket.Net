using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Testing;
using CryptoExchange.Net.Web3;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polymarket.Net.UnitTests
{
    [NonParallelizable]
    public class PolymarketRestIntegrationTests : RestIntegrationTest<PolymarketRestClient>
    {
        public override bool Run { get; set; } = false;

        public override PolymarketRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new PolymarketRestClient(null, loggerFactory, Options.Create(new PolymarketRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new PolymarketCredentials().WithL1(Enums.SignType.EOA, new EthKey(privateKey: key), sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().PredictionApi.ExchangeData.GetMarketByIdAsync("123", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
        }

        [Test]
        public async Task TestClobExchangeData()
        {
            var market = await CreateClient().PredictionApi.ExchangeData.GetMarketsAsync(closed: false, volumeMin: 1);
            var marketId = market.Data.First().MarketId;
            var tokenId = market.Data.First().ClobTokenIds.First();

            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetGeographicRestrictionsAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSamplingSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSamplingMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMarketsAsync(default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMarketByIdAsync(marketId, default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPriceAsync(tokenId, Enums.OrderSide.Buy, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPricesAsync(new Dictionary<string, Enums.OrderSide> { { "tokenId", Enums.OrderSide.Buy } }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMidpointPriceAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMidpointPricesAsync(new[] { tokenId }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPriceHistoryAsync(marketId, default, default, Enums.DataInterval.OneDay, default, default), false, "history");
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetBidAskSpreadsAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetBidAskSpreadsAsync(new string[] { tokenId }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetOrderBookAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetOrderBooksAsync(new[] { tokenId }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetTickSizeAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetNegativeRiskAsyncAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetFeeRateBpsAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetLastTradePriceAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetLastTradePricesAsync(new[] { tokenId }, default), false);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestDataApi() {
            var user = "0x0000000000000000000000000000000000000000";

            await RunAndCheckResult(client => client.PredictionApi.Trading.GetPositionsAsync(user, null, null, null, null, null, null, null, null, null, null, null, null, null, default), false);
        }
    }
}
