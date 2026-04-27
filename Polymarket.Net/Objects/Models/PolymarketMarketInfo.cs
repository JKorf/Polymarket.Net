using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Market info
/// </summary>
public record PolymarketMarketInfo
{
    /// <summary>
    /// ["<c>gst</c>"] Game start time
    /// </summary>
    [JsonPropertyName("gst")]
    public DateTime? GameStartTime { get; set; }
    /// <summary>
    /// ["<c>t</c>"] Tokens
    /// </summary>
    [JsonPropertyName("t")]
    public PolymarketMarketInfoToken[] Tokens { get; set; } = [];
    /// <summary>
    /// ["<c>mos</c>"] Minimal order size
    /// </summary>
    [JsonPropertyName("mos")]
    public decimal MinOrderSize { get; set; }
    /// <summary>
    /// ["<c>mts</c>"] Minimal tick size
    /// </summary>
    [JsonPropertyName("mts")]
    public decimal MinTickSize { get; set; }
    /// <summary>
    /// ["<c>mbf</c>"] Maker base fee in basis points
    /// </summary>
    [JsonPropertyName("mbf")]
    public int MakerBaseFee { get; set; }
    /// <summary>
    /// ["<c>tbf</c>"] Taker base fee in basis points
    /// </summary>
    [JsonPropertyName("tbf")]
    public int TakerBaseFee { get; set; }
    /// <summary>
    /// ["<c>rfqe</c>"] Request for quote enabled on market
    /// </summary>
    [JsonPropertyName("rfqe")]
    public bool RfqEnabled { get; set; }
    /// <summary>
    /// ["<c>itode</c>"] Whether taker order delay is enabled
    /// </summary>
    [JsonPropertyName("itode")]
    public bool TakerOrderDelayEnabled { get; set; }
    /// <summary>
    /// ["<c>ibce</c>"] Whether Blockaid check is enabled
    /// </summary>
    [JsonPropertyName("ibce")]
    public bool BlockaidEnabled { get; set; }
    /// <summary>
    /// ["<c>fd</c>"] Fee curve
    /// </summary>
    [JsonPropertyName("fd")]
    public PolymarketMarketInfoFees FeeCurve { get; set; } = null!;
    /// <summary>
    /// ["<c>oas</c>"] Minimum order age in seconds
    /// </summary>
    [JsonPropertyName("oas")]
    public decimal MinOrderAge { get; set; }
}

/// <summary>
/// Market token
/// </summary>
public record PolymarketMarketInfoToken
{
    /// <summary>
    /// ["<c>t</c>"] Token id
    /// </summary>
    [JsonPropertyName("t")]
    public string TokenId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>o</c>"] Outcome
    /// </summary>
    [JsonPropertyName("o")]
    public string Outcome { get; set; } = string.Empty;
}

/// <summary>
/// Fee info
/// </summary>
public record PolymarketMarketInfoFees
{
    /// <summary>
    /// ["<c>r</c>"] Fee rate
    /// </summary>
    [JsonPropertyName("r")]
    public decimal FeeRate { get; set; }
    /// <summary>
    /// ["<c>e</c>"] Curve exponent
    /// </summary>
    [JsonPropertyName("e")]
    public decimal Exponent { get; set; }
    /// <summary>
    /// ["<c>to</c>"] Taker only fee
    /// </summary>
    [JsonPropertyName("to")]
    public bool TakerOnly { get; set; }
}

