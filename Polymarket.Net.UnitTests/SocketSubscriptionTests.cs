using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Polymarket.Net.Clients;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects;

namespace Polymarket.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateClobSubscriptions()
        {
            var client = new PolymarketSocketClient(opts =>
            {
                opts.ApiCredentials = new PolymarketCredentials().WithL1(Enums.SignType.EOA, "0x1212121212121212121212121212121212121212121212121212121212121212", "1").WithL2("MTIz", "3", "123");
            });
            var tester = new SocketSubscriptionValidator<PolymarketSocketClient>(client, "Subscriptions/Clob", "wss://ws-subscriptions-clob.polymarket.com");
            await tester.ValidateAsync<PolymarketOrderUpdate>((client, handler) => client.ClobApi.SubscribeToUserUpdatesAsync(handler, null), "OrderUpdate", ignoreProperties: ["type"]);
            await tester.ValidateAsync<PolymarketTradeUpdate>((client, handler) => client.ClobApi.SubscribeToUserUpdatesAsync(null, handler), "TradeUpdate", ignoreProperties: ["type"]);
        }

        [Test]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new PolymarketSocketClient(Options.Create(new PolymarketSocketOptions
            {
                ApiCredentials = new PolymarketCredentials().WithL1(Enums.SignType.EOA, "0x1212121212121212121212121212121212121212121212121212121212121212"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<PolymarketSocketClient>(client, "Subscriptions/Clob", "wss://ws-subscriptions-clob.polymarket.com", "data");
            await tester.ValidateConcurrentAsync<PolymarketPriceChangeUpdate>(
                (client, handler) => client.ClobApi.SubscribeToTokenUpdatesAsync(["0x123"], handler),
                (client, handler) => client.ClobApi.SubscribeToTokenUpdatesAsync(["0x456"], handler),
                "Concurrent");
        }
    }
}
