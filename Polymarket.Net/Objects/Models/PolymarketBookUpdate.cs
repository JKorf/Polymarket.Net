using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Price change update
    /// </summary>
    public record PolymarketBookUpdate : PolymarketSocketUpdate
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
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>hash</c>"] Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>last_trade_price</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last_trade_price")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public PolymarketBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public PolymarketBookEntry[] Asks { get; set; } = [];
    }
}
