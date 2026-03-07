using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Tag info
    /// </summary>
    public record PolymarketTag
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>label</c>"] Label
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>slug</c>"] Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>forceShow</c>"] Force show
        /// </summary>
        [JsonPropertyName("forceShow")]
        public bool ForceShow { get; set; }
        /// <summary>
        /// ["<c>forceHide</c>"] Force hide
        /// </summary>
        [JsonPropertyName("forceHide")]
        public bool ForceHide { get; set; }
        /// <summary>
        /// ["<c>isCarousel</c>"] Is carousel
        /// </summary>
        [JsonPropertyName("isCarousel")]
        public bool IsCarousel { get; set; }
        /// <summary>
        /// ["<c>publishedAt</c>"] Publish time
        /// </summary>
        [JsonPropertyName("publishedAt")]
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// ["<c>createdAt</c>"] Create time
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updatedAt</c>"] Update time
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdateTime { get; set; }
    }
}
