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
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>resolutionSource</c>"] Resolution source
        /// </summary>
        [JsonPropertyName("resolutionSource")]
        public string ResolutionSource { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>startDate</c>"] Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// ["<c>endDate</c>"] End date
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// ["<c>creationDate</c>"] Create time
        /// </summary>
        [JsonPropertyName("creationDate")]
        public DateTime CreateTime { get; set; }
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
        /// ["<c>liquidity</c>"] Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public decimal Liquidity { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>openInterest</c>"] Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// ["<c>sortBy</c>"] Sort by
        /// </summary>
        [JsonPropertyName("sortBy")]
        public string SortBy { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>category</c>"] Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>subcategory</c>"] Subcategory
        /// </summary>
        [JsonPropertyName("subcategory")]
        public string Subcategory { get; set; } = string.Empty;
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
        /// ["<c>published_at</c>"] Publish time
        /// </summary>
        [JsonPropertyName("published_at")]
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
        /// ["<c>updatedAt</c>"] End date
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>commentsEnabled</c>"] Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// ["<c>competitive</c>"] Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal Competitive { get; set; }
        /// <summary>
        /// ["<c>volume24hr</c>"] Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// ["<c>volume1wk</c>"] Volume 1 week
        /// </summary>
        [JsonPropertyName("volume1wk")]
        public decimal Volume1wk { get; set; }
        /// <summary>
        /// ["<c>volume1mo</c>"] Volume 1 month
        /// </summary>
        [JsonPropertyName("volume1mo")]
        public decimal Volume1mo { get; set; }
        /// <summary>
        /// ["<c>volume1yr</c>"] Volume 1 year
        /// </summary>
        [JsonPropertyName("volume1yr")]
        public decimal Volume1yr { get; set; }
        /// <summary>
        /// ["<c>featuredImage</c>"] Featured image
        /// </summary>
        [JsonPropertyName("featuredImage")]
        public string FeaturedImage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>disqusThread</c>"] Disqus thread
        /// </summary>
        [JsonPropertyName("disqusThread")]
        public string DisqusThread { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>parentEvent</c>"] Parent event
        /// </summary>
        [JsonPropertyName("parentEvent")]
        public string ParentEvent { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>enableOrderBook</c>"] Enabled order book
        /// </summary>
        [JsonPropertyName("enableOrderBook")]
        public bool EnableOrderBook { get; set; }
        /// <summary>
        /// ["<c>liquidityAmm</c>"] Liquidity AMM
        /// </summary>
        [JsonPropertyName("liquidityAmm")]
        public decimal LiquidityAmm { get; set; }
        /// <summary>
        /// ["<c>liquidityClob</c>"] Liquidity CLOB
        /// </summary>
        [JsonPropertyName("liquidityClob")]
        public decimal LiquidityClob { get; set; }
        /// <summary>
        /// ["<c>negRisk</c>"] Negative risk
        /// </summary>
        [JsonPropertyName("negRisk")]
        public bool NegativeRisk { get; set; }
        /// <summary>
        /// ["<c>negRiskMarketID</c>"] Negative risk market id
        /// </summary>
        [JsonPropertyName("negRiskMarketID")]
        public string NegativeRiskMarketId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>negRiskFeeBips</c>"] Negative risk fee bps
        /// </summary>
        [JsonPropertyName("negRiskFeeBips")]
        public decimal NegativeRiskFeeBips { get; set; }
        /// <summary>
        /// ["<c>commentCount</c>"] Number of comments
        /// </summary>
        [JsonPropertyName("commentCount")]
        public long CommentCount { get; set; }
        /// <summary>
        /// ["<c>cyom</c>"] Cyom
        /// </summary>
        [JsonPropertyName("cyom")]
        public bool Cyom { get; set; }
        /// <summary>
        /// ["<c>closedTime</c>"] Close time
        /// </summary>
        [JsonPropertyName("closedTime")]
        public DateTime? CloseTime { get; set; }
        /// <summary>
        /// ["<c>showAllOutcomes</c>"] Show all outcomes
        /// </summary>
        [JsonPropertyName("showAllOutcomes")]
        public bool ShowAllOutcomes { get; set; }
        /// <summary>
        /// ["<c>showMarketImages</c>"] Show market images
        /// </summary>
        [JsonPropertyName("showMarketImages")]
        public bool ShowMarketImages { get; set; }
        /// <summary>
        /// ["<c>automaticallyResolved</c>"] Automatically resolved
        /// </summary>
        [JsonPropertyName("automaticallyResolved")]
        public bool AutomaticallyResolved { get; set; }
        /// <summary>
        /// ["<c>enableNegRisk</c>"] Enable negative risk
        /// </summary>
        [JsonPropertyName("enableNegRisk")]
        public bool EnableNegativeRisk { get; set; }
        /// <summary>
        /// ["<c>automaticallyActive</c>"] Automatically active
        /// </summary>
        [JsonPropertyName("automaticallyActive")]
        public bool AutomaticallyActive { get; set; }
        /// <summary>
        /// ["<c>eventDate</c>"] Event date
        /// </summary>
        [JsonPropertyName("eventDate")]
        public DateTime? EventDate { get; set; }
        /// <summary>
        /// ["<c>startTime</c>"] Start time
        /// </summary>
        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// ["<c>eventWeek</c>"] Event week
        /// </summary>
        [JsonPropertyName("eventWeek")]
        public int EventWeek { get; set; }
        /// <summary>
        /// ["<c>seriesSlug</c>"] Series slug
        /// </summary>
        [JsonPropertyName("seriesSlug")]
        public string SeriesSlug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>score</c>"] Score
        /// </summary>
        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>elapsed</c>"] Elapsed
        /// </summary>
        [JsonPropertyName("elapsed")]
        public string Elapsed { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>period</c>"] Period
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>live</c>"] Live
        /// </summary>
        [JsonPropertyName("live")]
        public bool Live { get; set; }
        /// <summary>
        /// ["<c>ended</c>"] Ended
        /// </summary>
        [JsonPropertyName("ended")]
        public bool Ended { get; set; }
        /// <summary>
        /// ["<c>finishedTimestamp</c>"] Finish timestamp
        /// </summary>
        [JsonPropertyName("finishedTimestamp")]
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// ["<c>gmpChartMode</c>"] Gmp chart mode
        /// </summary>
        [JsonPropertyName("gmpChartMode")]
        public string GmpChartMode { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tweetCount</c>"] Tweet count
        /// </summary>
        [JsonPropertyName("tweetCount")]
        public long TweetCount { get; set; }
        /// <summary>
        /// ["<c>featuredOrder</c>"] Featured order
        /// </summary>
        [JsonPropertyName("featuredOrder")]
        public long FeaturedOrder { get; set; }
        /// <summary>
        /// ["<c>estimateValue</c>"] Estimate value
        /// </summary>
        [JsonPropertyName("estimateValue")]
        public bool EstimateValue { get; set; }
        /// <summary>
        /// ["<c>cantEstimate</c>"] Cant estimate
        /// </summary>
        [JsonPropertyName("cantEstimate")]
        public bool CantEstimate { get; set; }
        /// <summary>
        /// ["<c>estimatedValue</c>"] Estimated value
        /// </summary>
        [JsonPropertyName("estimatedValue")]
        public string EstimatedValue { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>spreadsMainLine</c>"] Spreads main line
        /// </summary>
        [JsonPropertyName("spreadsMainLine")]
        public decimal SpreadsMainLine { get; set; }
        /// <summary>
        /// ["<c>totalsMainLine</c>"] Totals main line
        /// </summary>
        [JsonPropertyName("totalsMainLine")]
        public decimal TotalsMainLine { get; set; }
        /// <summary>
        /// ["<c>carouselMap</c>"] Carousel map
        /// </summary>
        [JsonPropertyName("carouselMap")]
        public string CarouselMap { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pendingDeployment</c>"] Pending deployment
        /// </summary>
        [JsonPropertyName("pendingDeployment")]
        public bool PendingDeployment { get; set; }
        /// <summary>
        /// ["<c>deploying</c>"] Deploying
        /// </summary>
        [JsonPropertyName("deploying")]
        public bool Deploying { get; set; }
        /// <summary>
        /// ["<c>deployingTimestamp</c>"] Deploy timestamp
        /// </summary>
        [JsonPropertyName("deployingTimestamp")]
        public DateTime? DeployTime { get; set; }
        /// <summary>
        /// ["<c>scheduledDeploymentTimestamp</c>"] Scheduled deployment time
        /// </summary>
        [JsonPropertyName("scheduledDeploymentTimestamp")]
        public DateTime? ScheduledDeployTime { get; set; }
        /// <summary>
        /// ["<c>gameStatus</c>"] Game status
        /// </summary>
        [JsonPropertyName("gameStatus")]
        public string? GameStatus { get; set; }

        /// <summary>
        /// ["<c>imageOptimized</c>"] Optimized image reference
        /// </summary>
        [JsonPropertyName("imageOptimized")]
        public PolymarketImageRef? ImageOptimized { get; set; }

        /// <summary>
        /// ["<c>iconOptimized</c>"] Optimized icon reference
        /// </summary>
        [JsonPropertyName("iconOptimized")]
        public PolymarketImageRef? IconOptimized { get; set; }

        /// <summary>
        /// ["<c>featuredImageOptimized</c>"] Optimized featured image reference
        /// </summary>
        [JsonPropertyName("featuredImageOptimized")]
        public PolymarketImageRef? FeaturedImageOptimized { get; set; }
        /// <summary>
        /// ["<c>subEvents</c>"] Sub events
        /// </summary>
        [JsonPropertyName("subEvents")]
        public string[]? SubEvents { get; set; }
        /// <summary>
        /// ["<c>markets</c>"] Markets
        /// </summary>
        [JsonPropertyName("markets")]
        public PolymarketGammaMarket[]? Markets { get; set; }
        /// <summary>
        /// ["<c>series</c>"] Series
        /// </summary>
        [JsonPropertyName("series")]
        public PolymarketSeries[]? Series { get; set; }
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
        /// ["<c>eventCreators</c>"] Event creators
        /// </summary>
        [JsonPropertyName("eventCreators")]
        public PolymarketCreator[]? EventCreators { get; set; }
        /// <summary>
        /// ["<c>chats</c>"] Chats
        /// </summary>
        [JsonPropertyName("chats")]
        public PolymarketChat[]? Chats { get; set; }
        /// <summary>
        /// ["<c>templates</c>"] Templates
        /// </summary>
        [JsonPropertyName("templates")]
        public PolymarketTemplate[]? Templates { get; set; } 
    }
}
