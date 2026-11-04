using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Whether an order is scoring
    /// </summary>
    public record PolymarketOrderScoring
    {
        /// <summary>
        /// ["<c>scoring</c>"] Is order scoring
        /// </summary>
        [JsonPropertyName("scoring")]
        public bool Scoring { get; set; }
    }
}
