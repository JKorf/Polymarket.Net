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
        /// Prediction API Layer 1 credentials
        /// </summary>
        public PolymarketL1Credential? PredictionL1 { get; set; }

        /// <summary>
        /// Prediction API Layer 2 credentials
        /// </summary>
        public HMACPassCredential? PredictionL2 { get; set; }

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
        /// <param name="predictionL1">Layer 1 credentials</param>
        /// <param name="predictionL2">Layer 2 credentials</param>
        /// <param name="perps">Perps credentials</param>
        public PolymarketCredentials(
            PolymarketL1Credential? predictionL1 = null,
            HMACPassCredential? predictionL2 = null,
            PolymarketPerpsCredential? perps = null)
        {
            PredictionL1 = predictionL1;
            PredictionL2 = predictionL2;
            Perps = perps;
        }

        /// <summary>
        /// Specify the Layer 1 credentials
        /// </summary>
        /// <param name="signType">Signature type</param>
        /// <param name="walletKey">Wallet key, requires private key to be set</param>
        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
        public PolymarketCredentials WithL1(SignType signType, EthKey walletKey, string? polymarketFundingAddress = null)
        {
            if (PredictionL1 != null) throw new InvalidOperationException("PredictionL1 credentials already set");

            PredictionL1 = new PolymarketL1Credential(signType, walletKey, polymarketFundingAddress);
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
            if (PredictionL2 != null) throw new InvalidOperationException("PredictionL2 credentials already set");

            PredictionL2 = new HMACPassCredential(key, secret, pass);
            return this;
        }

        /// <summary>
        /// Specify the Perps credentials
        /// </summary>
        public PolymarketCredentials WithPerps(string? secret, EthKey? signerKey, EthKey? proxyKey)
        {
            if (Perps != null) throw new InvalidOperationException("Perps credentials already set");

            Perps = new PolymarketPerpsCredential(secret, signerKey, proxyKey);
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketCredentials { PredictionL1 = PredictionL1, PredictionL2 = PredictionL2, Perps = Perps };

        /// <inheritdoc />
        public override void Validate()
        {
            PredictionL1?.Validate();
            PredictionL2?.Validate();
            Perps?.Validate();

            if (PredictionL1 == null && PredictionL2 == null && Perps == null)
                throw new Exception("No credentials provided. Provide at least one of PredictionL1, PredictionL2 or Perps credentials.");
        }
    }
}
