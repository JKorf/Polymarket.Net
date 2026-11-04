using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Funding history
/// </summary>
public record PolymarketFundingHistory
{
    /// <summary>
    /// ["<c>id</c>"] Id
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }
    /// <summary>
    /// ["<c>instrument_id</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>size</c>"] Quantity
    /// </summary>
    [JsonPropertyName("size")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>funding_rate</c>"] Funding rate
    /// </summary>
    [JsonPropertyName("funding_rate")]
    public decimal FundingRate { get; set; }
    /// <summary>
    /// ["<c>funding_asset</c>"] Funding asset
    /// </summary>
    [JsonPropertyName("funding_asset")]
    public string FundingAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>funding</c>"] Funding
    /// </summary>
    [JsonPropertyName("funding")]
    public decimal Funding { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

