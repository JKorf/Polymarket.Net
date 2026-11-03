using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Trade info
/// </summary>
public record PolymarketPerpTrade
{
    /// <summary>
    /// ["<c>trade_id</c>"] Trade id
    /// </summary>
    [JsonPropertyName("trade_id")]
    public long TradeId { get; set; }
    [JsonInclude, JsonPropertyName("tid")]
    internal long TradeIdInt { set => TradeId = value; }
    /// <summary>
    /// ["<c>instrument_id</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long SymbolId { get; set; }
    [JsonInclude, JsonPropertyName("iid")]
    internal long SymbolIdInt { set => SymbolId = value; }
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public PositionSide Side { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    [JsonInclude, JsonPropertyName("p")]
    internal decimal PriceInt { set => Price = value; }
    /// <summary>
    /// ["<c>quantity</c>"] Quantity
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
    [JsonInclude, JsonPropertyName("qty")]
    internal decimal QuantityInt { set => Quantity = value; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    [JsonInclude, JsonPropertyName("ts")]
    internal DateTime TimestampInt { set => Timestamp = value; }
    /// <summary>
    /// ["<c>hash</c>"] Hash
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>settlement</c>"] Settlement
    /// </summary>
    [JsonPropertyName("settlement")]
    public bool Settlement { get; set; }
}

