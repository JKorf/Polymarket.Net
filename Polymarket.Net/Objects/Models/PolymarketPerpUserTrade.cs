using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// User trade info
/// </summary>
public record PolymarketPerpUserTrade
{
    /// <summary>
    /// ["<c>trade_id</c>"] Trade id
    /// </summary>
    [JsonPropertyName("trade_id")]
    public long TradeId { get; set; }
    /// <summary>
    /// ["<c>order_id</c>"] Order id
    /// </summary>
    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>instrument_id</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public PositionSide Side { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>quantity</c>"] Quantity
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>taker</c>"] Taker
    /// </summary>
    [JsonPropertyName("taker")]
    public bool Taker { get; set; }
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>fee_asset</c>"] Fee asset
    /// </summary>
    [JsonPropertyName("fee_asset")]
    public string FeeAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>previous_size</c>"] Previous quantity
    /// </summary>
    [JsonPropertyName("previous_size")]
    public decimal PreviousQuantity { get; set; }
    /// <summary>
    /// ["<c>previous_entry_price</c>"] Previous entry price
    /// </summary>
    [JsonPropertyName("previous_entry_price")]
    public decimal PreviousEntryPrice { get; set; }
    /// <summary>
    /// ["<c>pnl</c>"] Profit and loss
    /// </summary>
    [JsonPropertyName("pnl")]
    public decimal Pnl { get; set; }
    /// <summary>
    /// ["<c>liquidation</c>"] Liquidation
    /// </summary>
    [JsonPropertyName("liquidation")]
    public bool Liquidation { get; set; }
    /// <summary>
    /// ["<c>adl</c>"] Auto deleverage
    /// </summary>
    [JsonPropertyName("adl")]
    public bool Adl { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>hash</c>"] Hash
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>builder_fee</c>"] Builder fee
    /// </summary>
    [JsonPropertyName("builder_fee")]
    public decimal BuilderFee { get; set; }
    /// <summary>
    /// ["<c>total_fee</c>"] Total fee
    /// </summary>
    [JsonPropertyName("total_fee")]
    public decimal TotalFee { get; set; }
    /// <summary>
    /// ["<c>settlement</c>"] Settlement
    /// </summary>
    [JsonPropertyName("settlement")]
    public bool Settlement { get; set; }
}

