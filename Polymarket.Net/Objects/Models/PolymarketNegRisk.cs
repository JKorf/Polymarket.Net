using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Negative risk
    /// </summary>
    public record PolymarketNegRisk
    {
        /// <summary>
        /// ["<c>neg-risk</c>"] Negative risk
        /// </summary>
        [JsonPropertyName("neg-risk")]
        public bool NegativeRisk { get; set; }
    }
}
