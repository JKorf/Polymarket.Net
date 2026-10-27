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
        /// Prediction API options
        /// </summary>
        public SocketApiOptions PredictionOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Perps API options
        /// </summary>
        public SocketApiOptions PerpsOptions { get; private set; } = new SocketApiOptions();

        internal PolymarketSocketOptions Set(PolymarketSocketOptions targetOptions)
        {
            targetOptions = base.Set<PolymarketSocketOptions>(targetOptions);            
            targetOptions.PredictionOptions = PredictionOptions.Set(targetOptions.PredictionOptions);
            targetOptions.PerpsOptions = PerpsOptions.Set(targetOptions.PerpsOptions);
            return targetOptions;
        }
    }
}
