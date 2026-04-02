using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net;

namespace Polymarket.Net
{
    /// <summary>
    /// Polymarket platform information and configuration
    /// </summary>
    public static class PolymarketPlatform
    {
        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<PolymarketSourceGenerationContext>());
        
        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Polymarket",
                "Polymarket",
                "https://raw.githubusercontent.com/JKorf/Polymarket.Net/main/Polymarket.Net/Icon/icon.png",
                "https://www.polymarket.com",
                ["https://docs.polymarket.com/api-reference"],
                PlatformType.PredictionMarket,
                CentralizationType.Decentralized
                );

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
            ClobApi = new RateLimitGate("Clob")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 9000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // 9000 requests per 10 seconds

            GammaApi = new RateLimitGate("Gamma")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // 1000 requests per 10 seconds

            DataApi = new RateLimitGate("Data")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // 1000 requests per 10 seconds

            ClobApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            ClobApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            GammaApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            GammaApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            DataApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            DataApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate ClobApi { get; private set; }
        internal IRateLimitGate GammaApi { get; private set; }
        internal IRateLimitGate DataApi { get; private set; }
    }
}
