using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polymarket.Net.Clients;
using Polymarket.Net.Objects;

namespace Polymarket.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataAccountCalls()
        {
            var client = new PolymarketRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new PolymarketCredentials("123", "456");
            });
            var tester = new RestRequestValidator<PolymarketRestClient>(client, "Endpoints/Clob/ExchangeData", "XXX", IsAuthenticated);
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetMidpointPriceAsync("123"), "GetMidpointPrice");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetPriceHistoryAsync("123"), "GetPriceHistory", nestedJsonProperty: "history");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(["123"]), "GetBidAskSpreads");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetOrderBookAsync("123"), "GetTokenInfo");
            await tester.ValidateAsync(client => client.ClobApi.ExchangeData.GetOrderBooksAsync(["123"]), "GetTokenInfos");

        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
