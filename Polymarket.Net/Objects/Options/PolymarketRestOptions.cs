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
        /// Clob API options
        /// </summary>
        public RestApiOptions ClobOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Gamma API options
        /// </summary>
        public RestApiOptions GammaOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Data API options
        /// </summary>
        public RestApiOptions DataOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Builder code
        /// </summary>
        public string BuilderCode { get; set; } = "0x7df2c024a68a29ed44b35d40ede5ef8e7d2ad7f4a8c9bf687735a7c2e005635b";

        internal PolymarketRestOptions Set(PolymarketRestOptions targetOptions)
        {
            targetOptions = base.Set<PolymarketRestOptions>(targetOptions);
            targetOptions.BuilderCode = BuilderCode;
            targetOptions.ClobOptions = ClobOptions.Set(targetOptions.ClobOptions);
            targetOptions.GammaOptions = GammaOptions.Set(targetOptions.ClobOptions);
            return targetOptions;
        }
    }
}
