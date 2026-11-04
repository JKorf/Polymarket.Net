using System.Text.Json.Serialization;

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
        [JsonInclude, JsonPropertyName("neg_risk")]
        internal bool NegativeRiskInt
        {
            set => NegativeRisk = value;
        }
    }
}
