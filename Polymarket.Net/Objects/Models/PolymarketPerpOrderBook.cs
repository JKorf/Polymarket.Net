using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Order book
/// </summary>
public record PolymarketPerpOrderBook
{
    /// <summary>
    /// ["<c>instrument_id</c>"] Instrument id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long InstrumentId { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>sequence</c>"] Sequence
    /// </summary>
    [JsonPropertyName("sequence")]
    public decimal Sequence { get; set; }

    /// <summary>
    /// ["<c>asks</c>"] Asks
    /// </summary>
    [JsonPropertyName("asks")]
    public PolymarketOrderBookEntry[] Asks { get; set; } = [];
    /// <summary>
    /// ["<c>bids</c>"] Bids
    /// </summary>
    [JsonPropertyName("bids")]
    public PolymarketOrderBookEntry[] Bids { get; set; } = [];
}

/// <summary>
/// Order book entry
/// </summary>
[JsonConverter(typeof(ArrayConverter<PolymarketOrderBookEntry>))]
public record PolymarketOrderBookEntry
{
    /// <summary>
    /// Price
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }
}
