using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// 
/// </summary>
public record PolymarketTicker
{
    /// <summary>
    /// ["<c>instrument_id</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>index_price</c>"] Index price
    /// </summary>
    [JsonPropertyName("index_price")]
    public decimal IndexPrice { get; set; }
    /// <summary>
    /// ["<c>mark_price</c>"] Mark price
    /// </summary>
    [JsonPropertyName("mark_price")]
    public decimal MarkPrice { get; set; }
    /// <summary>
    /// ["<c>last_price</c>"] Last price
    /// </summary>
    [JsonPropertyName("last_price")]
    public decimal LastPrice { get; set; }
    /// <summary>
    /// ["<c>mid_price</c>"] Mid price
    /// </summary>
    [JsonPropertyName("mid_price")]
    public decimal MidPrice { get; set; }
    /// <summary>
    /// ["<c>open_interest</c>"] Open interest
    /// </summary>
    [JsonPropertyName("open_interest")]
    public decimal OpenInterest { get; set; }
    /// <summary>
    /// ["<c>funding_rate</c>"] Funding rate
    /// </summary>
    [JsonPropertyName("funding_rate")]
    public decimal FundingRate { get; set; }
    /// <summary>
    /// ["<c>next_funding</c>"] Next funding
    /// </summary>
    [JsonPropertyName("next_funding")]
    public DateTime NextFunding { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

