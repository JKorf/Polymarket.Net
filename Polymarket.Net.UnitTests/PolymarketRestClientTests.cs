using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Polymarket.Net.Clients;
using Polymarket.Net.Objects;
using CryptoExchange.Net.Objects;

namespace Polymarket.Net.UnitTests
{
    [TestFixture()]
    public class PolymarketRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var chainId = 80002;
            var privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
            var address = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";


            var authProvider = new PolymarketAuthenticationProvider(new PolymarketCredentials(address, privateKey));

            var parameters = new ParameterCollection
            {
                { "salt", "479249096354" },
                { "maker", address },
                { "signer", address },
                { "taker", "0x0000000000000000000000000000000000000000" },
                { "tokenId", "1234" },
                { "makerAmount", "100000000" },
                { "takerAmount", "50000000" },
                { "expiration", "0" },
                { "nonce", "0" },
                { "feeRateBps", "100" },
                { "side", "BUY" },
                { "signatureType", 0 },
            };

            var result = authProvider.GetOrderSignature(parameters, 80002, false).ToLower();
            Assert.That(result, Is.EqualTo("0x302cd9abd0b5fcaa202a344437ec0b6660da984e24ae9ad915a592a90facf5a51bb8a873cd8d270f070217fea1986531d5eec66f1162a81f66e026db653bf7ce1c"));
        }

        [Test]
        public void CheckSignatureExample2()
        {
            var chainId = 137;
            //var privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
            var privateKey = "";
            var address = "";


            var authProvider = new PolymarketAuthenticationProvider(new PolymarketCredentials(address, privateKey));

            var parameters = new ParameterCollection
            {
                { "salt", "1515433236867" },
                { "maker", address },
                { "signer", address },
                { "taker", "0x0000000000000000000000000000000000000000" },
                { "tokenId", "11862165566757345985240476164489718219056735011698825377388402888080786399275" },
                { "makerAmount", "5000" },
                { "takerAmount", "5000000" },
                { "expiration", "0" },
                { "nonce", "0" },
                { "feeRateBps", "0" },
                { "side", "BUY" },
                { "signatureType", 1 },
            };

            var result = authProvider.GetOrderSignature(parameters, chainId, true).ToLower();
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<PolymarketRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<PolymarketSocketClient>();
        }
    }
}
