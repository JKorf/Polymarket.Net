using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Authentication.Signing;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Enums;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects
{
    /// <summary>
    /// Polymarket credential
    /// </summary>
    public class PolymarketCredential : CredentialPair
    {
        private string? _publicAddress;
        private byte[]? _hmacBytes;

        /// <summary>
        /// Signature type
        /// </summary>
        public SignType SignatureType { get; set; }
        /// <summary>
        /// The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface
        /// </summary>
        public string? PolymarketFundingAddress { get; set; }
        /// <summary>
        /// Private key for the trading wallet
        /// </summary>
        public string L1PrivateKey { get; set; }
        /// <summary>
        /// The layer 2 API key previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
        /// </summary>
        public string? L2ApiKey { get; set; }
        /// <summary>
        /// The layer 2 secret previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
        /// </summary>
        public string? L2Secret { get; set; }
        /// <summary>
        /// The layer 2 passphrase previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
        /// </summary>
        public string? L2Pass { get; set; }

        /// <inheritdoc />
        public override ApiCredentialsType CredentialType => ApiCredentialsType.Custom;
        /// <inheritdoc />
        public override string PublicIdentifier => GetPublicAddress();

        /// <summary>
        /// DI constructor
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use with parameters instead.")]
        public PolymarketCredential() { }

        /// <summary>
        /// Create new API credentials with a Polymarket public address and the private key for the funding address
        /// </summary>
        /// <param name="signatureType">The signature type</param>
        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
        /// <param name="l1PrivateKey">Private key for the trading wallet</param>
        public PolymarketCredential(SignType signatureType, string l1PrivateKey, string? polymarketFundingAddress = null)
        {
            SignatureType = signatureType;
            PolymarketFundingAddress = polymarketFundingAddress;
            L1PrivateKey = l1PrivateKey;
        }

        /// <summary>
        /// Create new API credentials with a Polymarket public address, the private key for the funding address and previously obtained layer 2 credentials using <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
        /// </summary>
        /// <param name="signatureType">The signature type</param>
        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
        /// <param name="l1PrivateKey">Private key for the trading wallet</param>
        /// <param name="l2Key">The layer 2 API key previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        /// <param name="l2Secret">The layer 2 secret previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        /// <param name="l2Pass">The layer 2 passphrase previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        public PolymarketCredential(
            SignType signatureType,
            string l1PrivateKey,
            string l2Key,
            string l2Secret,
            string l2Pass,
            string? polymarketFundingAddress = null)
        {
            SignatureType = signatureType;
            PolymarketFundingAddress = polymarketFundingAddress;
            L1PrivateKey = l1PrivateKey;
            L2ApiKey = l2Key;
            L2Secret = l2Secret;
            L2Pass = l2Pass;
        }

        /// <summary>
        /// Get the public address corresponding to the provided private key
        /// </summary>
        public string GetPublicAddress()
        {
            if (_publicAddress != null)
                return _publicAddress;

            var publicKeyBytes = Secp256k1.CreatePublicKey(ExchangeHelpers.HexToBytesString(L1PrivateKey), false);

            var withoutPrefix = new byte[64];
            Array.Copy(publicKeyBytes, 1, withoutPrefix, 0, 64);

            var hash = CeSha3Keccack.CalculateHash(withoutPrefix);
            var pubAddress = new byte[20];
            Array.Copy(hash, hash.Length - 20, pubAddress, 0, 20);

            _publicAddress = "0x" + ExchangeHelpers.BytesToHexString(pubAddress);
            return _publicAddress;
        }

        /// <summary>
        /// Get Layer 2 HMAC secret bytes
        /// </summary>
        /// <returns></returns>
        public byte[]? GetL2HmacBytes()
        {
            if (string.IsNullOrEmpty(L2Secret))
                return null;

            return _hmacBytes ??= Convert.FromBase64String(L2Secret!.Replace('-', '+').Replace('_', '/'));
        }
    }
}
