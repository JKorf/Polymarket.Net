using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models {
	/// <summary>
	/// Closed positions info
	/// </summary>
	public record PolymarketClosedPosition {
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
		/// ["<c>conditionId</c>"] Market id
		/// </summary>
		[JsonPropertyName("conditionId")]
		public string MarketId { get; set; } = string.Empty;

		/// <summary>
		/// ["<c>avgPrice</c>"] AvgPrice
		/// </summary>
		[JsonPropertyName("avgPrice")]
		public decimal AvgPrice { get; set; }

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
		/// ["<c>curPrice</c>"] CurPrice
		/// </summary>
		[JsonPropertyName("curPrice")]
		public decimal CurPrice { get; set; }

		/// <summary>
		/// ["<c>timestamp</c>"] Timestamp
		/// </summary>
		[JsonPropertyName("timestamp")]
		public UInt64 Timestamp { get; set; }

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
	}
}