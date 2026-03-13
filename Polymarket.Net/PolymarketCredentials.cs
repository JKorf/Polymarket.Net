using CryptoExchange.Net.Authentication;
using Polymarket.Net.Enums;

namespace Polymarket.Net
{
    /// <summary>
    /// Polymarket API credentials
    /// </summary>
    public class PolymarketCredentials : ApiCredentials
    {
        public PolymarketL1Credential L1Credential => GetCredential<PolymarketL1Credential>();
        public HMACCredential L2Credential => GetCredential<HMACCredential>();

        /// <summary>
        /// Create credentials using Layer 1 private key and optional Polymarket funding address. If the FuturesV3 API will be used use <see cref="PolymarketCredentials.PolymarketCredentials(HMACCredential?, PolymarketECDSACredential?)" /> instead.
        /// </summary>
        public PolymarketCredentials(SignType signType, string privateKey, string? polymarketFundingAddress = null)
            : this(new PolymarketL1Credential(signType, privateKey, polymarketFundingAddress)) { }

        public PolymarketCredentials(SignType signType,
            string l1PrivateKey,
            string l2Key,
            string l2Secret,
            string l2Pass,
            string? polymarketFundingAddress = null)
            : this (new PolymarketL1Credential(signType, l1PrivateKey, polymarketFundingAddress), new HMACCredential(l2Key, l2Secret, l2Pass)) { }

        /// <summary>
        /// Create credentials using HMAC credentials. If the FuturesV3 API will be used use <see cref="PolymarketCredentials.PolymarketCredentials(HMACCredential?, PolymarketECDSACredential?)" /> instead.
        /// </summary>
        /// <param name="hmacCredential">HMAC credentials for the Spot and Futures API</param>
        public PolymarketCredentials(HMACCredential hmacCredential)
            : this(null, hmacCredential) 
        {
        }

        /// <summary>
        /// Create credentials using ECDSA credentials. This only grants access to the FuturesV3 API.If the Spot API will be used use <see cref="PolymarketCredentials.PolymarketCredentials(HMACCredential?, PolymarketECDSACredential?)" /> instead.
        /// </summary>
        /// <param name="futuresV3Credential">ECDSA credentials for the FuturesV3 API</param>
        public PolymarketCredentials(PolymarketL1Credential futuresV3Credential)
            : this(futuresV3Credential, null)
        {
        }

        /// <summary>
        /// Create credentials proving both HMAC credentials for the Spot/Futures API's and ECDSA credentials for the FuturesV3 API
        /// </summary>
        /// <param name="hmacCredential">HMAC credentials for the Spot and Futures API</param>
        /// <param name="futuresV3Credential">ECDSA credentials for the FuturesV3 API</param>
        public PolymarketCredentials(PolymarketL1Credential? futuresV3Credential, HMACCredential? hmacCredential)
            : base(hmacCredential, futuresV3Credential)
        {
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => 
            new PolymarketCredentials(GetCredential<PolymarketL1Credential>(), GetCredential<HMACCredential>());
    }
}
