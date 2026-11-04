using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Deposit info
/// </summary>
public record PolymarketPerpsDepositUpdate
{
    /// <summary>
    /// ["<c>hash</c>"] Hash
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>amount</c>"] Quantity
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public DepositStatus Status { get; set; }
}

