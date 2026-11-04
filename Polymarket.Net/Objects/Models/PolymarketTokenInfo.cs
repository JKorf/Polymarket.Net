using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Token info
/// </summary>
public record PolymarketTokenInfo
{
    /// <summary>
    /// ["<c>condition_id</c>"] Market id
    /// </summary>
    [JsonPropertyName("condition_id")]
    public string MarketId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>module</c>"] Module
    /// </summary>
    [JsonPropertyName("module")]
    public string Module { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>outcome_index</c>"] Outcome index
    /// </summary>
    [JsonPropertyName("outcome_index")]
    public long? OutcomeIndex { get; set; }
    /// <summary>
    /// ["<c>resolved</c>"] Resolved
    /// </summary>
    [JsonPropertyName("resolved")]
    public bool Resolved { get; set; }
    /// <summary>
    /// ["<c>token_id</c>"] Token id
    /// </summary>
    [JsonPropertyName("token_id")]
    public string TokenId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>clob_index</c>"] Clob index
    /// </summary>
    [JsonPropertyName("clob_index")]
    public long? ClobIndex { get; set; }
    /// <summary>
    /// ["<c>closed</c>"] Closed
    /// </summary>
    [JsonPropertyName("closed")]
    public bool Closed { get; set; }
    /// <summary>
    /// ["<c>event_id</c>"] Event id
    /// </summary>
    [JsonPropertyName("event_id")]
    public long EventId { get; set; }
    /// <summary>
    /// ["<c>event_slug</c>"] Event slug
    /// </summary>
    [JsonPropertyName("event_slug")]
    public string EventSlug { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>final_price</c>"] Final price
    /// </summary>
    [JsonPropertyName("final_price")]
    public decimal? FinalPrice { get; set; }
    /// <summary>
    /// ["<c>market_slug</c>"] Market slug
    /// </summary>
    [JsonPropertyName("market_slug")]
    public string MarketSlug { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>neg_risk</c>"] Negative risk
    /// </summary>
    [JsonPropertyName("neg_risk")]
    public bool NegRisk { get; set; }
    /// <summary>
    /// ["<c>neg_risk_market_id</c>"] Negative risk market id
    /// </summary>
    [JsonPropertyName("neg_risk_market_id")]
    public string? NegRiskMarketId { get; set; }
    /// <summary>
    /// ["<c>opposite_token_id</c>"] Opposite token id
    /// </summary>
    [JsonPropertyName("opposite_token_id")]
    public string OppositeTokenId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>outcome</c>"] Outcome
    /// </summary>
    [JsonPropertyName("outcome")]
    public string Outcome { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>question_index</c>"] Question index
    /// </summary>
    [JsonPropertyName("question_index")]
    public long? QuestionIndex { get; set; }
    /// <summary>
    /// ["<c>structural_condition_id</c>"] Structural condition id
    /// </summary>
    [JsonPropertyName("structural_condition_id")]
    public string StructuralConditionId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>title</c>"] Title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
}

