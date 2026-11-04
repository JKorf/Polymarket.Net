using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// API keys
    /// </summary>
    public record PolymarketApiKeys
    {
        /// <summary>
        /// ["<c>apiKeys</c>"] API keys
        /// </summary>
        [JsonPropertyName("apiKeys")]
        public string[] ApiKeys { get; set; } = [];
    }
}
