using Polymarket.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Price change update
    /// </summary>
    public record PolymarketPriceChangeUpdate: PolymarketSocketUpdate
    {
        /// <summary>
        /// Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;

        /// <summary>
        /// Price changes
        /// </summary>
        [JsonPropertyName("price_changes")]
        public PolymarketPriceChange[] PriceChanges { get; set; } = [];
    }

    /// <summary>
    /// Price change info
    /// </summary>
    public record PolymarketPriceChange
    {
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("best_bid")]
        public decimal BestBid { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("best_ask")]
        public decimal BestAsk { get; set; }
    }
}
