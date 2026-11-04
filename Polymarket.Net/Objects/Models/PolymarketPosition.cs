using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Positions info
    /// </summary>
    public record PolymarketPosition
    {
        /// <summary>
        /// ["<c>proxy_wallet</c>"] ProxyWallet
        /// </summary>
        [JsonPropertyName("proxy_wallet")]
        public string ProxyWallet { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>condition_id</c>"] Market id
        /// </summary>
        [JsonPropertyName("condition_id")]
        public string MarketId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>token_id</c>"] Token id
        /// </summary>
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>current_size</c>"] Current size
        /// </summary>
        [JsonPropertyName("current_size")]
        public decimal CurrentSize { get; set; }
        /// <summary>
        /// ["<c>avg_price</c>"] Average price
        /// </summary>
        [JsonPropertyName("avg_price")]
        public decimal AvgPrice { get; set; }
        /// <summary>
        /// ["<c>entry_cost_usdc</c>"] Entry cost in USDC
        /// </summary>
        [JsonPropertyName("entry_cost_usdc")]
        public decimal EntryCostUsdc { get; set; }
        /// <summary>
        /// ["<c>entry_fees_usdc</c>"] Entry fees in USDC
        /// </summary>
        [JsonPropertyName("entry_fees_usdc")]
        public decimal EntryFeesUsdc { get; set; }
        /// <summary>
        /// ["<c>total_cost_usdc</c>"] Total cost in USDC
        /// </summary>
        [JsonPropertyName("total_cost_usdc")]
        public decimal TotalCostUsdc { get; set; }
        /// <summary>
        /// ["<c>current_price</c>"] Current price
        /// </summary>
        [JsonPropertyName("current_price")]
        public decimal CurrentPrice { get; set; }
        /// <summary>
        /// ["<c>current_value</c>"] CurrentValue
        /// </summary>
        [JsonPropertyName("current_value")]
        public decimal CurrentValue { get; set; }
        /// <summary>
        /// ["<c>total_size</c>"] Total size
        /// </summary>
        [JsonPropertyName("total_size")]
        public decimal TotalSize { get; set; }
        /// <summary>
        /// ["<c>realized_pnl</c>"] Realized profit and loss
        /// </summary>
        [JsonPropertyName("realized_pnl")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// ["<c>realized_pnl</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("unrealized_pnl")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>total_pnl</c>"] Total profit and loss
        /// </summary>
        [JsonPropertyName("total_pnl")]
        public decimal TotalPnl { get; set; }
        /// <summary>
        /// ["<c>percent_pnl</c>"] Percentage profit and loss
        /// </summary>
        [JsonPropertyName("percent_pnl")]
        public decimal PercentPnl { get; set; }
        /// <summary>
        /// ["<c>percent_realized_pnl</c>"] Percentage realized profit and loss
        /// </summary>
        [JsonPropertyName("percent_realized_pnl")]
        public decimal PercentRealizedPnl { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Position status
        /// </summary>
        [JsonPropertyName("status")]
        public PositionStatus Status { get; set; }
        /// <summary>
        /// ["<c>redeemable</c>"] Redeemable
        /// </summary>
        [JsonPropertyName("redeemable")]
        public bool Redeemable { get; set; }
        /// <summary>
        /// ["<c>mergeable</c>"] Mergeable
        /// </summary>
        [JsonPropertyName("mergeable")]
        public bool Mergeable { get; set; }
        /// <summary>
        /// ["<c>negativeRisk</c>"] Negative risk
        /// </summary>
        [JsonPropertyName("negativeRisk")]
        public bool NegativeRisk { get; set; }
        /// <summary>
        /// ["<c>archived</c>"] Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>slug</c>"] Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>icon</c>"] Icon
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>event_id</c>"] Event id
        /// </summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>event_slug</c>"] Event slug
        /// </summary>
        [JsonPropertyName("event_slug")]
        public string EventSlug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>outcome</c>"] Outcome
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>outcome_index</c>"] OutcomeIndex
        /// </summary>
        [JsonPropertyName("outcome_index")]
        public int OutcomeIndex { get; set; }
        /// <summary>
        /// ["<c>opposite_outcome</c>"] Opposite outcome
        /// </summary>
        [JsonPropertyName("opposite_outcome")]
        public string OppositeOutcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>opposite_token_id</c>"] Opposite token id
        /// </summary>
        [JsonPropertyName("opposite_token_id")]
        public string OppositeTokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>end_data</c>"] End date
        /// </summary>
        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ["<c>last_event_at</c>"] Last event at
        /// </summary>
        [JsonPropertyName("last_event_at")]
        public DateTime? LastEventAt { get; set; }
        /// <summary>
        /// ["<c>first_entry_at</c>"] First entry at
        /// </summary>
        [JsonPropertyName("first_entry_at")]
        public DateTime? FirstEntryAt { get; set; }
    }
}