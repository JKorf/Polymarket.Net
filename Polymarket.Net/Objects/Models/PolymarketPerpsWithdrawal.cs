using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Withdrawal info
/// </summary>
public record PolymarketPerpsWithdrawal
{
    /// <summary>
    /// ["<c>withdraw_id</c>"] Withdraw id
    /// </summary>
    [JsonPropertyName("withdraw_id")]
    public long WithdrawId { get; set; }
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
    /// ["<c>to</c>"] To
    /// </summary>
    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public WithdrawStatus Status { get; set; }
    /// <summary>
    /// ["<c>hash</c>"] Hash
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;
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

