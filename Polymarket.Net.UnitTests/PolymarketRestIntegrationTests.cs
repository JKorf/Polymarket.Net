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
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Polymarket.Net.UnitTests
{
    [NonParallelizable]
    public class PolymarketRestIntegrationTests : RestIntegrationTest<PolymarketRestClient>
    {
        public override bool Run { get; set; } = true;

        public override PolymarketRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new PolymarketRestClient(null, loggerFactory, Options.Create(new PolymarketRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                //ApiCredentials = Authenticated ? new PolymarketCredentials()
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().PredictionApi.ExchangeData.GetMarketByIdAsync(-1, default);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task TestPredictionAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.PredictionApi.Account.GetRateLimitTierAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PredictionApi.Account.GetClosedOnlyModeAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PredictionApi.Account.GetApiKeysAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PredictionApi.Account.GetNotificationsAsync(default), true);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestPredictionExchangeData()
        {
            var marketResult = await CreateClient().PredictionApi.ExchangeData.GetMarketsAsync(closed: false, volumeMin: 1);
            var market = marketResult.Data.First();
            var tokenId = marketResult.Data.First().ClobTokenIds.First();

            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetGeographicRestrictionsAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSamplingSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSamplingMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetSimplifiedMarketsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMarketsAsync(default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default), false,
                ignoreProperties: ["createdAt", "creationDate", "negRiskRequestID", "eventMetadata", "clobRewards"]);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMarketByIdAsync(market.Id, default, default), false,
                ignoreProperties: ["$schema", "clobRewards", "negRiskRequestID"]);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPriceAsync(tokenId, Enums.OrderSide.Buy, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPricesAsync(new Dictionary<string, Enums.OrderSide> { { "tokenId", Enums.OrderSide.Buy } }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMidpointPriceAsync(tokenId, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetMidpointPricesAsync(new[] { tokenId }, default), false);
            await RunAndCheckResult(warnings, client => client.PredictionApi.ExchangeData.GetPriceHistoryAsync(market.MarketId, default, default, Enums.DataInterval.OneDay, default, default), false, "history");
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
        public async Task TestPredictionTrading()
        {
            var marketResult = await CreateClient().PredictionApi.ExchangeData.GetMarketsAsync(closed: false, volumeMin: 1);
            var market = marketResult.Data.First();

            var warnings = new List<Exception>();
            await RunAndCheckResult(client => client.PredictionApi.Trading.GetPositionsAsync(default, market.MarketId, null, null, null, null, null, null, null, null, null, null, null, null, default), false);
            await RunAndCheckResult(client => client.PredictionApi.Trading.GetOpenOrdersAsync(default, default, default, default, default), true);
            await RunAndCheckResult(client => client.PredictionApi.Trading.GetUserTradesAsync(default, default, default, default, default, default, default, default), true);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestPerpsAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetCredentialsAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetBalancesAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetPortfolioAsync(default), true,
                ignoreProperties: ["coverage"]);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetEquityHistoryAsync(KlineInterval.FifteenMinutes, DateTime.UtcNow.AddDays(-1), default, default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetPnlHistoryAsync(KlineInterval.OneHour, DateTime.UtcNow.AddDays(-1), default, default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetFundingHistoryAsync(default, default, default, default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetDepositsAsync(default, default, default, default, default), true,
                ignoreProperties: ["log_index"]);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetInternalTransfersAsync(default, default, default, default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetWithdrawalsAsync(default, default, default, default, default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetAccountStatsAsync(default), true);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetAccountLimitsAsync(default), true,
                ignoreProperties: ["tier"]);
            await RunAndCheckResult(warnings, client => client.PerpsApi.Account.GetLeverageMarginConfigAsync(default, default), true);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestPerpsExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetExchangeInfoAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetCollateralAssetsAsync(default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetSymbolsAsync(default, default, default, default), false,
                ignoreProperties: ["fee_override", "funding_multiplier"]);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetTickersAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetStatisticsAsync(default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetExchangeStatisticsAsync(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetKlinesAsync(7, KlineInterval.FifteenMinutes, DateTime.UtcNow.AddDays(-1), default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetMarkPriceHistoryAsync(7, KlineInterval.FifteenMinutes, DateTime.UtcNow.AddDays(-1), default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetBookTickerAsync(7, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetOrderBookAsync(7, default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetIndexAsync("ETH", default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetRecentTradesAsync(7, default, default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetFundingRateHistoryAsync(7, default, default, default), false);
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetFeesAsync(default), false, "fee_schedule");
            await RunAndCheckResult(warnings, client => client.PerpsApi.ExchangeData.GetLimitTiersAsync(default), false);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }
    }
}
