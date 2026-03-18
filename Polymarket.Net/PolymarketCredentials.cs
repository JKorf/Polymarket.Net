using CryptoExchange.Net.Authentication;
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
        public PolymarketL1Credential L1 { get; set; }
        /// <summary>
        /// Layer 2 credentials
        /// </summary>
        public HMACCredential? L2 { get; set; }

        public PolymarketCredentials WithL1(SignType signType, string privateKey, string? polymarketFundingAddress = null)
        {
            if (L1 != null) throw new InvalidOperationException("L1 credentials already set");

            L1 = new PolymarketL1Credential(signType, privateKey, polymarketFundingAddress);
            return this;
        }

        public PolymarketCredentials WithL2(string key, string secret, string pass)
        {
            if (L2 != null) throw new InvalidOperationException("L2 credentials already set");

            L2 = new HMACCredential(key, secret, pass);
            return this;
        }



        ///// <summary>
        ///// Create credentials using Layer 1 private key and optional Polymarket funding address.
        ///// </summary>
        ///// <param name="signType">Signature type</param>
        ///// <param name="privateKey">Private key</param>
        ///// <param name="polymarketFundingAddress">Funding address, required when signType is Email</param>
        //public PolymarketCredentials(SignType signType, string privateKey, string? polymarketFundingAddress = null)
        //    : this(new PolymarketL1Credential(signType, privateKey, polymarketFundingAddress)) { }

        ///// <summary>
        ///// Create credentials using Layer 1 private key and optional Polymarket funding address, and layer 2 HMAC credentials
        ///// </summary>
        ///// <param name="signType">Signature type</param>
        ///// <param name="l1PrivateKey">Layer 1 private key</param>
        ///// <param name="l2Key">Layer 2 API key</param>
        ///// <param name="l2Secret">Layer 2 API secret</param>
        ///// <param name="l2Pass">Layer 2 passphrase</param>
        ///// <param name="polymarketFundingAddress">Funding address, required when signType is Email</param>
        //public PolymarketCredentials(SignType signType,
        //    string l1PrivateKey,
        //    string l2Key,
        //    string l2Secret,
        //    string l2Pass,
        //    string? polymarketFundingAddress = null)
        //    : this (new PolymarketL1Credential(signType, l1PrivateKey, polymarketFundingAddress), new HMACCredential(l2Key, l2Secret, l2Pass)) { }

        ///// <summary>
        ///// Create layer 1 credentials 
        ///// </summary>
        ///// <param name="layer1Credentials">Layer 1 credentials</param>
        //public PolymarketCredentials(PolymarketL1Credential layer1Credentials)
        //    : this(layer1Credentials, null)
        //{
        //}

        ///// <summary>
        ///// Create credentials using layer 1 credentials and layer 2 HMAC credentials
        ///// </summary>
        ///// <param name="layer1Credentials">Layer 1 credentials</param>
        ///// <param name="layer2Credentials">Layer 2 HMAC credentials</param>
        //public PolymarketCredentials(PolymarketL1Credential? layer1Credentials, HMACCredential? layer2Credentials)
        //    : base(layer2Credentials, layer1Credentials)
        //{
        //}

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketCredentials { L1 = L1, L2 = L2 };
    }
}
