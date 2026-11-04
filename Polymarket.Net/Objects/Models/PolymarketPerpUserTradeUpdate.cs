using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// User trade info
/// </summary>
public record PolymarketPerpUserTradeUpdate
{
    /// <summary>
    /// ["<c>tid</c>"] Trade id
    /// </summary>
    [JsonPropertyName("tid")]
    public long TradeId { get; set; }
    /// <summary>
    /// ["<c>oid</c>"] Order id
    /// </summary>
    [JsonPropertyName("oid")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>coid</c>"] Client order id
    /// </summary>
    [JsonPropertyName("coid")]
    public string? ClientOrderId { get; set; }
    /// <summary>
    /// ["<c>iid</c>"] Symbol id
    /// </summary>
    [JsonPropertyName("iid")]
    public long SymbolId { get; set; }
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public PositionSide Side { get; set; }
    /// <summary>
    /// ["<c>p</c>"] Price
    /// </summary>
    [JsonPropertyName("p")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>qty</c>"] Quantity
    /// </summary>
    [JsonPropertyName("qty")]
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
    /// ["<c>fea</c>"] Fee asset
    /// </summary>
    [JsonPropertyName("fea")]
    public string FeeAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>psz</c>"] Previous quantity
    /// </summary>
    [JsonPropertyName("psz")]
    public decimal PreviousQuantity { get; set; }
    /// <summary>
    /// ["<c>pep</c>"] Previous entry price
    /// </summary>
    [JsonPropertyName("pep")]
    public decimal PreviousEntryPrice { get; set; }
    /// <summary>
    /// ["<c>pnl</c>"] Profit and loss
    /// </summary>
    [JsonPropertyName("pnl")]
    public decimal Pnl { get; set; }
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
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
}

