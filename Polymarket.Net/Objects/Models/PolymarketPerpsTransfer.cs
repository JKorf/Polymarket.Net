using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Transfer history
/// </summary>
public record PolymarketPerpsTransfer
{
    /// <summary>
    /// ["<c>transfer_id</c>"] Transfer id
    /// </summary>
    [JsonPropertyName("transfer_id")]
    public long TransferId { get; set; }
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
    /// ["<c>direction</c>"] Direction
    /// </summary>
    [JsonPropertyName("direction")]
    public TransferDirection Direction { get; set; }
    /// <summary>
    /// ["<c>counterparty</c>"] Counterparty
    /// </summary>
    [JsonPropertyName("counterparty")]
    public string Counterparty { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>created_timestamp</c>"] Created timestamp
    /// </summary>
    [JsonPropertyName("created_timestamp")]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// ["<c>label</c>"] Label
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

