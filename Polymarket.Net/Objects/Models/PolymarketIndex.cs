using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Index info
/// </summary>
public record PolymarketIndex
{
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>index_price</c>"] Index price
    /// </summary>
    [JsonPropertyName("index_price")]
    public decimal IndexPrice { get; set; }
    /// <summary>
    /// ["<c>constituents</c>"] Constituents
    /// </summary>
    [JsonPropertyName("constituents")]
    public PolymarketIndexConstituent[] Constituents { get; set; } = [];
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Constituent
/// </summary>
public record PolymarketIndexConstituent
{
    /// <summary>
    /// ["<c>source</c>"] Source
    /// </summary>
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>weight</c>"] Weight
    /// </summary>
    [JsonPropertyName("weight")]
    public decimal Weight { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

