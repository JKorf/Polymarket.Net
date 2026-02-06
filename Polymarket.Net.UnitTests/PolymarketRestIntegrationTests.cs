using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Polymarket.Net.Clients;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.Collections.Generic;
using System.Linq;

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
                ApiCredentials = Authenticated ? new PolymarketCredentials(Enums.SignType.EOA, key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().ClobApi.ExchangeData.GetMarketAsync("123", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
        }

        [Test]
        public async Task TestClobExchangeData()
        {
            var market = await CreateClient().GammaApi.GetMarketsAsync(closed: false, volumeMin: 1);
            var marketId = market.Data.First().ConditionId;
            var tokenId = market.Data.First().ClobTokenIds.First();

            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetGeographicRestrictionsAsync(default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetSamplingSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetSamplingMarketsAsync(default, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetMarketsAsync(default, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetMarketAsync(marketId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetPriceAsync(tokenId, Enums.OrderSide.Buy, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetPricesAsync(new Dictionary<string, Enums.OrderSide> { { "tokenId", Enums.OrderSide.Buy } }, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetMidpointPriceAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetMidpointPricesAsync(new[] { tokenId }, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetPriceHistoryAsync(marketId, default, default, Enums.DataInterval.OneDay, default, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(new string[] { tokenId }, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetOrderBookAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetOrderBooksAsync(new[] { tokenId }, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetTickSizeAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetNegativeRiskAsyncAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetFeeRateBpsAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetLastTradePriceAsync(tokenId, default), false);
            await RunAndCheckResult(client => client.ClobApi.ExchangeData.GetLastTradePricesAsync(new[] { tokenId }, default), false);
        }
    }
}
