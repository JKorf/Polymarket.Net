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
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>league</c>"] League
        /// </summary>
        [JsonPropertyName("league")]
        public string League { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>record</c>"] Record
        /// </summary>
        [JsonPropertyName("record")]
        public string Record { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>logo</c>"] Logo
        /// </summary>
        [JsonPropertyName("logo")]
        public string Logo { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>abbreviation</c>"] Abbreviation
        /// </summary>
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>alias</c>"] Alias
        /// </summary>
        [JsonPropertyName("alias")]
        public string Alias { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>createdAt</c>"] Create time
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updatedAt</c>"] Update time
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdateTime { get; set; }
    }
}
