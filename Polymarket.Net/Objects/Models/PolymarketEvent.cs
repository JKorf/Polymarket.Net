using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Event info
    /// </summary>
    public record PolymarketEvent
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
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Resolution source
        /// </summary>
        [JsonPropertyName("resolutionSource")]
        public string ResolutionSource { get; set; } = string.Empty;
        /// <summary>
        /// Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// End date
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("creationDate")]
        public DateTime CreateTime { get; set; }
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
        /// Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public decimal Liquidity { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// Sort by
        /// </summary>
        [JsonPropertyName("sortBy")]
        public string SortBy { get; set; } = string.Empty;
        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// Subcategory
        /// </summary>
        [JsonPropertyName("subcategory")]
        public string Subcategory { get; set; } = string.Empty;
        /// <summary>
        /// Is template
        /// </summary>
        [JsonPropertyName("isTemplate")]
        public bool IsTemplate { get; set; }
        /// <summary>
        /// Template variables
        /// </summary>
        [JsonPropertyName("templateVariables")]
        public string TemplateVariables { get; set; } = string.Empty;
        /// <summary>
        /// Publish time
        /// </summary>
        [JsonPropertyName("published_at")]
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
        /// End date
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal Competitive { get; set; }
        /// <summary>
        /// Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// Volume 1 week
        /// </summary>
        [JsonPropertyName("volume1wk")]
        public decimal Volume1wk { get; set; }
        /// <summary>
        /// Volume 1 month
        /// </summary>
        [JsonPropertyName("volume1mo")]
        public decimal Volume1mo { get; set; }
        /// <summary>
        /// Volume 1 year
        /// </summary>
        [JsonPropertyName("volume1yr")]
        public decimal Volume1yr { get; set; }
        /// <summary>
        /// Featured image
        /// </summary>
        [JsonPropertyName("featuredImage")]
        public string FeaturedImage { get; set; } = string.Empty;
        /// <summary>
        /// Disqus thread
        /// </summary>
        [JsonPropertyName("disqusThread")]
        public string DisqusThread { get; set; } = string.Empty;
        /// <summary>
        /// Parent event
        /// </summary>
        [JsonPropertyName("parentEvent")]
        public string ParentEvent { get; set; } = string.Empty;
        /// <summary>
        /// Enabled order book
        /// </summary>
        [JsonPropertyName("enableOrderBook")]
        public bool EnableOrderBook { get; set; }
        /// <summary>
        /// Liquidity AMM
        /// </summary>
        [JsonPropertyName("liquidityAmm")]
        public decimal LiquidityAmm { get; set; }
        /// <summary>
        /// Liquidity CLOB
        /// </summary>
        [JsonPropertyName("liquidityClob")]
        public decimal LiquidityClob { get; set; }
        /// <summary>
        /// Negative risk
        /// </summary>
        [JsonPropertyName("negRisk")]
        public bool NegativeRisk { get; set; }
        /// <summary>
        /// Negative risk market id
        /// </summary>
        [JsonPropertyName("negRiskMarketID")]
        public string NegativeRiskMarketId { get; set; } = string.Empty;
        /// <summary>
        /// Negative risk fee bps
        /// </summary>
        [JsonPropertyName("negRiskFeeBips")]
        public decimal NegativeRiskFeeBips { get; set; }
        /// <summary>
        /// Number of comments
        /// </summary>
        [JsonPropertyName("commentCount")]
        public long CommentCount { get; set; }
        /// <summary>
        /// Cyom
        /// </summary>
        [JsonPropertyName("cyom")]
        public bool Cyom { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonPropertyName("closedTime")]
        public DateTime? CloseTime { get; set; }
        /// <summary>
        /// Show all outcomes
        /// </summary>
        [JsonPropertyName("showAllOutcomes")]
        public bool ShowAllOutcomes { get; set; }
        /// <summary>
        /// Show market images
        /// </summary>
        [JsonPropertyName("showMarketImages")]
        public bool ShowMarketImages { get; set; }
        /// <summary>
        /// Automatically resolved
        /// </summary>
        [JsonPropertyName("automaticallyResolved")]
        public bool AutomaticallyResolved { get; set; }
        /// <summary>
        /// Enable negative risk
        /// </summary>
        [JsonPropertyName("enableNegRisk")]
        public bool EnableNegativeRisk { get; set; }
        /// <summary>
        /// Automatically active
        /// </summary>
        [JsonPropertyName("automaticallyActive")]
        public bool AutomaticallyActive { get; set; }
        /// <summary>
        /// Event date
        /// </summary>
        [JsonPropertyName("eventDate")]
        public DateTime? EventDate { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Event week
        /// </summary>
        [JsonPropertyName("eventWeek")]
        public int EventWeek { get; set; }
        /// <summary>
        /// Series slug
        /// </summary>
        [JsonPropertyName("seriesSlug")]
        public string SeriesSlug { get; set; } = string.Empty;
        /// <summary>
        /// Score
        /// </summary>
        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;
        /// <summary>
        /// Elapsed
        /// </summary>
        [JsonPropertyName("elapsed")]
        public string Elapsed { get; set; } = string.Empty;
        /// <summary>
        /// Period
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;
        /// <summary>
        /// Live
        /// </summary>
        [JsonPropertyName("live")]
        public bool Live { get; set; }
        /// <summary>
        /// Ended
        /// </summary>
        [JsonPropertyName("ended")]
        public bool Ended { get; set; }
        /// <summary>
        /// Finish timestamp
        /// </summary>
        [JsonPropertyName("finishedTimestamp")]
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// Gmp chart mode
        /// </summary>
        [JsonPropertyName("gmpChartMode")]
        public string GmpChartMode { get; set; } = string.Empty;
        /// <summary>
        /// Tweet count
        /// </summary>
        [JsonPropertyName("tweetCount")]
        public long TweetCount { get; set; }
        /// <summary>
        /// Featured order
        /// </summary>
        [JsonPropertyName("featuredOrder")]
        public long FeaturedOrder { get; set; }
        /// <summary>
        /// Estimate value
        /// </summary>
        [JsonPropertyName("estimateValue")]
        public bool EstimateValue { get; set; }
        /// <summary>
        /// Cant estimate
        /// </summary>
        [JsonPropertyName("cantEstimate")]
        public bool CantEstimate { get; set; }
        /// <summary>
        /// Estimated value
        /// </summary>
        [JsonPropertyName("estimatedValue")]
        public string EstimatedValue { get; set; } = string.Empty;
        /// <summary>
        /// Spreads main line
        /// </summary>
        [JsonPropertyName("spreadsMainLine")]
        public decimal SpreadsMainLine { get; set; }
        /// <summary>
        /// Totals main line
        /// </summary>
        [JsonPropertyName("totalsMainLine")]
        public decimal TotalsMainLine { get; set; }
        /// <summary>
        /// Carousel map
        /// </summary>
        [JsonPropertyName("carouselMap")]
        public string CarouselMap { get; set; } = string.Empty;
        /// <summary>
        /// Pending deployment
        /// </summary>
        [JsonPropertyName("pendingDeployment")]
        public bool PendingDeployment { get; set; }
        /// <summary>
        /// Deploying
        /// </summary>
        [JsonPropertyName("deploying")]
        public bool Deploying { get; set; }
        /// <summary>
        /// Deploy timestamp
        /// </summary>
        [JsonPropertyName("deployingTimestamp")]
        public DateTime? DeployTime { get; set; }
        /// <summary>
        /// Scheduled deployment time
        /// </summary>
        [JsonPropertyName("scheduledDeploymentTimestamp")]
        public DateTime? ScheduledDeployTime { get; set; }
        /// <summary>
        /// Game status
        /// </summary>
        [JsonPropertyName("gameStatus")]
        public string? GameStatus { get; set; }

        /// <summary>
        /// Optimized image reference
        /// </summary>
        [JsonPropertyName("imageOptimized")]
        public PolymarketImageRef? ImageOptimized { get; set; }

        /// <summary>
        /// Optimized icon reference
        /// </summary>
        [JsonPropertyName("iconOptimized")]
        public PolymarketImageRef? IconOptimized { get; set; }

        /// <summary>
        /// Optimized featured image reference
        /// </summary>
        [JsonPropertyName("featuredImageOptimized")]
        public PolymarketImageRef? FeaturedImageOptimized { get; set; }
        /// <summary>
        /// Sub events
        /// </summary>
        [JsonPropertyName("subEvents")]
        public string[]? SubEvents { get; set; }
        /// <summary>
        /// Markets
        /// </summary>
        [JsonPropertyName("markets")]
        public PolymarketGammaMarket[]? Markets { get; set; }
        /// <summary>
        /// Series
        /// </summary>
        [JsonPropertyName("series")]
        public PolymarketSeries[]? Series { get; set; }
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
        /// Event creators
        /// </summary>
        [JsonPropertyName("eventCreators")]
        public PolymarketCreator[]? EventCreators { get; set; }
        /// <summary>
        /// Chats
        /// </summary>
        [JsonPropertyName("chats")]
        public PolymarketChat[]? Chats { get; set; }
        /// <summary>
        /// Templates
        /// </summary>
        [JsonPropertyName("templates")]
        public PolymarketTemplate[]? Templates { get; set; } 
    }
}
