namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Signature type
    /// </summary>
    public enum SignType
    {
        /// <summary>
        /// Standard EOA (Externally Owned Account) signatures - includes MetaMask, hardware wallets, and any wallet where you control the private key directly
        /// </summary>
        EOA = 0,
        /// <summary>
        /// Email/Magic wallet signatures (delegated signing)
        /// </summary>
        Email = 1,
        /// <summary>
        /// Browser wallet proxy signatures (when using a proxy contract, not direct wallet connections)
        /// </summary>
        Proxy = 2,
        /// <summary>
        /// Poly 1271 signatures, using an deposit address. For accounts created after 04 May 2026
        /// </summary>
        Poly1271 = 3
    }
}
