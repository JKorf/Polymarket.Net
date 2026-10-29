using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Deposit info
/// </summary>
public record PolymarketPerpsDeposit
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
    /// ["<c>from</c>"] From
    /// </summary>
    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>to</c>"] To
    /// </summary>
    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public DepositStatus Status { get; set; }
    /// <summary>
    /// ["<c>confirmations</c>"] Confirmations
    /// </summary>
    [JsonPropertyName("confirmations")]
    public int Confirmations { get; set; }
    /// <summary>
    /// ["<c>required_confirmations</c>"] Required confirmations
    /// </summary>
    [JsonPropertyName("required_confirmations")]
    public int RequiredConfirmations { get; set; }
    /// <summary>
    /// ["<c>created_timestamp</c>"] Created timestamp
    /// </summary>
    [JsonPropertyName("created_timestamp")]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// ["<c>confirmed_timestamp</c>"] Confirmed timestamp
    /// </summary>
    [JsonPropertyName("confirmed_timestamp")]
    public DateTime? ConfirmTime { get; set; }
}

