//using CryptoExchange.Net.Authentication;
//using Polymarket.Net.Clients.ClobApi;
//using Polymarket.Net.Enums;
//using System;
//using System.Linq;

//namespace Polymarket.Net.Objects
//{
//    /// <summary>
//    /// Polymarket credentials
//    /// </summary>
//    public class PolymarketCredentials : ApiCredentials
//    {
//        /// <summary>
//        /// Polymarket credentials
//        /// </summary>
//        public PolymarketCredential? Poly
//        {
//            get => (PolymarketCredential?)CredentialPairs.SingleOrDefault(x => x.CredentialType == ApiCredentialsType.Custom);
//            set => AddOrRemoveCredential(ApiCredentialsType.Custom, value);
//        }

//        /// <summary>
//        /// DI constructor
//        /// </summary>
//        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use with parameters instead.")]
//        public PolymarketCredentials() { }

//        /// <summary>
//        /// Create new API credentials with a Polymarket public address and the private key for the funding address
//        /// </summary>
//        /// <param name="signType">The signature type</param>
//        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
//        /// <param name="l1PrivateKey">Private key for the trading wallet</param>
//        public PolymarketCredentials(SignType signType, string l1PrivateKey, string? polymarketFundingAddress = null)
//            : base(new PolymarketCredential(signType, l1PrivateKey, polymarketFundingAddress))
//        {
//        }

//        /// <summary>
//        /// Create new API credentials with a Polymarket public address, the private key for the funding address and previously obtained layer 2 credentials using <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
//        /// </summary>
//        /// <param name="signType">The signature type</param>
//        /// <param name="polymarketFundingAddress">The polymarket funding address when using email/magic wallets. Can be found in your account in the web interface</param>
//        /// <param name="l1PrivateKey">Private key for the trading wallet</param>
//        /// <param name="l2Key">The layer 2 API key previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
//        /// <param name="l2Secret">The layer 2 secret previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
//        /// <param name="l2Pass">The layer 2 passphrase previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
//        public PolymarketCredentials(
//            SignType signType,
//            string l1PrivateKey,
//            string l2Key,
//            string l2Secret,
//            string l2Pass,
//            string? polymarketFundingAddress = null) 
//            : base(new PolymarketCredential(signType, l1PrivateKey, l2Key, l2Secret, l2Pass, polymarketFundingAddress))
//        {
//        }
//    }
//}
