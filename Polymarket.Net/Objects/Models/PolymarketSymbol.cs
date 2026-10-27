using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Symbol info
/// </summary>
public record PolymarketSymbol
{
    /// <summary>
    /// ["<c>instrument_id</c>"] Instrument id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long InstrumentId { get; set; }
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
    /// ["<c>isolated_only</c>"] Isolated only
    /// </summary>
    [JsonPropertyName("isolated_only")]
    public bool IsolatedOnly { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>base_asset</c>"] Base asset
    /// </summary>
    [JsonPropertyName("base_asset")]
    public string BaseAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quote_asset</c>"] Quote asset
    /// </summary>
    [JsonPropertyName("quote_asset")]
    public string QuoteAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>funding_interval</c>"] Funding interval
    /// </summary>
    [JsonPropertyName("funding_interval")]
    public string FundingInterval { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quantity_decimals</c>"] Quantity decimals
    /// </summary>
    [JsonPropertyName("quantity_decimals")]
    public int QuantityDecimals { get; set; }
    /// <summary>
    /// ["<c>price_decimals</c>"] Price decimals
    /// </summary>
    [JsonPropertyName("price_decimals")]
    public int PriceDecimals { get; set; }
    /// <summary>
    /// ["<c>price_bounds</c>"] Price bounds
    /// </summary>
    [JsonPropertyName("price_bounds")]
    public decimal PriceBounds { get; set; }
    /// <summary>
    /// ["<c>liquidation_fee</c>"] Liquidation fee
    /// </summary>
    [JsonPropertyName("liquidation_fee")]
    public decimal LiquidationFee { get; set; }
    /// <summary>
    /// ["<c>max_order_count</c>"] Max order count
    /// </summary>
    [JsonPropertyName("max_order_count")]
    public int MaxOrderCount { get; set; }
    /// <summary>
    /// ["<c>min_notional</c>"] Min notional
    /// </summary>
    [JsonPropertyName("min_notional")]
    public decimal MinNotional { get; set; }
    /// <summary>
    /// ["<c>max_market_notional</c>"] Max market notional
    /// </summary>
    [JsonPropertyName("max_market_notional")]
    public decimal MaxMarketNotional { get; set; }
    /// <summary>
    /// ["<c>max_limit_notional</c>"] Max limit notional
    /// </summary>
    [JsonPropertyName("max_limit_notional")]
    public decimal MaxLimitNotional { get; set; }
    /// <summary>
    /// ["<c>max_leverage</c>"] Max leverage
    /// </summary>
    [JsonPropertyName("max_leverage")]
    public decimal MaxLeverage { get; set; }
    /// <summary>
    /// ["<c>risk_tiers</c>"] Risk tiers
    /// </summary>
    [JsonPropertyName("risk_tiers")]
    public PolymarketSymbolRiskTier[] RiskTiers { get; set; } = [];
    /// <summary>
    /// ["<c>ui_live_time</c>"] Ui live time
    /// </summary>
    [JsonPropertyName("ui_live_time")]
    public DateTime UiLiveTime { get; set; }
    /// <summary>
    /// ["<c>close_only</c>"] Close only
    /// </summary>
    [JsonPropertyName("close_only")]
    public bool CloseOnly { get; set; }
    /// <summary>
    /// ["<c>display_symbol</c>"] Display symbol
    /// </summary>
    [JsonPropertyName("display_symbol")]
    public string DisplaySymbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>settlement</c>"] Settlement
    /// </summary>
    [JsonPropertyName("settlement")]
    public PolymarketSymbolSettlement? Settlement { get; set; }
}

/// <summary>
/// Risk tier
/// </summary>
public record PolymarketSymbolRiskTier
{
    /// <summary>
    /// ["<c>lower_bound</c>"] Lower bound
    /// </summary>
    [JsonPropertyName("lower_bound")]
    public decimal LowerBound { get; set; }
    /// <summary>
    /// ["<c>max_leverage</c>"] Max leverage
    /// </summary>
    [JsonPropertyName("max_leverage")]
    public decimal MaxLeverage { get; set; }
}

/// <summary>
/// Settlement info
/// </summary>
public record PolymarketSymbolSettlement
{
    /// <summary>
    /// ["<c>sequence</c>"] Sequence
    /// </summary>
    [JsonPropertyName("sequence")]
    public long Sequence { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>insurance_debit</c>"] Insurance debit
    /// </summary>
    [JsonPropertyName("insurance_debit")]
    public decimal InsuranceDebit { get; set; }
}

