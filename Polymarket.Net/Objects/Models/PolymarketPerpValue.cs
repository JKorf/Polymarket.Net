using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Value
/// </summary>
[JsonConverter(typeof(ArrayConverter<PolymarketPerpValue>))]
public record PolymarketPerpValue
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Value
    /// </summary>
    [ArrayProperty(1)]
    public decimal Value { get; set; }
}

