using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Order heartbeat response
    /// </summary>
    public record PolymarketOrderHeartbeat
    {
        /// <summary>
        /// ["<c>heartbeat_id</c>"] The heart beat id, should be used for subsequent heartbeat requests
        /// </summary>
        [JsonPropertyName("heartbeat_id")]
        public string HeartbeatId { get; set; } = string.Empty;
    }
}
