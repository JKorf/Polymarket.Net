using CryptoExchange.Net.Authentication;
using Polymarket.Net.Clients.ClobApi;

namespace Polymarket.Net.Objects
{
    /// <summary>
    /// Polymarket credentials
    /// </summary>
    public class PolymarketCredentials : ApiCredentials
    {
        /// <summary>
        /// The polymarket address, can be found in your account in the web interface
        /// </summary>
        public string PolymarketAddress { get; set; }
        /// <summary>
        /// Private key for the funding address
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

        /// <summary>
        /// Create new API credentials with a Polymarket public address and the private key for the funding address
        /// </summary>
        /// <param name="polymarketAddress">The polymarket address, can be found in your account in the web interface</param>
        /// <param name="l1PrivateKey">Private key for the funding address</param>
        public PolymarketCredentials(string polymarketAddress, string l1PrivateKey) : base(polymarketAddress, l1PrivateKey, null, ApiCredentialsType.Hmac)
        {
            PolymarketAddress = polymarketAddress;
            L1PrivateKey = l1PrivateKey;
        }

        /// <summary>
        /// Create new API credentials with a Polymarket public address, the private key for the funding address and previously obtained layer 2 credentials using <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/>
        /// </summary>
        /// <param name="polymarketAddress">The polymarket address, can be found in your account in the web interface</param>
        /// <param name="l1PrivateKey">Private key for the funding address</param>
        /// <param name="l2Key">The layer 2 API key previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        /// <param name="l2Secret">The layer 2 secret previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        /// <param name="l2Pass">The layer 2 passphrase previously obtained with <see cref="PolymarketRestClientClobApiAccount.GetOrCreateApiCredentialsAsync"/></param>
        public PolymarketCredentials(
            string polymarketAddress,
            string l1PrivateKey,
            string l2Key, 
            string l2Secret, 
            string l2Pass) : base(l2Key, l2Secret, l2Pass, ApiCredentialsType.Hmac)
        {
            PolymarketAddress = polymarketAddress;
            L1PrivateKey = l1PrivateKey;
            L2ApiKey = l2Key;
            L2Secret = l2Secret;
            L2Pass = l2Pass;
        }

        /// <inheritdoc/>
        public override ApiCredentials Copy()
        {
            if (L2ApiKey != null)
            {
                return new PolymarketCredentials(PolymarketAddress, L1PrivateKey, L2ApiKey, L2Secret!, L2Pass!)
                {
                    CredentialType = this.CredentialType
                };
            }
            else
            {
                return new PolymarketCredentials(PolymarketAddress, L1PrivateKey)
                {
                    CredentialType = this.CredentialType
                };
            }
        }
    }
}
