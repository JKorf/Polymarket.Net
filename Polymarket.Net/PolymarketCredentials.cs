using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Web3;
using Polymarket.Net.Enums;
using System;

namespace Polymarket.Net
{
    /// <summary>
    /// Polymarket API credentials
    /// </summary>
    public class PolymarketCredentials : ApiCredentials
    {
        /// <summary>
        /// Layer 1 credentials
        /// </summary>
        public PolymarketL1Credential? L1 { get; set; }

        /// <summary>
        /// Layer 2 credentials
        /// </summary>
        public HMACPassCredential? L2 { get; set; }

        /// <summary>
        /// Perps credentials
        /// </summary>
        public PolymarketPerpsCredential? Perps { get; set; }

        /// <summary>
        /// Create new credentials
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PolymarketCredentials() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        /// <summary>
        /// Create new credentials
        /// </summary>
        /// <param name="layer1Credential">Layer 1 credentials</param>
        /// <param name="layer2Credentials">Layer 2 credentials</param>
        /// <param name="perpsCredentials">Perps credentials</param>
        public PolymarketCredentials(
            PolymarketL1Credential? layer1Credential,
            HMACPassCredential? layer2Credentials = null,
            PolymarketPerpsCredential? perpsCredentials = null)
        {
            L1 = layer1Credential;
            L2 = layer2Credentials;
            Perps = perpsCredentials;
        }

        /// <summary>
        /// Specify the Layer 1 credentials
        /// </summary>
        /// <param name="signType">Signature type</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
        public PolymarketCredentials WithL1(SignType signType, string privateKey, string? polymarketFundingAddress = null)
        {
            if (L1 != null) throw new InvalidOperationException("L1 credentials already set");

            L1 = new PolymarketL1Credential(signType, privateKey, polymarketFundingAddress);
            return this;
        }

        /// <summary>
        /// Specify the Layer 2 credentials
        /// </summary>
        /// <param name="key">Api key</param>
        /// <param name="secret">Api secret</param>
        /// <param name="pass">Passphrase</param>
        public PolymarketCredentials WithL2(string key, string secret, string pass)
        {
            if (L2 != null) throw new InvalidOperationException("L2 credentials already set");

            L2 = new HMACPassCredential(key, secret, pass);
            return this;
        }

        /// <summary>
        /// Specify the Perps credentials
        /// </summary>
        public PolymarketCredentials WithPerps(string secret, EthKey? signerKey, EthKey proxyKey)
        {
            if (Perps != null) throw new InvalidOperationException("Perps credentials already set");

            Perps = new PolymarketPerpsCredential(secret, signerKey, proxyKey);
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketCredentials { L1 = L1, L2 = L2, Perps = Perps };

        /// <inheritdoc />
        public override void Validate()
        {
            L1?.Validate();
            L2?.Validate();
            Perps?.Validate();
        }
    }
}
