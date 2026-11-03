using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// TP/SL
/// </summary>
public record PolymarketPerpsTpSlUpdate
{
    /// <summary>
    /// ["<c>oid</c>"] Order ID
    /// </summary>
    [JsonPropertyName("oid")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>st</c>"] Status
    /// </summary>
    [JsonPropertyName("st")]
    public TpSlStatus Status { get; set; }
    /// <summary>
    /// ["<c>reason</c>"] Expire reason
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}

