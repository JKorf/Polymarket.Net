using CryptoExchange.Net.Objects.Options;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Options for the PolymarketSocketClient
    /// </summary>
    public class PolymarketSocketOptions : SocketExchangeOptions<PolymarketEnvironment, PolymarketCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static PolymarketSocketOptions Default { get; set; } = new PolymarketSocketOptions()
        {
            Environment = PolymarketEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketSocketOptions()
        {
            Default?.Set(this);
        }
        
         /// <summary>
        /// Clob API options
        /// </summary>
        public SocketApiOptions ClobOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Gamma API options
        /// </summary>
        public SocketApiOptions GammaOptions { get; private set; } = new SocketApiOptions();

        internal PolymarketSocketOptions Set(PolymarketSocketOptions targetOptions)
        {
            targetOptions = base.Set<PolymarketSocketOptions>(targetOptions);            
            targetOptions.ClobOptions = ClobOptions.Set(targetOptions.ClobOptions);
            targetOptions.GammaOptions = ClobOptions.Set(targetOptions.GammaOptions);
            return targetOptions;
        }
    }
}
