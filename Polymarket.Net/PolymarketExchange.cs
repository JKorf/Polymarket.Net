using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;

namespace Polymarket.Net
{
    /// <summary>
    /// Polymarket exchange information and configuration
    /// </summary>
    public static class PolymarketExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Polymarket";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "Polymarket";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/Polymarket.Net/main/Polymarket.Net/Icon/icon.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.polymarket.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.polymarket.com/api-reference"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.DEX;

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<PolymarketSourceGenerationContext>());

        /// <summary>
        /// Rate limiter configuration for the Polymarket API
        /// </summary>
        public static PolymarketRateLimiters RateLimiter { get; } = new PolymarketRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Polymarket API
    /// </summary>
    public class PolymarketRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal PolymarketRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            Polymarket = new RateLimitGate("Polymarket");
            Polymarket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Polymarket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Polymarket { get; private set; }

    }
}
