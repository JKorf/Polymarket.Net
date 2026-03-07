using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
