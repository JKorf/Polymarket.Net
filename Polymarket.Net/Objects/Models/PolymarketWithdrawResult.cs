using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Withdraw result
/// </summary>
public record PolymarketWithdrawResult
{
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    /// <summary>
    /// ["<c>withdraw_id</c>"] Withdraw id
    /// </summary>
    [JsonPropertyName("withdraw_id")]
    public long? WithdrawId { get; set; }
}

