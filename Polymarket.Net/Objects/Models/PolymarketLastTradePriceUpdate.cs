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
    /// Last trade price update
    /// </summary>
    public record PolymarketLastTradePriceUpdate : PolymarketSocketUpdate
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
        /// Transaction hash
        /// </summary>
        [JsonPropertyName("transaction_hash")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fee rate BPS
        /// </summary>
        [JsonPropertyName("fee_rate_bps")]
        public decimal FeeRateBps { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
    }

}
