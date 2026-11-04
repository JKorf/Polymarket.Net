using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Funding rate hinfo
/// </summary>
public record PolymarketFundingRate
{
    /// <summary>
    /// ["<c>funding_rate</c>"] Funding rate
    /// </summary>
    [JsonPropertyName("funding_rate")]
    public decimal FundingRate { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

