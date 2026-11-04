using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Price update
    /// </summary>
    public record PolymarketPriceUpdate
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>source</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>full_accuracy_value</c>"] Value
        /// </summary>
        [JsonPropertyName("full_accuracy_value")]
        public decimal Value { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Price update
    /// </summary>
    public record PolymarketEquityPriceUpdate: PolymarketPriceUpdate
    {
        /// <summary>
        /// ["<c>market_status</c>"] Market status
        /// </summary>
        [JsonPropertyName("market_status")]
        public MarketStatus Status { get; set; }
    }
}
