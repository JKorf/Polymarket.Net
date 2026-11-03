using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// User funding update
/// </summary>
public record PolymarketUserFundingUpdate
{
    /// <summary>
    /// ["<c>id</c>"] Id
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }
    /// <summary>
    /// ["<c>iid</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("iid")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>sz</c>"] Quantity
    /// </summary>
    [JsonPropertyName("sz")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>fr</c>"] Funding rate
    /// </summary>
    [JsonPropertyName("fr")]
    public decimal FundingRate { get; set; }
    /// <summary>
    /// ["<c>fua</c>"] Funding asset
    /// </summary>
    [JsonPropertyName("fua")]
    public string FundingAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>fund</c>"] Funding
    /// </summary>
    [JsonPropertyName("fund")]
    public decimal Funding { get; set; }
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
}

