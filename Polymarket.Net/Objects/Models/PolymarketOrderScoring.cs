using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Whether an order is scoring
    /// </summary>
    public record PolymarketOrderScoring
    {
        /// <summary>
        /// ["<c>scoring</c>"] Is order scoring
        /// </summary>
        [JsonPropertyName("scoring")]
        public bool Scoring { get; set; }
    }
}
