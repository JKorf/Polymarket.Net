using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Transfer result
/// </summary>
public record PolymarketPerpsTranfserResult
{
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>transfer_id</c>"] Transfer id
    /// </summary>
    [JsonPropertyName("transfer_id")]
    public long TransferId { get; set; }
}

