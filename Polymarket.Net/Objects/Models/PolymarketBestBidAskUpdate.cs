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
    /// Best bid/ask update
    /// </summary>
    public record PolymarketBestBidAskUpdate : PolymarketSocketUpdate
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
        /// ["<c>best_bid</c>"] Best bid
        /// </summary>
        [JsonPropertyName("best_bid")]
        public decimal BestBid { get; set; }
        /// <summary>
        /// ["<c>best_ask</c>"] Best ask
        /// </summary>
        [JsonPropertyName("best_ask")]
        public decimal BestAsk { get; set; }
        /// <summary>
        /// ["<c>spread</c>"] Spread
        /// </summary>
        [JsonPropertyName("spread")]
        public decimal Spread { get; set; }
    }
}
