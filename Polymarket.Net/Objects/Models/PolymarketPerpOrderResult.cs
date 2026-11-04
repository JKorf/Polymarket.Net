using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Order result
/// </summary>
public record PolymarketPerpOrderResult
{
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>error</c>"] Error message
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }
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
}

