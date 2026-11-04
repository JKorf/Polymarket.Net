using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Closed only mode
    /// </summary>
    public record PolymarketClosedOnlyMode
    {
        /// <summary>
        /// ["<c>closed_only</c>"] Is closed only enabled
        /// </summary>
        [JsonPropertyName("closed_only")]
        public bool ClosedOnly { get; set; }
    }
}
