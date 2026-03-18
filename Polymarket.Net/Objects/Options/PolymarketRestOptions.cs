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
        /// Builder API api key
        /// </summary>
        public string? BuilderApiKey { get; set; }
        /// <summary>
        /// Builder API secret
        /// </summary>
        public string? BuilderSecret { get; set; }
        /// <summary>
        /// Builder API passphrase
        /// </summary>
        public string? BuilderPass { get; set; }

        internal PolymarketRestOptions Set(PolymarketRestOptions targetOptions)
        {
            targetOptions = base.Set<PolymarketRestOptions>(targetOptions);
            targetOptions.BuilderApiKey = BuilderApiKey;
            targetOptions.BuilderSecret = BuilderSecret;
            targetOptions.BuilderPass = BuilderPass;
            targetOptions.ClobOptions = ClobOptions.Set(targetOptions.ClobOptions);
            targetOptions.GammaOptions = GammaOptions.Set(targetOptions.ClobOptions);
            return targetOptions;
        }
    }
}
