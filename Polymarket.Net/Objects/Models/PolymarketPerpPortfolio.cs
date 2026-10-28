using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Portfolio
/// </summary>
public record PolymarketPerpPortfolio
{
    /// <summary>
    /// ["<c>positions</c>"] Positions
    /// </summary>
    [JsonPropertyName("positions")]
    public PolymarketPerpPosition[] Positions { get; set; } = [];
    /// <summary>
    /// ["<c>margin</c>"] Margin
    /// </summary>
    [JsonPropertyName("margin")]
    public PolymarketPerpMargin Margin { get; set; } = null!;
    /// <summary>
    /// ["<c>withdrawable</c>"] Withdrawable
    /// </summary>
    [JsonPropertyName("withdrawable")]
    public decimal Withdrawable { get; set; }
    /// <summary>
    /// ["<c>in_liquidation</c>"] In liquidation
    /// </summary>
    [JsonPropertyName("in_liquidation")]
    public bool InLiquidation { get; set; }
    /// <summary>
    /// ["<c>fee_tier</c>"] Fee tier
    /// </summary>
    [JsonPropertyName("fee_tier")]
    public int FeeTier { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Position
/// </summary>
public record PolymarketPerpPosition
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
    /// ["<c>leverage</c>"] Leverage
    /// </summary>
    [JsonPropertyName("leverage")]
    public decimal Leverage { get; set; }
    /// <summary>
    /// ["<c>cross</c>"] Cross
    /// </summary>
    [JsonPropertyName("cross")]
    public bool Cross { get; set; }
    /// <summary>
    /// ["<c>initial_margin</c>"] Initial margin
    /// </summary>
    [JsonPropertyName("initial_margin")]
    public decimal InitialMargin { get; set; }
    /// <summary>
    /// ["<c>maintenance_margin</c>"] Maintenance margin
    /// </summary>
    [JsonPropertyName("maintenance_margin")]
    public decimal MaintenanceMargin { get; set; }
    /// <summary>
    /// ["<c>position_value</c>"] Position value
    /// </summary>
    [JsonPropertyName("position_value")]
    public decimal PositionValue { get; set; }
    /// <summary>
    /// ["<c>liquidation_price</c>"] Liquidation price
    /// </summary>
    [JsonPropertyName("liquidation_price")]
    public decimal LiquidationPrice { get; set; }
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
    /// <summary>
    /// ["<c>cumulative_funding</c>"] Cumulative funding
    /// </summary>
    [JsonPropertyName("cumulative_funding")]
    public decimal CumulativeFunding { get; set; }
}

/// <summary>
/// Margin
/// </summary>
public record PolymarketPerpMargin
{
    /// <summary>
    /// ["<c>total_account_value</c>"] Total account value
    /// </summary>
    [JsonPropertyName("total_account_value")]
    public decimal TotalAccountValue { get; set; }
    /// <summary>
    /// ["<c>available_order_margin</c>"] Available order margin
    /// </summary>
    [JsonPropertyName("available_order_margin")]
    public decimal AvailableOrderMargin { get; set; }
    /// <summary>
    /// ["<c>total_initial_margin</c>"] Total initial margin
    /// </summary>
    [JsonPropertyName("total_initial_margin")]
    public decimal TotalInitialMargin { get; set; }
    /// <summary>
    /// ["<c>total_maintenance_margin</c>"] Total maintenance margin
    /// </summary>
    [JsonPropertyName("total_maintenance_margin")]
    public decimal TotalMaintenanceMargin { get; set; }
    /// <summary>
    /// ["<c>total_position_value</c>"] Total position value
    /// </summary>
    [JsonPropertyName("total_position_value")]
    public decimal TotalPositionValue { get; set; }
}

