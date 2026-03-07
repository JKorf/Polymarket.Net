using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Mid price
    /// </summary>
    public record PolymarketMidPrice
    {
        /// <summary>
        /// ["<c>mid</c>"] Mid
        /// </summary>
        [JsonPropertyName("mid")]
        public decimal Mid { get; set; }
    }


}
