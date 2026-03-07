using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Geographic restrictions
    /// </summary>
    public record PolymarketGeoRestriction
    {
        /// <summary>
        /// ["<c>blocked</c>"] Is blocked
        /// </summary>
        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }
        /// <summary>
        /// ["<c>ip</c>"] IP address
        /// </summary>
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>country</c>"] Country
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>region</c>"] Region
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;
    }
}
