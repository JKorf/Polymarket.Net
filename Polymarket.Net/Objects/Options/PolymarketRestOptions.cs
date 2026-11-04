using CryptoExchange.Net.Objects.Options;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Options for the PolymarketRestClient
    /// </summary>
    public class PolymarketRestOptions : RestExchangeOptions<PolymarketEnvironment, PolymarketCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static PolymarketRestOptions Default { get; set; } = new PolymarketRestOptions()
        {
            Environment = PolymarketEnvironment.Live,
            AutoTimestamp = false
        };

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketRestOptions()
        {
            Default?.Set(this);
        }
                
         /// <summary>
        /// Prediction API options
        /// </summary>
        public PolymarketRestApiOptions PredictionOptions { get; private set; } = new PolymarketRestApiOptions()
        {
            BuilderCode = "0x7df2c024a68a29ed44b35d40ede5ef8e7d2ad7f4a8c9bf687735a7c2e005635b"
        };

        /// <summary>
        /// Perps API options
        /// </summary>
        public PolymarketRestPerpsApiOptions PerpsOptions { get; private set; } = new PolymarketRestPerpsApiOptions()
        {
            BuilderCode = "0xECdcc8933F29Ec16E820227C743A21dBa18C1149",
            BuilderFeePercentage = 0.01m
        };

        internal PolymarketRestOptions Set(PolymarketRestOptions targetOptions)
        {
            targetOptions = base.Set<PolymarketRestOptions>(targetOptions);
            targetOptions.PredictionOptions = PredictionOptions.Set(targetOptions.PredictionOptions);
            targetOptions.PerpsOptions = PerpsOptions.Set(targetOptions.PerpsOptions);
            return targetOptions;
        }
    }
}
