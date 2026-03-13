using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Authentication.Signing;
using Polymarket.Net.Enums;
using Secp256k1Net;
using System;

namespace Polymarket.Net
{
    public class PolymarketL1Credential : CredentialPair
    {
        private string? _publicAddress;

        public SignType SignType { get; set; }
        public string PrivateKey { get; set; }
        /// <summary>
        /// The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface
        /// </summary>
        public string? PolymarketFundingAddress { get; set; }

        public override ApiCredentialsType CredentialType => ApiCredentialsType.Ecdsa;

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
    }
}
