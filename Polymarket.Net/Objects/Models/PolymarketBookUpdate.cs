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
    public record PolymarketBookUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last_trade_price")]
        public decimal LastTradePrice { get; set; }
        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public PolymarketBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public PolymarketBookEntry[] Asks { get; set; } = [];
    }
}
