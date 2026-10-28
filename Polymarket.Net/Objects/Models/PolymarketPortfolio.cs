using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Portfolio
/// </summary>
public record PolymarketPortfolio
{
    /// <summary>
    /// ["<c>positions</c>"] Positions
    /// </summary>
    [JsonPropertyName("positions")]
    public PolymarketPortfolioPosition[] Positions { get; set; } = [];
    /// <summary>
    /// ["<c>equity</c>"] Equity
    /// </summary>
    [JsonPropertyName("equity")]
    public decimal Equity { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Portfolio position
/// </summary>
public record PolymarketPortfolioPosition
{
    /// <summary>
    /// ["<c>instrument_id</c>"] Instrument id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long InstrumentId { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>size</c>"] Quantity
    /// </summary>
    [JsonPropertyName("size")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>entry_price</c>"] Entry price
    /// </summary>
    [JsonPropertyName("entry_price")]
    public decimal EntryPrice { get; set; }
    /// <summary>
    /// ["<c>unrealized_pnl</c>"] Unrealized pnl
    /// </summary>
    [JsonPropertyName("unrealized_pnl")]
    public decimal UnrealizedPnl { get; set; }
    /// <summary>
    /// ["<c>return_on_equity</c>"] Return on equity
    /// </summary>
    [JsonPropertyName("return_on_equity")]
    public decimal ReturnOnEquity { get; set; }
}

