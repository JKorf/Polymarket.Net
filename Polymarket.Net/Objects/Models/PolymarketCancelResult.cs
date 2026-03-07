using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Order cancel result
    /// </summary>
    public record PolymarketCancelResult
    {
        /// <summary>
        /// ["<c>canceled</c>"] Canceled
        /// </summary>
        [JsonPropertyName("canceled")]
        public string[] Canceled { get; set; } = [];
        /// <summary>
        /// ["<c>not_canceled</c>"] Not canceled
        /// </summary>
        [JsonPropertyName("not_canceled")]
        public Dictionary<string, string> NotCanceled { get; set; } = new Dictionary<string, string>();
    }
}
