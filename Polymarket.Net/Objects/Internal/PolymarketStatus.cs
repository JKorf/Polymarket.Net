using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Internal
{
    /// <summary>
    /// Status result
    /// </summary>
    public record PolymarketStatus
    {
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>error</c>"] Error message
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
