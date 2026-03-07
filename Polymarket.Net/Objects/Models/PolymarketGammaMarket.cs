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
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>question</c>"] Question
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>conditionId</c>"] Condition id
        /// </summary>
        [JsonPropertyName("conditionId")]
        public string ConditionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>slug</c>"] Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>twitterCardImage</c>"] Twitter card image
        /// </summary>
        [JsonPropertyName("twitterCardImage")]
        public string TwitterCardImage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>resolutionSource</c>"] Resolution source
        /// </summary>
        [JsonPropertyName("resolutionSource")]
        public string ResolutionSource { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>endDate</c>"] End date
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ["<c>category</c>"] Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ammType</c>"] Amm type
        /// </summary>
        [JsonPropertyName("ammType")]
        public string AmmType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>liquidity</c>"] Liquidity
        /// </summary>
        [JsonPropertyName("liquidity")]
        public string Liquidity { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>sponsorName</c>"] Sponsor name
        /// </summary>
        [JsonPropertyName("sponsorName")]
        public string? SponsorName { get; set; }
        /// <summary>
        /// ["<c>sponsorImage</c>"] Sponsor image
        /// </summary>
        [JsonPropertyName("sponsorImage")]
        public string? SponsorImage { get; set; }
        /// <summary>
        /// ["<c>startDate</c>"] Start date
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// ["<c>xAxisValue</c>"] X axis value
        /// </summary>
        [JsonPropertyName("xAxisValue")]
        public string XAxisValue { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>yAxisValue</c>"] Y axis value
        /// </summary>
        [JsonPropertyName("yAxisValue")]
        public string YAxisValue { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>denominationToken</c>"] Denomination token
        /// </summary>
        [JsonPropertyName("denominationToken")]
        public string DenominationToken { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public string Fee { get; set; } = string.Empty;
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
        /// ["<c>lowerBound</c>"] Lower bound
        /// </summary>
        [JsonPropertyName("lowerBound")]
        public string LowerBound { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>upperBound</c>"] Upper bound
        /// </summary>
        [JsonPropertyName("upperBound")]
        public string UpperBound { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>outcomes</c>"] Outcomes
        /// </summary>
        [JsonPropertyName("outcomes")]
        [JsonConverter(typeof(ObjectStringConverter<string[]>))]
        public string[]? Outcomes { get; set; }
        /// <summary>
        /// ["<c>outcomePrices</c>"] Outcome prices
        /// </summary>
        [JsonPropertyName("outcomePrices")]
        [JsonConverter(typeof(ObjectStringConverter<decimal[]>))]
        public decimal[]? OutcomePrices { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public string Volume { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>active</c>"] Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        /// <summary>
        /// ["<c>marketType</c>"] Market type
        /// </summary>
        [JsonPropertyName("marketType")]
        public string MarketType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>formatType</c>"] Format type
        /// </summary>
        [JsonPropertyName("formatType")]
        public string FormatType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lowerBoundDate</c>"] Lower bound date
        /// </summary>
        [JsonPropertyName("lowerBoundDate")]
        public string LowerBoundDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>upperBoundDate</c>"] Upper bound date
        /// </summary>
        [JsonPropertyName("upperBoundDate")]
        public string UpperBoundDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        /// <summary>
        /// ["<c>marketMakerAddress</c>"] Market maker address
        /// </summary>
        [JsonPropertyName("marketMakerAddress")]
        public string MarketMakerAddress { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>createdBy</c>"] Created by
        /// </summary>
        [JsonPropertyName("createdBy")]
        public decimal CreatedBy { get; set; }
        /// <summary>
        /// ["<c>updatedBy</c>"] Updated by
        /// </summary>
        [JsonPropertyName("updatedBy")]
        public decimal UpdatedBy { get; set; }
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
        /// <summary>
        /// ["<c>closedTime</c>"] Close time
        /// </summary>
        [JsonPropertyName("closedTime")]
        public DateTime? CloseTime { get; set; }
        /// <summary>
        /// ["<c>wideFormat</c>"] Wide format
        /// </summary>
        [JsonPropertyName("wideFormat")]
        public bool WideFormat { get; set; }
        /// <summary>
        /// ["<c>new</c>"] New
        /// </summary>
        [JsonPropertyName("new")]
        public bool New { get; set; }
        /// <summary>
        /// ["<c>mailchimpTag</c>"] Mailchimp tag
        /// </summary>
        [JsonPropertyName("mailchimpTag")]
        public string MailchimpTag { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>featured</c>"] Featured
        /// </summary>
        [JsonPropertyName("featured")]
        public bool Featured { get; set; }
        /// <summary>
        /// ["<c>archived</c>"] Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// ["<c>resolvedBy</c>"] Resolved by
        /// </summary>
        [JsonPropertyName("resolvedBy")]
        public string ResolvedBy { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>restricted</c>"] Restricted
        /// </summary>
        [JsonPropertyName("restricted")]
        public bool Restricted { get; set; }
        /// <summary>
        /// ["<c>marketGroup</c>"] Market group
        /// </summary>
        [JsonPropertyName("marketGroup")]
        public decimal MarketGroup { get; set; }
        /// <summary>
        /// ["<c>groupItemTitle</c>"] Group item title
        /// </summary>
        [JsonPropertyName("groupItemTitle")]
        public string GroupItemTitle { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>groupItemThreshold</c>"] Group item threshold
        /// </summary>
        [JsonPropertyName("groupItemThreshold")]
        public string GroupItemThreshold { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>questionID</c>"] Question id
        /// </summary>
        [JsonPropertyName("questionID")]
        public string QuestionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>umaEndDate</c>"] Uma end date
        /// </summary>
        [JsonPropertyName("umaEndDate")]
        public DateTime? UmaEndDate { get; set; }
        /// <summary>
        /// ["<c>enableOrderBook</c>"] Enable order book
        /// </summary>
        [JsonPropertyName("enableOrderBook")]
        public bool EnableOrderBook { get; set; }
        /// <summary>
        /// ["<c>orderPriceMinTickSize</c>"] Order price min tick quantity
        /// </summary>
        [JsonPropertyName("orderPriceMinTickSize")]
        public decimal OrderPriceMinTickQuantity { get; set; }
        /// <summary>
        /// ["<c>orderMinSize</c>"] Order min quantity
        /// </summary>
        [JsonPropertyName("orderMinSize")]
        public decimal OrderMinQuantity { get; set; }
        /// <summary>
        /// ["<c>umaResolutionStatus</c>"] Uma resolution status
        /// </summary>
        [JsonPropertyName("umaResolutionStatus")]
        public string UmaResolutionStatus { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>curationOrder</c>"] Curation order
        /// </summary>
        [JsonPropertyName("curationOrder")]
        public decimal CurationOrder { get; set; }
        /// <summary>
        /// ["<c>volumeNum</c>"] Volume num
        /// </summary>
        [JsonPropertyName("volumeNum")]
        public decimal VolumeNum { get; set; }
        /// <summary>
        /// ["<c>liquidityNum</c>"] Liquidity num
        /// </summary>
        [JsonPropertyName("liquidityNum")]
        public decimal LiquidityNum { get; set; }
        /// <summary>
        /// ["<c>endDateIso</c>"] End date iso
        /// </summary>
        [JsonPropertyName("endDateIso")]
        public DateTime? EndDateIso { get; set; }
        /// <summary>
        /// ["<c>startDateIso</c>"] Start date iso
        /// </summary>
        [JsonPropertyName("startDateIso")]
        public DateTime StartDateIso { get; set; }
        /// <summary>
        /// ["<c>umaEndDateIso</c>"] Uma end date iso
        /// </summary>
        [JsonPropertyName("umaEndDateIso")]
        public DateTime? UmaEndDateIso { get; set; }
        /// <summary>
        /// ["<c>hasReviewedDates</c>"] Has reviewed dates
        /// </summary>
        [JsonPropertyName("hasReviewedDates")]
        public bool HasReviewedDates { get; set; }
        /// <summary>
        /// ["<c>readyForCron</c>"] Ready for cron
        /// </summary>
        [JsonPropertyName("readyForCron")]
        public bool ReadyForCron { get; set; }
        /// <summary>
        /// ["<c>commentsEnabled</c>"] Comments enabled
        /// </summary>
        [JsonPropertyName("commentsEnabled")]
        public bool CommentsEnabled { get; set; }
        /// <summary>
        /// ["<c>volume24hr</c>"] Volume24hr
        /// </summary>
        [JsonPropertyName("volume24hr")]
        public decimal Volume24hr { get; set; }
        /// <summary>
        /// ["<c>volume1wk</c>"] Volume1wk
        /// </summary>
        [JsonPropertyName("volume1wk")]
        public decimal Volume1wk { get; set; }
        /// <summary>
        /// ["<c>volume1mo</c>"] Volume1mo
        /// </summary>
        [JsonPropertyName("volume1mo")]
        public decimal Volume1mo { get; set; }
        /// <summary>
        /// ["<c>volume1yr</c>"] Volume1yr
        /// </summary>
        [JsonPropertyName("volume1yr")]
        public decimal Volume1yr { get; set; }
        /// <summary>
        /// ["<c>gameStartTime</c>"] Game start time
        /// </summary>
        [JsonPropertyName("gameStartTime")]
        public DateTime? GameStartTime { get; set; }
        /// <summary>
        /// ["<c>secondsDelay</c>"] Seconds delay
        /// </summary>
        [JsonPropertyName("secondsDelay")]
        public decimal SecondsDelay { get; set; }
        /// <summary>
        /// ["<c>clobTokenIds</c>"] Clob token ids
        /// </summary>
        [JsonPropertyName("clobTokenIds")]
        [JsonConverter(typeof(ObjectStringConverter<string[]>))]
        public string[]? ClobTokenIds { get; set; }
        /// <summary>
        /// ["<c>disqusThread</c>"] Disqus thread
        /// </summary>
        [JsonPropertyName("disqusThread")]
        public string DisqusThread { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>shortOutcomes</c>"] Short outcomes
        /// </summary>
        [JsonPropertyName("shortOutcomes")]
        public string ShortOutcomes { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>teamAID</c>"] Team aid
        /// </summary>
        [JsonPropertyName("teamAID")]
        public string TeamAID { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>teamBID</c>"] Team bid
        /// </summary>
        [JsonPropertyName("teamBID")]
        public string TeamBID { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>umaBond</c>"] Uma bond
        /// </summary>
        [JsonPropertyName("umaBond")]
        public string UmaBond { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>umaReward</c>"] Uma reward
        /// </summary>
        [JsonPropertyName("umaReward")]
        public string UmaReward { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fpmmLive</c>"] Fpmm live
        /// </summary>
        [JsonPropertyName("fpmmLive")]
        public bool FpmmLive { get; set; }
        /// <summary>
        /// ["<c>volume24hrAmm</c>"] Volume 24 hour amm
        /// </summary>
        [JsonPropertyName("volume24hrAmm")]
        public decimal Volume24hrAmm { get; set; }
        /// <summary>
        /// ["<c>volume1wkAmm</c>"] Volume 1week amm
        /// </summary>
        [JsonPropertyName("volume1wkAmm")]
        public decimal Volume1wkAmm { get; set; }
        /// <summary>
        /// ["<c>volume1moAmm</c>"] Volume 1month amm
        /// </summary>
        [JsonPropertyName("volume1moAmm")]
        public decimal Volume1moAmm { get; set; }
        /// <summary>
        /// ["<c>volume1yrAmm</c>"] Volume 1year amm
        /// </summary>
        [JsonPropertyName("volume1yrAmm")]
        public decimal Volume1yrAmm { get; set; }
        /// <summary>
        /// ["<c>volume24hrClob</c>"] Volume 24hour clob
        /// </summary>
        [JsonPropertyName("volume24hrClob")]
        public decimal Volume24hrClob { get; set; }
        /// <summary>
        /// ["<c>volume1wkClob</c>"] Volume 1 week clob
        /// </summary>
        [JsonPropertyName("volume1wkClob")]
        public decimal Volume1wkClob { get; set; }
        /// <summary>
        /// ["<c>volume1moClob</c>"] Volume 1 month clob
        /// </summary>
        [JsonPropertyName("volume1moClob")]
        public decimal Volume1moClob { get; set; }
        /// <summary>
        /// ["<c>volume1yrClob</c>"] Volume 1year clob
        /// </summary>
        [JsonPropertyName("volume1yrClob")]
        public decimal Volume1yrClob { get; set; }
        /// <summary>
        /// ["<c>volumeAmm</c>"] Volume amm
        /// </summary>
        [JsonPropertyName("volumeAmm")]
        public decimal VolumeAmm { get; set; }
        /// <summary>
        /// ["<c>volumeClob</c>"] Volume clob
        /// </summary>
        [JsonPropertyName("volumeClob")]
        public decimal VolumeClob { get; set; }
        /// <summary>
        /// ["<c>liquidityAmm</c>"] Liquidity amm
        /// </summary>
        [JsonPropertyName("liquidityAmm")]
        public decimal LiquidityAmm { get; set; }
        /// <summary>
        /// ["<c>liquidityClob</c>"] Liquidity clob
        /// </summary>
        [JsonPropertyName("liquidityClob")]
        public decimal LiquidityClob { get; set; }
        /// <summary>
        /// ["<c>makerBaseFee</c>"] Maker base fee
        /// </summary>
        [JsonPropertyName("makerBaseFee")]
        public decimal MakerBaseFee { get; set; }
        /// <summary>
        /// ["<c>takerBaseFee</c>"] Taker base fee
        /// </summary>
        [JsonPropertyName("takerBaseFee")]
        public decimal TakerBaseFee { get; set; }
        /// <summary>
        /// ["<c>customLiveness</c>"] Custom liveness
        /// </summary>
        [JsonPropertyName("customLiveness")]
        public decimal CustomLiveness { get; set; }
        /// <summary>
        /// ["<c>acceptingOrders</c>"] Accepting orders
        /// </summary>
        [JsonPropertyName("acceptingOrders")]
        public bool AcceptingOrders { get; set; }
        /// <summary>
        /// ["<c>notificationsEnabled</c>"] Notifications enabled
        /// </summary>
        [JsonPropertyName("notificationsEnabled")]
        public bool NotificationsEnabled { get; set; }
        /// <summary>
        /// ["<c>score</c>"] Score
        /// </summary>
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
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
        /// ["<c>events</c>"] Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[]? Events { get; set; }
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
        /// ["<c>creator</c>"] Creator
        /// </summary>
        [JsonPropertyName("creator")]
        public string Creator { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ready</c>"] Ready
        /// </summary>
        [JsonPropertyName("ready")]
        public bool Ready { get; set; }
        /// <summary>
        /// ["<c>funded</c>"] Funded
        /// </summary>
        [JsonPropertyName("funded")]
        public bool Funded { get; set; }
        /// <summary>
        /// ["<c>pastSlugs</c>"] Past slugs
        /// </summary>
        [JsonPropertyName("pastSlugs")]
        public string PastSlugs { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>readyTimestamp</c>"] Ready time
        /// </summary>
        [JsonPropertyName("readyTimestamp")]
        public DateTime? ReadyTime { get; set; }
        /// <summary>
        /// ["<c>fundedTimestamp</c>"] Funded time
        /// </summary>
        [JsonPropertyName("fundedTimestamp")]
        public DateTime? FundTime { get; set; }
        /// <summary>
        /// ["<c>acceptingOrdersTimestamp</c>"] Accepting orders time
        /// </summary>
        [JsonPropertyName("acceptingOrdersTimestamp")]
        public DateTime? AcceptingOrdersTime { get; set; }
        /// <summary>
        /// ["<c>competitive</c>"] Competitive
        /// </summary>
        [JsonPropertyName("competitive")]
        public decimal Competitive { get; set; }
        /// <summary>
        /// ["<c>rewardsMinSize</c>"] Rewards min quantity
        /// </summary>
        [JsonPropertyName("rewardsMinSize")]
        public decimal RewardsMinQuantity { get; set; }
        /// <summary>
        /// ["<c>rewardsMaxSpread</c>"] Rewards max spread
        /// </summary>
        [JsonPropertyName("rewardsMaxSpread")]
        public decimal RewardsMaxSpread { get; set; }
        /// <summary>
        /// ["<c>spread</c>"] Spread
        /// </summary>
        [JsonPropertyName("spread")]
        public decimal Spread { get; set; }
        /// <summary>
        /// ["<c>automaticallyResolved</c>"] Automatically resolved
        /// </summary>
        [JsonPropertyName("automaticallyResolved")]
        public bool AutomaticallyResolved { get; set; }
        /// <summary>
        /// ["<c>oneDayPriceChange</c>"] One day price change
        /// </summary>
        [JsonPropertyName("oneDayPriceChange")]
        public decimal OneDayPriceChange { get; set; }
        /// <summary>
        /// ["<c>oneHourPriceChange</c>"] One hour price change
        /// </summary>
        [JsonPropertyName("oneHourPriceChange")]
        public decimal OneHourPriceChange { get; set; }
        /// <summary>
        /// ["<c>oneWeekPriceChange</c>"] One week price change
        /// </summary>
        [JsonPropertyName("oneWeekPriceChange")]
        public decimal OneWeekPriceChange { get; set; }
        /// <summary>
        /// ["<c>oneMonthPriceChange</c>"] One month price change
        /// </summary>
        [JsonPropertyName("oneMonthPriceChange")]
        public decimal OneMonthPriceChange { get; set; }
        /// <summary>
        /// ["<c>oneYearPriceChange</c>"] One year price change
        /// </summary>
        [JsonPropertyName("oneYearPriceChange")]
        public decimal OneYearPriceChange { get; set; }
        /// <summary>
        /// ["<c>lastTradePrice</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("lastTradePrice")]
        public decimal LastTradePrice { get; set; }
        /// <summary>
        /// ["<c>bestBid</c>"] Best bid
        /// </summary>
        [JsonPropertyName("bestBid")]
        public decimal BestBid { get; set; }
        /// <summary>
        /// ["<c>bestAsk</c>"] Best ask
        /// </summary>
        [JsonPropertyName("bestAsk")]
        public decimal BestAsk { get; set; }
        /// <summary>
        /// ["<c>automaticallyActive</c>"] Automatically active
        /// </summary>
        [JsonPropertyName("automaticallyActive")]
        public bool AutomaticallyActive { get; set; }
        /// <summary>
        /// ["<c>clearBookOnStart</c>"] Clear book on start
        /// </summary>
        [JsonPropertyName("clearBookOnStart")]
        public bool ClearBookOnStart { get; set; }
        /// <summary>
        /// ["<c>chartColor</c>"] Chart color
        /// </summary>
        [JsonPropertyName("chartColor")]
        public string ChartColor { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>seriesColor</c>"] Series color
        /// </summary>
        [JsonPropertyName("seriesColor")]
        public string SeriesColor { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>showGmpSeries</c>"] Show gmp series
        /// </summary>
        [JsonPropertyName("showGmpSeries")]
        public bool ShowGmpSeries { get; set; }
        /// <summary>
        /// ["<c>showGmpOutcome</c>"] Show gmp outcome
        /// </summary>
        [JsonPropertyName("showGmpOutcome")]
        public bool ShowGmpOutcome { get; set; }
        /// <summary>
        /// ["<c>manualActivation</c>"] Manual activation
        /// </summary>
        [JsonPropertyName("manualActivation")]
        public bool ManualActivation { get; set; }
        /// <summary>
        /// ["<c>negRiskOther</c>"] Negative risk other
        /// </summary>
        [JsonPropertyName("negRiskOther")]
        public bool NegativeRiskOther { get; set; }
        /// <summary>
        /// ["<c>gameId</c>"] Game id
        /// </summary>
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>groupItemRange</c>"] Group item range
        /// </summary>
        [JsonPropertyName("groupItemRange")]
        public string GroupItemRange { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>sportsMarketType</c>"] Sports market type
        /// </summary>
        [JsonPropertyName("sportsMarketType")]
        public string SportsMarketType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>line</c>"] Line
        /// </summary>
        [JsonPropertyName("line")]
        public decimal Line { get; set; }
        /// <summary>
        /// ["<c>umaResolutionStatuses</c>"] Uma resolution statuses
        /// </summary>
        [JsonPropertyName("umaResolutionStatuses")]
        public string? UmaResolutionStatuses { get; set; }
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
        /// ["<c>deployingTimestamp</c>"] Deploying time
        /// </summary>
        [JsonPropertyName("deployingTimestamp")]
        public DateTime? DeployTime { get; set; }
        /// <summary>
        /// ["<c>scheduledDeploymentTimestamp</c>"] Scheduled deployment time
        /// </summary>
        [JsonPropertyName("scheduledDeploymentTimestamp")]
        public DateTime? ScheduledDeploymentTime { get; set; }
        /// <summary>
        /// ["<c>rfqEnabled</c>"] Rfq enabled
        /// </summary>
        [JsonPropertyName("rfqEnabled")]
        public bool RfqEnabled { get; set; }
        /// <summary>
        /// ["<c>eventStartTime</c>"] Event start time
        /// </summary>
        [JsonPropertyName("eventStartTime")]
        public DateTime? EventStartTime { get; set; }
    }
}
