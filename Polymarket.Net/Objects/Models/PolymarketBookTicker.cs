using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Book ticker
/// </summary>
public record PolymarketBookTicker
{
    /// <summary>
    /// ["<c>instrument_id</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>bid_price</c>"] Best bid price
    /// </summary>
    [JsonPropertyName("bid_price")]
    public decimal BestBidPrice { get; set; }
    /// <summary>
    /// ["<c>bid_quantity</c>"] Best bid quantity
    /// </summary>
    [JsonPropertyName("bid_quantity")]
    public decimal BestBidQuantity { get; set; }
    /// <summary>
    /// ["<c>ask_price</c>"] Best ask price
    /// </summary>
    [JsonPropertyName("ask_price")]
    public decimal BestAskPrice { get; set; }
    /// <summary>
    /// ["<c>ask_quantity</c>"] Best ask quantity
    /// </summary>
    [JsonPropertyName("ask_quantity")]
    public decimal BestAskQuantity { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

