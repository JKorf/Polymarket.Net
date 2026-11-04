using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Socket update
    /// </summary>
    public record PolymarketSocketUpdate
    {
        /// <summary>
        /// ["<c>event_type</c>"] Event type
        /// </summary>
        [JsonPropertyName("event_type")]
        public string EventType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
