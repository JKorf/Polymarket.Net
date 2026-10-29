using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Ticker update
    /// </summary>
    public record PolymarketPerpTickerUpdate
    {
        /// <summary>
        /// ["<c>iid</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("iid")]
        public long SymbolId { get; set; }
        /// <summary>
        /// ["<c>idx</c>"] Index price
        /// </summary>
        [JsonPropertyName("idx")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// ["<c>mark</c>"] Mark price
        /// </summary>
        [JsonPropertyName("mark")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>last</c>"] Last price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>mid</c>"] Mid price
        /// </summary>
        [JsonPropertyName("mid")]
        public decimal MidPrice { get; set; }
        /// <summary>
        /// ["<c>oi</c>"] Open interest
        /// </summary>
        [JsonPropertyName("oi")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// ["<c>fr</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("fr")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// ["<c>nxf</c>"] Next funding time
        /// </summary>
        [JsonPropertyName("nxf")]
        public DateTime NextFundingTime { get; set; }
    }
}
