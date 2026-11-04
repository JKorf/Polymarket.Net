using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Order Book update
    /// </summary>
    public record PolymarketPerpBookUpdate
    {
        /// <summary>
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public PolymarketOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public PolymarketOrderBookEntry[] Bids { get; set; } = [];
    }
}
