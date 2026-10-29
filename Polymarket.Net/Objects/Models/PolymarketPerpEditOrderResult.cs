using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Edit order result
/// </summary>
public record PolymarketPerpEditOrderResult
{
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>error</c>"] Error
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }
    /// <summary>
    /// ["<c>order</c>"] Order
    /// </summary>
    [JsonPropertyName("order")]
    public PolymarketPerpOrder Order { get; set; } = null!;
}
