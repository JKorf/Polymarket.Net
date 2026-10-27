using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Price stats
/// </summary>
public record PolymarketStatistics
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
    /// ["<c>volume</c>"] Volume in quote asset
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal Volume { get; set; }
    /// <summary>
    /// ["<c>open_price</c>"] Open price
    /// </summary>
    [JsonPropertyName("open_price")]
    public decimal OpenPrice { get; set; }
    /// <summary>
    /// ["<c>klines</c>"] Klines
    /// </summary>
    [JsonPropertyName("klines")]
    public PolymarketKline[] Klines { get; set; } = []!;
}

