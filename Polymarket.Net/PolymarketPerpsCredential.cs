using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Authentication.Signing;
using CryptoExchange.Net.Web3;
using Polymarket.Net.Enums;
using Secp256k1Net;
using System;

namespace Polymarket.Net
{
    /// <summary>
    /// Perps credential
    /// </summary>
    public class PolymarketPerpsCredential : CredentialSet
    {
        /// <summary>
        /// Signer key
        /// </summary>
        public EthKey? SignerKey { get; set; }

        /// <summary>
        /// Proxy key
        /// </summary>
        public EthKey ProxyKey { get; set; }

        /// <summary>
        /// The API secret from the CreateProxy call
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Create new Polymarket Perps credentials
        /// </summary>
        public PolymarketPerpsCredential(string secret, EthKey? signerKey, EthKey proxyKey) : base(proxyKey.PublicAddress)
        {
            SignerKey = signerKey;
            ProxyKey = proxyKey;
            Secret = secret;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketPerpsCredential(Secret, SignerKey, ProxyKey);

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (ProxyKey == null)
                throw new ArgumentException("Proxy not set", nameof(ProxyKey));
        }
    }
}
