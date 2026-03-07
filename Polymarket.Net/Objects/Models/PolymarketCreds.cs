using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// L2 credentials
    /// </summary>
    public record PolymarketCreds
    {
        /// <summary>
        /// ["<c>apiKey</c>"] API key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>secret</c>"] Secret
        /// </summary>
        [JsonPropertyName("secret")]
        public string Secret { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>passphrase</c>"] The passphrase
        /// </summary>
        [JsonPropertyName("passphrase")]
        public string Passphrase { get; set; } = string.Empty;
    }
}
