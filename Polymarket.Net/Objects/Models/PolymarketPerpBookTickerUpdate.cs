using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Book Ticker update
    /// </summary>
    public record PolymarketPerpBookTickerUpdate
    {
        /// <summary>
        /// ["<c>iid</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("iid")]
        public long SymbolId { get; set; }
        /// <summary>
        /// ["<c>bp</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bp")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bq</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("bq")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>ap</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("ap")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>aq</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("aq")]
        public decimal BestAskQuantity { get; set; }
    }
}
