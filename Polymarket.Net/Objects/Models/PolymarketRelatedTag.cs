using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Related tags
    /// </summary>
    public record PolymarketRelatedTag
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tagID</c>"] Tag id
        /// </summary>
        [JsonPropertyName("tagID")]
        public long TagId { get; set; }
        /// <summary>
        /// ["<c>relatedTagID</c>"] Related tag id
        /// </summary>
        [JsonPropertyName("relatedTagID")]
        public long RelatedTagId { get; set; }
        /// <summary>
        /// ["<c>rank</c>"] Rank
        /// </summary>
        [JsonPropertyName("rank")]
        public long Rank { get; set; }
    }
}
