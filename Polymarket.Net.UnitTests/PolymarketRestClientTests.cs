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
            uint chainId = 80002;
            var privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
            var address = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";


            var authProvider = new PolymarketAuthenticationProvider(new PolymarketCredentials().WithL1(Enums.SignType.EOA, privateKey, address));

            var parameters = new ParameterCollection
            {
                { "salt", "479249096354" },
                { "maker", address },
                { "signer", address },
                { "tokenId", "1234" },
                { "makerAmount", "100000000" },
                { "takerAmount", "50000000" },
                { "expiration", "0" },
                { "side", "BUY" },
                { "signatureType", 0 },
                { "timestamp", "1768900397000" },
                { "metadata", "0x0000000000000000000000000000000000000000000000000000000000000000" },
                { "builder", "0x0000000000000000000000000000000000000000000000000000000000000000" },
            };

            var result = authProvider.GetOrderSignature(parameters, chainId, false).ToLower();
            Assert.That(result, Is.EqualTo("0xb426c5b304df77bd5bcb888ba8600c7c1e02e7af99b3b88298f4406b053de3d70dd86371cc345a7b258309b49635b4d65333ff8fc970ee72e46ea1298fdaa00c1b"));
        }

        [Test]
        public void CheckSignatureExample2()
        {
            uint chainId = 137;
            var privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
            var address = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";

            var authProvider = new PolymarketAuthenticationProvider(new PolymarketCredentials().WithL1(Enums.SignType.EOA, privateKey, address));

            var parameters = new ParameterCollection
            {
                { "salt", "1515433236867" },
                { "maker", address },
                { "signer", address },
                { "tokenId", "11862165566757345985240476164489718219056735011698825377388402888080786399275" },
                { "makerAmount", "5000" },
                { "takerAmount", "5000000" },
                { "expiration", "0" },
                { "side", "BUY" },
                { "signatureType", 1 },
                { "timestamp", "1768900398000" },
                { "metadata", "0x0000000000000000000000000000000000000000000000000000000000000000" },
                { "builder", "0x0000000000000000000000000000000000000000000000000000000000000000" },
            };

            var result = authProvider.GetOrderSignature(parameters, chainId, true).ToLower();
            Assert.That(result, Is.EqualTo("0x2b0ebbc7a37efedff07bd93cfe0dd694aa15e9c67054eac911a2a803bea67f0e7b783e69ad9232e43abd8027d90fd7c61579b1f500177659378a3df44b2a021b1c"));
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<PolymarketRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<PolymarketSocketClient>();
        }
    }
}
