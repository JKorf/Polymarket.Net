using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Series info
    /// </summary>
    public record PolymarketSeries
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Ticker
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = string.Empty;
        /// <summary>
        /// Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Subtitle
        /// </summary>
        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; } = string.Empty;
        /// <summary>
        /// Series type
        /// </summary>
        [JsonPropertyName("seriesType")]
        public string SeriesType { get; set; } = string.Empty;
        /// <summary>
        /// Recurrence
        /// </summary>
        [JsonPropertyName("recurrence")]
        public string Recurrence { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Image
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
        /// <summary>
        /// Icon
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// Layout
        /// </summary>
        [JsonPropertyName("layout")]
        public string Layout { get; set; } = string.Empty;
        /// <summary>
        /// Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        /// <summary>
        /// Closed
        /// </summary>
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        /// <summary>
        /// Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// New
        /// </summary>
        [JsonPropertyName("new")]
        public bool New { get; set; }
        /// <summary>
        /// Featured
        /// </summary>
        [JsonPropertyName("featured")]
        public bool Featured { get; set; }
        /// <summary>
        /// Restricted
        /// </summary>
        [JsonPropertyName("restricted")]
        public bool Restricted { get; set; }
        /// <summary>
        /// Is template
        /// </summary>
        [JsonPropertyName("isTemplate")]
        public bool IsTemplate { get; set; }
        /// <summary>
        /// Template variables
        /// </summary>
        [JsonPropertyName("templateVariables")]
        public bool TemplateVariables { get; set; }
        /// <summary>
        /// Publish time
        /// </summary>
        [JsonPropertyName("publishedAt")]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// Created by
        /// </summary>
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;
        /// <summary>
        /// Updated by
        /// </summary>
        [JsonPropertyName("updatedBy")]
        public string UpdatedBy { get; set; } = string.Empty;
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
        /// <summary>
        /// Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal? Competitive { get; set; }
        /// <summary>
        /// Volume24hr
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public decimal Liquidity { get; set; }
        /// <summary>
        /// Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Pyth token id
        /// </summary>
        [JsonPropertyName("pythTokenID")]
        public string PythTokenID { get; set; } = string.Empty;
        /// <summary>
        /// Cg asset name
        /// </summary>
        [JsonPropertyName("cgAssetName")]
        public string CgAssetName { get; set; } = string.Empty;
        /// <summary>
        /// Score
        /// </summary>
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
        /// <summary>
        /// Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[]? Events { get; set; }
        /// <summary>
        /// Collections
        /// </summary>
        [JsonPropertyName("collections")]
        public PolymarketSeriesCollection[]? Collections { get; set; }
        /// <summary>
        /// Categories
        /// </summary>
        [JsonPropertyName("categories")]
        public PolymarketMarketCategory[]? Categories { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public PolymarketTag[]? Tags { get; set; }
        /// <summary>
        /// Comment count
        /// </summary>
        [JsonPropertyName("commentCount")]
        public decimal CommentCount { get; set; }
        /// <summary>
        /// Chats
        /// </summary>
        [JsonPropertyName("chats")]
        public PolymarketChat[]? Chats { get; set; }
    }
}
