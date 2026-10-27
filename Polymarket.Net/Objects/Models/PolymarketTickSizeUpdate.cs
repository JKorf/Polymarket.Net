using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Tick size update
    /// </summary>
    public record PolymarketTickSizeUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// ["<c>market</c>"] Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset_id</c>"] Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>old_tick_size</c>"] Old tick size
        /// </summary>
        [JsonPropertyName("old_tick_size")]
        public decimal OldTickSize { get; set; }
        /// <summary>
        /// ["<c>new_tick_size</c>"] New tick size
        /// </summary>
        [JsonPropertyName("new_tick_size")]
        public decimal NewTickSize { get; set; }
    }
}
