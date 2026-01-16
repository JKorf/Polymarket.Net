using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Socket update
    /// </summary>
    public record PolymarketSocketUpdate
    {
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("event_type")]
        public string EventType { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
