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
        /// ["<c>proxyWallet</c>"] ProxyWallet
        /// </summary>
        [JsonPropertyName("proxyWallet")]
        public string ProxyWallet { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>conditionId</c>"] ConditionId
        /// </summary>
        [JsonPropertyName("conditionId")]
        public string ConditionId { get; set; } = String.Empty;

        /// <summary>
        /// ["<c>size</c>"] Size
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Size { get; set; }

        /// <summary>
        /// ["<c>avgPrice</c>"] AvgPrice
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AvgPrice { get; set; }

        /// <summary>
        /// ["<c>initialValue</c>"] InitialValue
        /// </summary>
        [JsonPropertyName("initialValue")]
        public decimal InitialValue { get; set; }

        /// <summary>
        /// ["<c>currentValue</c>"] CurrentValue
        /// </summary>
        [JsonPropertyName("currentValue")]
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// ["<c>cashPnl</c>"] CashPnl
        /// </summary>
        [JsonPropertyName("cashPnl")]
        public decimal CashPnl { get; set; }

        /// <summary>
        /// ["<c>percentPnl</c>"] PercentPnl
        /// </summary>
        [JsonPropertyName("percentPnl")]
        public decimal PercentPnl { get; set; }

        /// <summary>
        /// ["<c>totalBought</c>"] TotalBought
        /// </summary>
        [JsonPropertyName("totalBought")]
        public decimal TotalBought { get; set; }

        /// <summary>
        /// ["<c>totalSold</c>"] TotalSold
        /// </summary>
        [JsonPropertyName("realizedPnl")]
        public decimal RealizedPnl { get; set; }

        /// <summary>
        /// ["<c>percentRealizedPnl</c>"] PercentRealizedPnl
        /// </summary>
        [JsonPropertyName("percentRealizedPnl")]
        public decimal PercentRealizedPnl { get; set; }

        /// <summary>
        /// ["<c>curPrice</c>"] CurPrice
        /// </summary>
        [JsonPropertyName("curPrice")]
        public decimal CurPrice { get; set; }

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
        /// ["<c>eventId</c>"] EventId
        /// </summary>
        [JsonPropertyName("eventId")]
        public string EventId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>eventSlug</c>"] EventSlug
        /// </summary>
        [JsonPropertyName("eventSlug")]
        public string EventSlug { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>outcome</c>"] Outcome
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>outcomeIndex</c>"] OutcomeIndex
        /// </summary>
        [JsonPropertyName("outcomeIndex")]
        public int OutcomeIndex { get; set; }

        /// <summary>
        /// ["<c>oppositeOutcome</c>"] OppositeOutcome
        /// </summary>
        [JsonPropertyName("oppositeOutcome")]
        public string OppositeOutcome { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>oppositeAsset</c>"] OppositeAsset
        /// </summary>
        [JsonPropertyName("oppositeAsset")]
        public string OppositeAsset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>createdDate</c>"] CreatedDate
        /// </summary>
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>negativeRisk</c>"] NegativeRisk
        /// </summary>
        [JsonPropertyName("negativeRisk")]
        public bool NegativeRisk { get; set; }
    }
}