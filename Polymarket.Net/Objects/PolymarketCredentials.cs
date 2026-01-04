using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects
{
    public class PolymarketCredentials : ApiCredentials
    {
        public string PublicAddress { get; set; }
        public string L1PrivateKey { get; set; }
        public string? L2ApiKey { get; set; }
        public string? L2Secret { get; set; }
        public string? L2Pass { get; set; }

        public PolymarketCredentials(string publicAddress, string l1PrivateKey) : base(publicAddress, l1PrivateKey, null, ApiCredentialsType.Hmac)
        {
            PublicAddress = publicAddress;
            L1PrivateKey = l1PrivateKey;
        }

        public PolymarketCredentials(
            string publicAddress, 
            string l1PrivateKey,
            string l2Key, 
            string l2Secret, 
            string l2Pass) : base(l2Key, l2Secret, l2Pass, ApiCredentialsType.Hmac)
        {
            PublicAddress = publicAddress;
            L1PrivateKey = l1PrivateKey;
            L2ApiKey = l2Key;
            L2Secret = l2Secret;
            L2Pass = l2Pass;
        }

        public override ApiCredentials Copy()
        {
            if (L2ApiKey != null)
            {
                return new PolymarketCredentials(PublicAddress, L1PrivateKey, L2ApiKey, L2Secret!, L2Pass!)
                {
                    CredentialType = this.CredentialType
                };
            }
            else
            {
                return new PolymarketCredentials(PublicAddress, L1PrivateKey)
                {
                    CredentialType = this.CredentialType
                };
            }
        }
    }
}
