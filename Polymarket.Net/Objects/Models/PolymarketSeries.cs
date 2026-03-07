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
        /// ["<c>seriesType</c>"] Series type
        /// </summary>
        [JsonPropertyName("seriesType")]
        public string SeriesType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>recurrence</c>"] Recurrence
        /// </summary>
        [JsonPropertyName("recurrence")]
        public string Recurrence { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
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
        public bool TemplateVariables { get; set; }
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
        /// ["<c>competitive</c>"] Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal? Competitive { get; set; }
        /// <summary>
        /// ["<c>volume24hr</c>"] Volume24hr
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>liquidity</c>"] Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public decimal Liquidity { get; set; }
        /// <summary>
        /// ["<c>startDate</c>"] Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// ["<c>pythTokenID</c>"] Pyth token id
        /// </summary>
        [JsonPropertyName("pythTokenID")]
        public string PythTokenID { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cgAssetName</c>"] Cg asset name
        /// </summary>
        [JsonPropertyName("cgAssetName")]
        public string CgAssetName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>score</c>"] Score
        /// </summary>
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
        /// <summary>
        /// ["<c>events</c>"] Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[]? Events { get; set; }
        /// <summary>
        /// ["<c>collections</c>"] Collections
        /// </summary>
        [JsonPropertyName("collections")]
        public PolymarketSeriesCollection[]? Collections { get; set; }
        /// <summary>
        /// ["<c>categories</c>"] Categories
        /// </summary>
        [JsonPropertyName("categories")]
        public PolymarketMarketCategory[]? Categories { get; set; }
        /// <summary>
        /// ["<c>tags</c>"] Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public PolymarketTag[]? Tags { get; set; }
        /// <summary>
        /// ["<c>commentCount</c>"] Comment count
        /// </summary>
        [JsonPropertyName("commentCount")]
        public decimal CommentCount { get; set; }
        /// <summary>
        /// ["<c>chats</c>"] Chats
        /// </summary>
        [JsonPropertyName("chats")]
        public PolymarketChat[]? Chats { get; set; }
    }
}
