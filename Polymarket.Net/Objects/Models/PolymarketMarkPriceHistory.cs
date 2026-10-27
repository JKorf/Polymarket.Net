using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Mark price history
/// </summary>
[JsonConverter(typeof(ArrayConverter<PolymarketMarkPriceHistory>))]
public record PolymarketMarkPriceHistory
{
    /// <summary>
    /// Data timestamp
    /// </summary>
    [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Mark price
    /// </summary>
    [ArrayProperty(1)]
    public decimal MarkPrice { get; set; }
}

