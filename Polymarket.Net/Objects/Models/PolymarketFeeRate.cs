using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

internal record PolymarketFeeRatesWrapper
{
    /// <summary>
    /// ["<c>fee_schedule</c>"] Fee schedule
    /// </summary>
    [JsonPropertyName("fee_schedule")]
    public PolymarketFeeRate[] FeeSchedule { get; set; } = [];
}

/// <summary>
/// Fee rate info
/// </summary>
public record PolymarketFeeRate
{
    /// <summary>
    /// ["<c>instrument_type</c>"] Instrument type
    /// </summary>
    [JsonPropertyName("instrument_type")]
    public InstrumentType InstrumentType { get; set; }
    /// <summary>
    /// ["<c>category</c>"] Category
    /// </summary>
    [JsonPropertyName("category")]
    public Category Category { get; set; }
    /// <summary>
    /// ["<c>taker_fee_rate</c>"] Taker fee rate
    /// </summary>
    [JsonPropertyName("taker_fee_rate")]
    public decimal TakerFeeRate { get; set; }
    /// <summary>
    /// ["<c>maker_fee_rate</c>"] Maker fee rate
    /// </summary>
    [JsonPropertyName("maker_fee_rate")]
    public decimal MakerFeeRate { get; set; }
    /// <summary>
    /// ["<c>tiers</c>"] Tiers
    /// </summary>
    [JsonPropertyName("tiers")]
    public PolymarketFeeRateTier[] Tiers { get; set; } = [];
}

/// <summary>
/// Fee tier
/// </summary>
public record PolymarketFeeRateTier
{
    /// <summary>
    /// ["<c>min_volume_30d</c>"] Min volume30d
    /// </summary>
    [JsonPropertyName("min_volume_30d")]
    public decimal MinVolume30d { get; set; }
    /// <summary>
    /// ["<c>taker_fee_rate</c>"] Taker fee rate
    /// </summary>
    [JsonPropertyName("taker_fee_rate")]
    public decimal TakerFeeRate { get; set; }
    /// <summary>
    /// ["<c>maker_fee_rate</c>"] Maker fee rate
    /// </summary>
    [JsonPropertyName("maker_fee_rate")]
    public decimal MakerFeeRate { get; set; }
}

