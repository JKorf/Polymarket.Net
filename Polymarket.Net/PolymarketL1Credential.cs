using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Web3;
using Polymarket.Net.Enums;
using System;

namespace Polymarket.Net
{
    /// <summary>
    /// Layer 1 credential
    /// </summary>
    public class PolymarketL1Credential : CredentialSet
    {
        /// <summary>
        /// Sign type
        /// </summary>
        public SignType SignType { get; }
        /// <summary>
        /// Wallet key
        /// </summary>
        public EthKey WalletKey { get; }

        /// <summary>
        /// The polymarket funding or deposit address. Should be provided when using SignType.EOA or SignType.Poly1271. Can be found in your account in the web interface
        /// </summary>
        public string? PolymarketFundingAddress { get; }

        /// <summary>
        /// Create new Polymarket Layer 1 credentials
        /// </summary>
        /// <param name="signType">Signature type</param>
        /// <param name="walletKey">Wallet key, requires private key to be set</param>
        /// <param name="polymarketFundingAddress">Funding or deposit address, necessary when using SignType.EOA or SignType.Poly1271</param>
        public PolymarketL1Credential(SignType signType, EthKey walletKey, string? polymarketFundingAddress = null) : base(walletKey.PublicAddress)
        {
            if (walletKey.PrivateKey == null)
                throw new ArgumentException("Private key required for message signing");

            SignType = signType;
            WalletKey = walletKey;
            PolymarketFundingAddress = polymarketFundingAddress;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new PolymarketL1Credential(SignType, WalletKey, PolymarketFundingAddress);

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (WalletKey == null)
                throw new ArgumentException("Key not set", nameof(Key));

            if (SignType == SignType.Poly1271 && string.IsNullOrEmpty(PolymarketFundingAddress))
                throw new Exception("Poly1271 signing requires the DepositAddress to be provided in the PolymarketFundingAddress");
        }
    }
}
