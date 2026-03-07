using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Series collection
    /// </summary>
    public record PolymarketSeriesCollection
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ticker</c>"] Ticker
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>slug</c>"] Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>subtitle</c>"] Subtitle
        /// </summary>
        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>collectionType</c>"] Collection type
        /// </summary>
        [JsonPropertyName("collectionType")]
        public string CollectionType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tags</c>"] Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public string Tags { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>image</c>"] Image
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>icon</c>"] Icon
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>headerImage</c>"] Header image
        /// </summary>
        [JsonPropertyName("headerImage")]
        public string HeaderImage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>layout</c>"] Layout
        /// </summary>
        [JsonPropertyName("layout")]
        public string Layout { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>active</c>"] Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        /// <summary>
        /// ["<c>archived</c>"] Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// ["<c>new</c>"] New
        /// </summary>
        [JsonPropertyName("new")]
        public bool New { get; set; }
        /// <summary>
        /// ["<c>featured</c>"] Featured
        /// </summary>
        [JsonPropertyName("featured")]
        public bool Featured { get; set; }
        /// <summary>
        /// ["<c>restricted</c>"] Restricted
        /// </summary>
        [JsonPropertyName("restricted")]
        public bool Restricted { get; set; }
        /// <summary>
        /// ["<c>isTemplate</c>"] Is template
        /// </summary>
        [JsonPropertyName("isTemplate")]
        public bool IsTemplate { get; set; }
        /// <summary>
        /// ["<c>templateVariables</c>"] Template variables
        /// </summary>
        [JsonPropertyName("templateVariables")]
        public string TemplateVariables { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>publishedAt</c>"] Publish time
        /// </summary>
        [JsonPropertyName("publishedAt")]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// ["<c>createdBy</c>"] Created by
        /// </summary>
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>updatedBy</c>"] Updated by
        /// </summary>
        [JsonPropertyName("updatedBy")]
        public string UpdatedBy { get; set; } = string.Empty;
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
        /// <summary>
        /// ["<c>commentsEnabled</c>"] Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// ["<c>imageOptimized</c>"] Image optimized
        /// </summary>
        [JsonPropertyName("imageOptimized")]
        public PolymarketImageRef? ImageOptimized { get; set; } = null!;
        /// <summary>
        /// ["<c>iconOptimized</c>"] Icon optimized
        /// </summary>
        [JsonPropertyName("iconOptimized")]
        public PolymarketImageRef? IconOptimized { get; set; } = null!;
        /// <summary>
        /// ["<c>headerImageOptimized</c>"] Header image optimized
        /// </summary>
        [JsonPropertyName("headerImageOptimized")]
        public PolymarketImageRef? HeaderImageOptimized { get; set; } = null!;
    }
}
