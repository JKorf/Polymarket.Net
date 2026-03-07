using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Closed only mode
    /// </summary>
    public record PolymarketClosedOnlyMode
    {
        /// <summary>
        /// ["<c>closed_only</c>"] Is closed only enabled
        /// </summary>
        [JsonPropertyName("closed_only")]
        public bool ClosedOnly { get; set; }
    }
}
