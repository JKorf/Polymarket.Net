using Polymarket.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Polymarket.Net.UnitTests
{
    [TestFixture]
    public class PolymarketSharedApiRestIntegrationTest : SharedApiRestIntegrationTest
    {
        protected override bool ManualRun => false;

        [Test]
        public override async Task RunAsync()
        {
            // Symbols cache required
            var client = (IPolymarketSharedApiClient)GetClient();
            await client.PerpsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest());

            await ExecuteAsync();
        }

        protected override ISharedApiClientBase GetClient()
        {
            var collection = new ServiceCollection();
            collection.AddPolymarket();
            collection.AddLogging(x =>
            {
                var logLevel = LogLevel.Trace;
                x.SetMinimumLevel(logLevel);
                x.AddProvider(new TraceLoggerProvider());
            });
            var provider = collection.BuildServiceProvider();
            return provider.GetRequiredService<IPolymarketSharedApiClient>();
        }

        protected override SharedApiTestConfig GetConfig(ISharedRest client, CapabilityOptions options)
        {
            return new SharedApiTestConfig(new SharedSymbol(TradingMode.PerpetualLinear, "ETH", "USD"), "ETH");
        }
    }

    [TestFixture]
    public class PolymarketSharedApiSocketIntegrationTest : SharedApiSocketIntegrationTest
    {
        protected override bool ManualRun => false;

        [Test]
        public override async Task RunAsync()
        {
            // Symbols cache required
            var client = (IPolymarketSharedApiClient)GetClient();
            await client.PerpsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest());

            var result = await ExecuteAsync();
            foreach (var item in result)
                Assert.Warn(item.Message);
        }

        protected override ISharedApiClientBase GetClient()
        {
            var collection = new ServiceCollection();
            collection.AddPolymarket();
            collection.AddLogging(x =>
            {
                var logLevel = LogLevel.Trace;
                x.SetMinimumLevel(logLevel);
                x.AddProvider(new TraceLoggerProvider());
            });
            var provider = collection.BuildServiceProvider();
            return provider.GetRequiredService<IPolymarketSharedApiClient>();
        }

        protected override SharedApiTestConfig GetConfig(ISharedSocket client, CapabilityOptions options)
        {
            return new SharedApiTestConfig(new SharedSymbol(TradingMode.PerpetualLinear, "ETH", "USD"), "ETH");
        }
    }
}
