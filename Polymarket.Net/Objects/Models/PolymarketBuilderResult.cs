using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

internal record PolymarketBuilderResultWrapper
{
    /// <summary>
    /// ["<c>data</c>"] Data
    /// </summary>
    [JsonPropertyName("data")]
    public PolymarketBuilderResult[] Data { get; set; } = [];
}

/// <summary>
/// Builder result
/// </summary>
public record PolymarketBuilderResult
{
    /// <summary>
    /// ["<c>trader</c>"] Trader
    /// </summary>
    [JsonPropertyName("trader")]
    public string Trader { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>builder</c>"] Builder
    /// </summary>
    [JsonPropertyName("builder")]
    public string Builder { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>max_fee_rate</c>"] Max fee rate
    /// </summary>
    [JsonPropertyName("max_fee_rate")]
    public decimal MaxFeeRate { get; set; }
    /// <summary>
    /// ["<c>approval_version</c>"] Approval version
    /// </summary>
    [JsonPropertyName("approval_version")]
    public int ApprovalVersion { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>sequence</c>"] Sequence
    /// </summary>
    [JsonPropertyName("sequence")]
    public long Sequence { get; set; }
}

