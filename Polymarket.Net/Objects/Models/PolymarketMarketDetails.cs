using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// 
    /// </summary>
    public record PolymarketMarketDetails
    {
        /// <summary>
        /// ["<c>enable_order_book</c>"] Enable order book
        /// </summary>
        [JsonPropertyName("enable_order_book")]
        public bool EnableOrderBook { get; set; }
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
        /// ["<c>accepting_orders</c>"] Accepting orders
        /// </summary>
        [JsonPropertyName("accepting_orders")]
        public bool AcceptingOrders { get; set; }
        /// <summary>
        /// ["<c>accepting_order_timestamp</c>"] Accepting order timestamp
        /// </summary>
        [JsonPropertyName("accepting_order_timestamp")]
        public DateTime? AcceptingOrderTimestamp { get; set; }
        /// <summary>
        /// ["<c>minimum_order_size</c>"] Minimum order quantity
        /// </summary>
        [JsonPropertyName("minimum_order_size")]
        public decimal MinimumOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>minimum_tick_size</c>"] Minimum tick quantity
        /// </summary>
        [JsonPropertyName("minimum_tick_size")]
        public decimal MinimumTickQuantity { get; set; }
        /// <summary>
        /// ["<c>condition_id</c>"] Condition id
        /// </summary>
        [JsonPropertyName("condition_id")]
        public string ConditionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>question_id</c>"] Question id
        /// </summary>
        [JsonPropertyName("question_id")]
        public string QuestionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>question</c>"] Question
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>market_slug</c>"] Market slug
        /// </summary>
        [JsonPropertyName("market_slug")]
        public string MarketSlug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>end_date_iso</c>"] End time
        /// </summary>
        [JsonPropertyName("end_date_iso")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// ["<c>game_start_time</c>"] Game start time
        /// </summary>
        [JsonPropertyName("game_start_time")]
        public DateTime? GameStartTime { get; set; }
        /// <summary>
        /// ["<c>seconds_delay</c>"] Seconds delay
        /// </summary>
        [JsonPropertyName("seconds_delay")]
        public long SecondsDelay { get; set; }
        /// <summary>
        /// ["<c>fpmm</c>"] Fpmm
        /// </summary>
        [JsonPropertyName("fpmm")]
        public string? Fpmm { get; set; }
        /// <summary>
        /// ["<c>maker_base_fee</c>"] Maker base fee
        /// </summary>
        [JsonPropertyName("maker_base_fee")]
        public decimal MakerBaseFee { get; set; }
        /// <summary>
        /// ["<c>taker_base_fee</c>"] Taker base fee
        /// </summary>
        [JsonPropertyName("taker_base_fee")]
        public decimal TakerBaseFee { get; set; }
        /// <summary>
        /// ["<c>notifications_enabled</c>"] Notifications enabled
        /// </summary>
        [JsonPropertyName("notifications_enabled")]
        public bool NotificationsEnabled { get; set; }
        /// <summary>
        /// ["<c>neg_risk</c>"] Negative risk enabled
        /// </summary>
        [JsonPropertyName("neg_risk")]
        public bool NegativeRisk { get; set; }
        /// <summary>
        /// ["<c>neg_risk_market_id</c>"] Negative risk market id
        /// </summary>
        [JsonPropertyName("neg_risk_market_id")]
        public string NegativeRiskMarketId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>neg_risk_request_id</c>"] Negative risk request id
        /// </summary>
        [JsonPropertyName("neg_risk_request_id")]
        public string NegativeRiskRequestId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>icon</c>"] Icon
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>image</c>"] Image
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>rewards</c>"] Rewards
        /// </summary>
        [JsonPropertyName("rewards")]
        public PolymarketMarketReward Rewards { get; set; } = null!;
        /// <summary>
        /// ["<c>is_50_50_outcome</c>"] Is 50/50 outcome
        /// </summary>
        [JsonPropertyName("is_50_50_outcome")]
        public bool Is5050Outcome { get; set; }
        /// <summary>
        /// ["<c>tokens</c>"] Tokens
        /// </summary>
        [JsonPropertyName("tokens")]
        public PolymarketMarketToken[] Tokens { get; set; } = [];
        /// <summary>
        /// ["<c>tags</c>"] Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = [];
    }
}
