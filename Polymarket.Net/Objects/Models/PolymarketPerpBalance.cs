using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Balance info
/// </summary>
public record PolymarketPerpBalance
{
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>balance</c>"] Balance
    /// </summary>
    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
    /// <summary>
    /// ["<c>value</c>"] Value
    /// </summary>
    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}

