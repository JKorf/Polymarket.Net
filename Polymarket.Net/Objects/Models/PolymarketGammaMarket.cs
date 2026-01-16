using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Gamma API market info
    /// </summary>
    public record PolymarketGammaMarket
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Question
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// Condition id
        /// </summary>
        [JsonPropertyName("conditionId")]
        public string ConditionId { get; set; } = string.Empty;
        /// <summary>
        /// Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// Twitter card image
        /// </summary>
        [JsonPropertyName("twitterCardImage")]
        public string TwitterCardImage { get; set; } = string.Empty;
        /// <summary>
        /// Resolution source
        /// </summary>
        [JsonPropertyName("resolutionSource")]
        public string ResolutionSource { get; set; } = string.Empty;
        /// <summary>
        /// End date
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// Amm type
        /// </summary>
        [JsonPropertyName("ammType")]
        public string AmmType { get; set; } = string.Empty;
        /// <summary>
        /// Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public string Liquidity { get; set; } = string.Empty;
        /// <summary>
        /// Sponsor name
        /// </summary>
        [JsonPropertyName("sponsorName")]
        public string? SponsorName { get; set; }
        /// <summary>
        /// Sponsor image
        /// </summary>
        [JsonPropertyName("sponsorImage")]
        public string? SponsorImage { get; set; }
        /// <summary>
        /// Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// X axis value
        /// </summary>
        [JsonPropertyName("xAxisValue")]
        public string XAxisValue { get; set; } = string.Empty;
        /// <summary>
        /// Y axis value
        /// </summary>
        [JsonPropertyName("yAxisValue")]
        public string YAxisValue { get; set; } = string.Empty;
        /// <summary>
        /// Denomination token
        /// </summary>
        [JsonPropertyName("denominationToken")]
        public string DenominationToken { get; set; } = string.Empty;
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public string Fee { get; set; } = string.Empty;
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
        /// Lower bound
        /// </summary>
        [JsonPropertyName("lowerBound")]
        public string LowerBound { get; set; } = string.Empty;
        /// <summary>
        /// Upper bound
        /// </summary>
        [JsonPropertyName("upperBound")]
        public string UpperBound { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Outcomes
        /// </summary>
        [JsonPropertyName("outcomes")]
        [JsonConverter(typeof(ObjectStringConverter<string[]>))]
        public string[]? Outcomes { get; set; }
        /// <summary>
        /// Outcome prices
        /// </summary>
        [JsonPropertyName("outcomePrices")]
        [JsonConverter(typeof(ObjectStringConverter<decimal[]>))]
        public decimal[]? OutcomePrices { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public string Volume { get; set; } = string.Empty;
        /// <summary>
        /// Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        /// <summary>
        /// Market type
        /// </summary>
        [JsonPropertyName("marketType")]
        public string MarketType { get; set; } = string.Empty;
        /// <summary>
        /// Format type
        /// </summary>
        [JsonPropertyName("formatType")]
        public string FormatType { get; set; } = string.Empty;
        /// <summary>
        /// Lower bound date
        /// </summary>
        [JsonPropertyName("lowerBoundDate")]
        public string LowerBoundDate { get; set; } = string.Empty;
        /// <summary>
        /// Upper bound date
        /// </summary>
        [JsonPropertyName("upperBoundDate")]
        public string UpperBoundDate { get; set; } = string.Empty;
        /// <summary>
        /// Closed
        /// </summary>
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        /// <summary>
        /// Market maker address
        /// </summary>
        [JsonPropertyName("marketMakerAddress")]
        public string MarketMakerAddress { get; set; } = string.Empty;
        /// <summary>
        /// Created by
        /// </summary>
        [JsonPropertyName("createdBy")]
        public decimal CreatedBy { get; set; }
        /// <summary>
        /// Updated by
        /// </summary>
        [JsonPropertyName("updatedBy")]
        public decimal UpdatedBy { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonPropertyName("closedTime")]
        public DateTime? CloseTime { get; set; }
        /// <summary>
        /// Wide format
        /// </summary>
        [JsonPropertyName("wideFormat")]
        public bool WideFormat { get; set; }
        /// <summary>
        /// New
        /// </summary>
        [JsonPropertyName("new")]
        public bool New { get; set; }
        /// <summary>
        /// Mailchimp tag
        /// </summary>
        [JsonPropertyName("mailchimpTag")]
        public string MailchimpTag { get; set; } = string.Empty;
        /// <summary>
        /// Featured
        /// </summary>
        [JsonPropertyName("featured")]
        public bool Featured { get; set; }
        /// <summary>
        /// Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// Resolved by
        /// </summary>
        [JsonPropertyName("resolvedBy")]
        public string ResolvedBy { get; set; } = string.Empty;
        /// <summary>
        /// Restricted
        /// </summary>
        [JsonPropertyName("restricted")]
        public bool Restricted { get; set; }
        /// <summary>
        /// Market group
        /// </summary>
        [JsonPropertyName("marketGroup")]
        public decimal MarketGroup { get; set; }
        /// <summary>
        /// Group item title
        /// </summary>
        [JsonPropertyName("groupItemTitle")]
        public string GroupItemTitle { get; set; } = string.Empty;
        /// <summary>
        /// Group item threshold
        /// </summary>
        [JsonPropertyName("groupItemThreshold")]
        public string GroupItemThreshold { get; set; } = string.Empty;
        /// <summary>
        /// Question id
        /// </summary>
        [JsonPropertyName("questionID")]
        public string QuestionId { get; set; } = string.Empty;
        /// <summary>
        /// Uma end date
        /// </summary>
        [JsonPropertyName("umaEndDate")]
        public DateTime? UmaEndDate { get; set; }
        /// <summary>
        /// Enable order book
        /// </summary>
        [JsonPropertyName("enableOrderBook")]
        public bool EnableOrderBook { get; set; }
        /// <summary>
        /// Order price min tick quantity
        /// </summary>
        [JsonPropertyName("orderPriceMinTickSize")]
        public decimal OrderPriceMinTickQuantity { get; set; }
        /// <summary>
        /// Order min quantity
        /// </summary>
        [JsonPropertyName("orderMinSize")]
        public decimal OrderMinQuantity { get; set; }
        /// <summary>
        /// Uma resolution status
        /// </summary>
        [JsonPropertyName("umaResolutionStatus")]
        public string UmaResolutionStatus { get; set; } = string.Empty;
        /// <summary>
        /// Curation order
        /// </summary>
        [JsonPropertyName("curationOrder")]
        public decimal CurationOrder { get; set; }
        /// <summary>
        /// Volume num
        /// </summary>
        [JsonPropertyName("volumeNum")]
        public decimal VolumeNum { get; set; }
        /// <summary>
        /// Liquidity num
        /// </summary>
        [JsonPropertyName("liquidityNum")]
        public decimal LiquidityNum { get; set; }
        /// <summary>
        /// End date iso
        /// </summary>
        [JsonPropertyName("endDateIso")]
        public DateTime? EndDateIso { get; set; }
        /// <summary>
        /// Start date iso
        /// </summary>
        [JsonPropertyName("startDateIso")]
        public DateTime StartDateIso { get; set; }
        /// <summary>
        /// Uma end date iso
        /// </summary>
        [JsonPropertyName("umaEndDateIso")]
        public DateTime? UmaEndDateIso { get; set; }
        /// <summary>
        /// Has reviewed dates
        /// </summary>
        [JsonPropertyName("hasReviewedDates")]
        public bool HasReviewedDates { get; set; }
        /// <summary>
        /// Ready for cron
        /// </summary>
        [JsonPropertyName("readyForCron")]
        public bool ReadyForCron { get; set; }
        /// <summary>
        /// Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// Volume24hr
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// Volume1wk
        /// </summary>
        [JsonPropertyName("volume1wk")]
        public decimal Volume1wk { get; set; }
        /// <summary>
        /// Volume1mo
        /// </summary>
        [JsonPropertyName("volume1mo")]
        public decimal Volume1mo { get; set; }
        /// <summary>
        /// Volume1yr
        /// </summary>
        [JsonPropertyName("volume1yr")]
        public decimal Volume1yr { get; set; }
        /// <summary>
        /// Game start time
        /// </summary>
        [JsonPropertyName("gameStartTime")]
        public DateTime? GameStartTime { get; set; }
        /// <summary>
        /// Seconds delay
        /// </summary>
        [JsonPropertyName("secondsDelay")]
        public decimal SecondsDelay { get; set; }
        /// <summary>
        /// Clob token ids
        /// </summary>
        [JsonPropertyName("clobTokenIds")]
        [JsonConverter(typeof(ObjectStringConverter<string[]>))]
        public string[]? ClobTokenIds { get; set; }
        /// <summary>
        /// Disqus thread
        /// </summary>
        [JsonPropertyName("disqusThread")]
        public string DisqusThread { get; set; } = string.Empty;
        /// <summary>
        /// Short outcomes
        /// </summary>
        [JsonPropertyName("shortOutcomes")]
        public string ShortOutcomes { get; set; } = string.Empty;
        /// <summary>
        /// Team aid
        /// </summary>
        [JsonPropertyName("teamAID")]
        public string TeamAID { get; set; } = string.Empty;
        /// <summary>
        /// Team bid
        /// </summary>
        [JsonPropertyName("teamBID")]
        public string TeamBID { get; set; } = string.Empty;
        /// <summary>
        /// Uma bond
        /// </summary>
        [JsonPropertyName("umaBond")]
        public string UmaBond { get; set; } = string.Empty;
        /// <summary>
        /// Uma reward
        /// </summary>
        [JsonPropertyName("umaReward")]
        public string UmaReward { get; set; } = string.Empty;
        /// <summary>
        /// Fpmm live
        /// </summary>
        [JsonPropertyName("fpmmLive")]
        public bool FpmmLive { get; set; }
        /// <summary>
        /// Volume 24 hour amm
        /// </summary>
        [JsonPropertyName("volume24hrAmm")]
        public decimal Volume24hrAmm { get; set; }
        /// <summary>
        /// Volume 1week amm
        /// </summary>
        [JsonPropertyName("volume1wkAmm")]
        public decimal Volume1wkAmm { get; set; }
        /// <summary>
        /// Volume 1month amm
        /// </summary>
        [JsonPropertyName("volume1moAmm")]
        public decimal Volume1moAmm { get; set; }
        /// <summary>
        /// Volume 1year amm
        /// </summary>
        [JsonPropertyName("volume1yrAmm")]
        public decimal Volume1yrAmm { get; set; }
        /// <summary>
        /// Volume 24hour clob
        /// </summary>
        [JsonPropertyName("volume24hrClob")]
        public decimal Volume24hrClob { get; set; }
        /// <summary>
        /// Volume 1 week clob
        /// </summary>
        [JsonPropertyName("volume1wkClob")]
        public decimal Volume1wkClob { get; set; }
        /// <summary>
        /// Volume 1 month clob
        /// </summary>
        [JsonPropertyName("volume1moClob")]
        public decimal Volume1moClob { get; set; }
        /// <summary>
        /// Volume 1year clob
        /// </summary>
        [JsonPropertyName("volume1yrClob")]
        public decimal Volume1yrClob { get; set; }
        /// <summary>
        /// Volume amm
        /// </summary>
        [JsonPropertyName("volumeAmm")]
        public decimal VolumeAmm { get; set; }
        /// <summary>
        /// Volume clob
        /// </summary>
        [JsonPropertyName("volumeClob")]
        public decimal VolumeClob { get; set; }
        /// <summary>
        /// Liquidity amm
        /// </summary>
        [JsonPropertyName("liquidityAmm")]
        public decimal LiquidityAmm { get; set; }
        /// <summary>
        /// Liquidity clob
        /// </summary>
        [JsonPropertyName("liquidityClob")]
        public decimal LiquidityClob { get; set; }
        /// <summary>
        /// Maker base fee
        /// </summary>
        [JsonPropertyName("makerBaseFee")]
        public decimal MakerBaseFee { get; set; }
        /// <summary>
        /// Taker base fee
        /// </summary>
        [JsonPropertyName("takerBaseFee")]
        public decimal TakerBaseFee { get; set; }
        /// <summary>
        /// Custom liveness
        /// </summary>
        [JsonPropertyName("customLiveness")]
        public decimal CustomLiveness { get; set; }
        /// <summary>
        /// Accepting orders
        /// </summary>
        [JsonPropertyName("acceptingOrders")]
        public bool AcceptingOrders { get; set; }
        /// <summary>
        /// Notifications enabled
        /// </summary>
        [JsonPropertyName("notificationsEnabled")]
        public bool NotificationsEnabled { get; set; }
        /// <summary>
        /// Score
        /// </summary>
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
        /// <summary>
        /// Image optimized
        /// </summary>
        [JsonPropertyName("imageOptimized")]
        public PolymarketImageRef? ImageOptimized { get; set; } = null!;
        /// <summary>
        /// Icon optimized
        /// </summary>
        [JsonPropertyName("iconOptimized")]
        public PolymarketImageRef? IconOptimized { get; set; } = null!;
        /// <summary>
        /// Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[]? Events { get; set; }
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
        /// Creator
        /// </summary>
        [JsonPropertyName("creator")]
        public string Creator { get; set; } = string.Empty;
        /// <summary>
        /// Ready
        /// </summary>
        [JsonPropertyName("ready")]
        public bool Ready { get; set; }
        /// <summary>
        /// Funded
        /// </summary>
        [JsonPropertyName("funded")]
        public bool Funded { get; set; }
        /// <summary>
        /// Past slugs
        /// </summary>
        [JsonPropertyName("pastSlugs")]
        public string PastSlugs { get; set; } = string.Empty;
        /// <summary>
        /// Ready time
        /// </summary>
        [JsonPropertyName("readyTimestamp")]
        public DateTime? ReadyTime { get; set; }
        /// <summary>
        /// Funded time
        /// </summary>
        [JsonPropertyName("fundedTimestamp")]
        public DateTime? FundTime { get; set; }
        /// <summary>
        /// Accepting orders time
        /// </summary>
        [JsonPropertyName("acceptingOrdersTimestamp")]
        public DateTime? AcceptingOrdersTime { get; set; }
        /// <summary>
        /// Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal Competitive { get; set; }
        /// <summary>
        /// Rewards min quantity
        /// </summary>
        [JsonPropertyName("rewardsMinSize")]
        public decimal RewardsMinQuantity { get; set; }
        /// <summary>
        /// Rewards max spread
        /// </summary>
        [JsonPropertyName("rewardsMaxSpread")]
        public decimal RewardsMaxSpread { get; set; }
        /// <summary>
        /// Spread
        /// </summary>
        [JsonPropertyName("spread")]
        public decimal Spread { get; set; }
        /// <summary>
        /// Automatically resolved
        /// </summary>
        [JsonPropertyName("automaticallyResolved")]
        public bool AutomaticallyResolved { get; set; }
        /// <summary>
        /// One day price change
        /// </summary>
        [JsonPropertyName("oneDayPriceChange")]
        public decimal OneDayPriceChange { get; set; }
        /// <summary>
        /// One hour price change
        /// </summary>
        [JsonPropertyName("oneHourPriceChange")]
        public decimal OneHourPriceChange { get; set; }
        /// <summary>
        /// One week price change
        /// </summary>
        [JsonPropertyName("oneWeekPriceChange")]
        public decimal OneWeekPriceChange { get; set; }
        /// <summary>
        /// One month price change
        /// </summary>
        [JsonPropertyName("oneMonthPriceChange")]
        public decimal OneMonthPriceChange { get; set; }
        /// <summary>
        /// One year price change
        /// </summary>
        [JsonPropertyName("oneYearPriceChange")]
        public decimal OneYearPriceChange { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("lastTradePrice")]
        public decimal LastTradePrice { get; set; }
        /// <summary>
        /// Best bid
        /// </summary>
        [JsonPropertyName("bestBid")]
        public decimal BestBid { get; set; }
        /// <summary>
        /// Best ask
        /// </summary>
        [JsonPropertyName("bestAsk")]
        public decimal BestAsk { get; set; }
        /// <summary>
        /// Automatically active
        /// </summary>
        [JsonPropertyName("automaticallyActive")]
        public bool AutomaticallyActive { get; set; }
        /// <summary>
        /// Clear book on start
        /// </summary>
        [JsonPropertyName("clearBookOnStart")]
        public bool ClearBookOnStart { get; set; }
        /// <summary>
        /// Chart color
        /// </summary>
        [JsonPropertyName("chartColor")]
        public string ChartColor { get; set; } = string.Empty;
        /// <summary>
        /// Series color
        /// </summary>
        [JsonPropertyName("seriesColor")]
        public string SeriesColor { get; set; } = string.Empty;
        /// <summary>
        /// Show gmp series
        /// </summary>
        [JsonPropertyName("showGmpSeries")]
        public bool ShowGmpSeries { get; set; }
        /// <summary>
        /// Show gmp outcome
        /// </summary>
        [JsonPropertyName("showGmpOutcome")]
        public bool ShowGmpOutcome { get; set; }
        /// <summary>
        /// Manual activation
        /// </summary>
        [JsonPropertyName("manualActivation")]
        public bool ManualActivation { get; set; }
        /// <summary>
        /// Negative risk other
        /// </summary>
        [JsonPropertyName("negRiskOther")]
        public bool NegativeRiskOther { get; set; }
        /// <summary>
        /// Game id
        /// </summary>
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;
        /// <summary>
        /// Group item range
        /// </summary>
        [JsonPropertyName("groupItemRange")]
        public string GroupItemRange { get; set; } = string.Empty;
        /// <summary>
        /// Sports market type
        /// </summary>
        [JsonPropertyName("sportsMarketType")]
        public string SportsMarketType { get; set; } = string.Empty;
        /// <summary>
        /// Line
        /// </summary>
        [JsonPropertyName("line")]
        public decimal Line { get; set; }
        /// <summary>
        /// Uma resolution statuses
        /// </summary>
        [JsonPropertyName("umaResolutionStatuses")]
        public string? UmaResolutionStatuses { get; set; }
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
        /// Deploying time
        /// </summary>
        [JsonPropertyName("deployingTimestamp")]
        public DateTime? DeployTime { get; set; }
        /// <summary>
        /// Scheduled deployment time
        /// </summary>
        [JsonPropertyName("scheduledDeploymentTimestamp")]
        public DateTime? ScheduledDeploymentTime { get; set; }
        /// <summary>
        /// Rfq enabled
        /// </summary>
        [JsonPropertyName("rfqEnabled")]
        public bool RfqEnabled { get; set; }
        /// <summary>
        /// Event start time
        /// </summary>
        [JsonPropertyName("eventStartTime")]
        public DateTime? EventStartTime { get; set; }
    }
}
