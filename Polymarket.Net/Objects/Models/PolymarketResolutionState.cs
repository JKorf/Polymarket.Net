using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

internal record PolymarketResolutionStateWrapper
{
    /// <summary>
    /// ["<c>data</c>"] Data
    /// </summary>
    [JsonPropertyName("data")]
    public PolymarketResolutionState[] Data { get; set; } = [];
}

/// <summary>
/// 
/// </summary>
public record PolymarketResolutionState
{
    /// <summary>
    /// ["<c>extended_review</c>"] Extended review
    /// </summary>
    [JsonPropertyName("extended_review")]
    public bool ExtendedReview { get; set; }
    /// <summary>
    /// ["<c>last_update_timestamp</c>"] Last update timestamp
    /// </summary>
    [JsonPropertyName("last_update_timestamp")]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// ["<c>log_index</c>"] Log index
    /// </summary>
    [JsonPropertyName("log_index")]
    public string LogIndex { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>new_version_q</c>"] New version q
    /// </summary>
    [JsonPropertyName("new_version_q")]
    public bool NewVersionQ { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Resolution status
    /// </summary>
    [JsonPropertyName("status")]
    public ResolutionStatus ResolutionStatus { get; set; }
    /// <summary>
    /// ["<c>transaction_hash</c>"] Transaction hash
    /// </summary>
    [JsonPropertyName("transaction_hash")]
    public string TransactionHash { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>was_disputed</c>"] Was disputed
    /// </summary>
    [JsonPropertyName("was_disputed")]
    public bool WasDisputed { get; set; }
    /// <summary>
    /// ["<c>condition_id</c>"] Market id
    /// </summary>
    [JsonPropertyName("condition_id")]
    public string MarketId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>expected_settlement_time</c>"] Expected settlement time
    /// </summary>
    [JsonPropertyName("expected_settlement_time")]
    public DateTime? ExpectedSettlementTime { get; set; }
    /// <summary>
    /// ["<c>market_type</c>"] Market type
    /// </summary>
    [JsonPropertyName("market_type")]
    public string MarketType { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>payouts</c>"] Payouts
    /// </summary>
    [JsonPropertyName("payouts")]
    public string? Payouts { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
    /// <summary>
    /// ["<c>proposed_price</c>"] Proposed price
    /// </summary>
    [JsonPropertyName("proposed_price")]
    public decimal? ProposedPrice { get; set; }
    /// <summary>
    /// ["<c>question_id</c>"] Question id
    /// </summary>
    [JsonPropertyName("question_id")]
    public string QuestionId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>reporter</c>"] Reporter
    /// </summary>
    [JsonPropertyName("reporter")]
    public string Reporter { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>reproposed_price</c>"] Reproposed price
    /// </summary>
    [JsonPropertyName("reproposed_price")]
    public decimal? ReproposedPrice { get; set; }
    /// <summary>
    /// ["<c>resolution_source</c>"] Resolution source
    /// </summary>
    [JsonPropertyName("resolution_source")]
    public string ResolutionSource { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>resolved_at</c>"] Resolved at
    /// </summary>
    [JsonPropertyName("resolved_at")]
    public DateTime? ResolveTime { get; set; }
    /// <summary>
    /// ["<c>resolved_block</c>"] Resolved block
    /// </summary>
    [JsonPropertyName("resolved_block")]
    public long ResolvedBlock { get; set; }
    /// <summary>
    /// ["<c>settlement_time_basis</c>"] Settlement time basis
    /// </summary>
    [JsonPropertyName("settlement_time_basis")]
    public string SettlementTimeBasis { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>was_arbitrated</c>"] Was arbitrated
    /// </summary>
    [JsonPropertyName("was_arbitrated")]
    public bool WasArbitrated { get; set; }
}

