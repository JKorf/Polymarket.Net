using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// 
/// </summary>
public record PolymarketExchangeStatistics
{
    /// <summary>
    /// ["<c>start_timestamp</c>"] Start timestamp
    /// </summary>
    [JsonPropertyName("start_timestamp")]
    public DateTime StartTime { get; set; }
    /// <summary>
    /// ["<c>end_timestamp</c>"] End timestamp
    /// </summary>
    [JsonPropertyName("end_timestamp")]
    public DateTime EndTime { get; set; }
    /// <summary>
    /// ["<c>volume</c>"] Volume in USD
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal Volume { get; set; }
    /// <summary>
    /// ["<c>open_interest</c>"] Open interest in USD
    /// </summary>
    [JsonPropertyName("open_interest")]
    public decimal OpenInterest { get; set; }
    /// <summary>
    /// ["<c>open_interest_timestamp</c>"] Open interest timestamp
    /// </summary>
    [JsonPropertyName("open_interest_timestamp")]
    public DateTime? OpenInterestTime { get; set; }
    /// <summary>
    /// ["<c>fees</c>"] Fees in USD
    /// </summary>
    [JsonPropertyName("fees")]
    public decimal Fees { get; set; }
}

