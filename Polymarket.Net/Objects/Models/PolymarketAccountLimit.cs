using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Account limits
/// </summary>
public record PolymarketAccountLimit
{
    /// <summary>
    /// ["<c>actions_per_minute</c>"] Actions per minute
    /// </summary>
    [JsonPropertyName("actions_per_minute")]
    public int ActionsPerMinute { get; set; }
    /// <summary>
    /// ["<c>actions_per_minute_limit</c>"] Actions per minute limit
    /// </summary>
    [JsonPropertyName("actions_per_minute_limit")]
    public int ActionsPerMinuteLimit { get; set; }
    /// <summary>
    /// ["<c>actions_burst</c>"] Actions burst
    /// </summary>
    [JsonPropertyName("actions_burst")]
    public int ActionsBurst { get; set; }
    /// <summary>
    /// ["<c>actions_burst_limit</c>"] Actions burst limit
    /// </summary>
    [JsonPropertyName("actions_burst_limit")]
    public int ActionsBurstLimit { get; set; }
    /// <summary>
    /// ["<c>open_orders</c>"] Open orders
    /// </summary>
    [JsonPropertyName("open_orders")]
    public int OpenOrders { get; set; }
    /// <summary>
    /// ["<c>open_orders_limit</c>"] Open orders limit
    /// </summary>
    [JsonPropertyName("open_orders_limit")]
    public int OpenOrdersLimit { get; set; }
    /// <summary>
    /// ["<c>reset</c>"] Reset
    /// </summary>
    [JsonPropertyName("reset")]
    public DateTime? Reset { get; set; }
    /// <summary>
    /// ["<c>messages_per_minute</c>"] Messages per minute
    /// </summary>
    [JsonPropertyName("messages_per_minute")]
    public int MessagesPerMinute { get; set; }
}

