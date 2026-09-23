using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Configuration;
using System;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Polymarket options
    /// </summary>
    public class PolymarketOptions : LibraryOptions<PolymarketRestOptions, PolymarketSocketOptions, PolymarketCredentials, PolymarketEnvironment>
    {
        /// <summary>
        /// Create PolymarketOptions instance using the provided configuration action
        /// </summary>
        public static PolymarketOptions Create(Action<PolymarketOptions>? configure = null)
        {
            var options = CreateUnconfigured();
            configure?.Invoke(options);
            return Normalize(options);
        }

        /// <summary>
        /// Create PolymarketOptions using the provided IConfiguration
        /// </summary>
        public static PolymarketOptions CreateFromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var options = CreateUnconfigured();
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid Polymarket configuration provided", ex);
            }

            if (options.Environment != null)
                options.Environment = PolymarketEnvironment.GetEnvironmentByName(options.Environment.Name) ?? options.Environment;
            if (options.Rest?.Environment != null)
                options.Rest.Environment = PolymarketEnvironment.GetEnvironmentByName(options.Rest.Environment.Name) ?? options.Rest.Environment;
            if (options.Socket?.Environment != null)
                options.Socket.Environment = PolymarketEnvironment.GetEnvironmentByName(options.Socket.Environment.Name) ?? options.Socket.Environment;

            return Normalize(options);
        }

        private static PolymarketOptions CreateUnconfigured()
        {
            var options = new PolymarketOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            return options;
        }

        private static PolymarketOptions Normalize(PolymarketOptions options)
        {
            if (options.Rest == null)
                throw new ArgumentException("REST options cannot be null", nameof(options));
            if (options.Socket == null)
                throw new ArgumentException("Socket options cannot be null", nameof(options));

            options.Rest.Environment ??= options.Environment ?? PolymarketEnvironment.Live;
            options.Rest.ApiCredentials ??= options.ApiCredentials;
            options.Socket.Environment ??= options.Environment ?? PolymarketEnvironment.Live;
            options.Socket.ApiCredentials ??= options.ApiCredentials;
            return options;
        }
    }
}
