using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Authentication.Signing;
using Polymarket.Net.Enums;
using Secp256k1Net;
using System;
using System.Net;

namespace Polymarket.Net
{
    /// <summary>
    /// Layer 1 credential
    /// </summary>
    public class PolymarketL1Credential : CredentialSet
    {
        private string? _publicAddress;

        /// <summary>
        /// Sign type
        /// </summary>
        public SignType SignType { get; set; }
        /// <summary>
        /// Private key
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface
        /// </summary>
        public string? PolymarketFundingAddress { get; set; }

        /// <summary>
        /// Create new Polymarket Layer 1 credentials
        /// </summary>
        /// <param name="signType">Signature type</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="polymarketFundingAddress">Funding address, necessary when using Email signature type</param>
        public PolymarketL1Credential(SignType signType, string privateKey, string? polymarketFundingAddress = null) : base(GetPublicAddress(privateKey))
        {
            SignType = signType;
            PrivateKey = privateKey;
            PolymarketFundingAddress = polymarketFundingAddress;
        }

        /// <summary>
        /// Get the public address corresponding to the provided private key
        /// </summary>
        public string GetPublicAddress()
        {
            if (_publicAddress != null)
                return _publicAddress;

            _publicAddress = GetPublicAddress(PrivateKey);
            return _publicAddress;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketL1Credential(SignType, PrivateKey, PolymarketFundingAddress);

        private static string GetPublicAddress(string privateKey)
        {
            var publicKeyBytes = Secp256k1.CreatePublicKey(ExchangeHelpers.HexToBytesString(privateKey), false);

            var withoutPrefix = new byte[64];
            Array.Copy(publicKeyBytes, 1, withoutPrefix, 0, 64);

            var hash = CeSha3Keccack.CalculateHash(withoutPrefix);
            var pubAddress = new byte[20];
            Array.Copy(hash, hash.Length - 20, pubAddress, 0, 20);

            return "0x" + ExchangeHelpers.BytesToHexString(pubAddress);
        }

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(PrivateKey))
                throw new ArgumentException("PrivateKey not set", nameof(PrivateKey));
        }
    }
}
