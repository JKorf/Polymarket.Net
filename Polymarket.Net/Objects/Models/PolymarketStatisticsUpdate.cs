using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Price stats
/// </summary>
public record PolymarketStatisticsUpdate
{
    /// <summary>
    /// ["<c>iid</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("iid")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>vol</c>"] Volume in quote asset
    /// </summary>
    [JsonPropertyName("vol")]
    public decimal Volume { get; set; }
    /// <summary>
    /// ["<c>open</c>"] Open price
    /// </summary>
    [JsonPropertyName("open")]
    public decimal OpenPrice { get; set; }
    /// <summary>
    /// ["<c>klines</c>"] Klines
    /// </summary>
    [JsonPropertyName("klines")]
    public PolymarketKline[] Klines { get; set; } = []!;
}

