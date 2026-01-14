using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Sports team info
    /// </summary>
    public record PolymarketSportsTeam
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// League
        /// </summary>
        [JsonPropertyName("league")]
        public string League { get; set; } = string.Empty;
        /// <summary>
        /// Record
        /// </summary>
        [JsonPropertyName("record")]
        public string Record { get; set; } = string.Empty;
        /// <summary>
        /// Logo
        /// </summary>
        [JsonPropertyName("logo")]
        public string Logo { get; set; } = string.Empty;
        /// <summary>
        /// Abbreviation
        /// </summary>
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } = string.Empty;
        /// <summary>
        /// Alias
        /// </summary>
        [JsonPropertyName("alias")]
        public string Alias { get; set; } = string.Empty;
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdateTime { get; set; }
    }
}
