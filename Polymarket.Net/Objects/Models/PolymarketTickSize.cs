using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Tick size
    /// </summary>
    public record PolymarketTickSize
    {
        /// <summary>
        /// ["<c>minimum_tick_size</c>"] Minimum tick size
        /// </summary>
        [JsonPropertyName("minimum_tick_size")]
        public decimal MinTickSize { get; set; }
    }
}
