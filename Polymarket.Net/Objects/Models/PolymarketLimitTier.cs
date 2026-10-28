using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Limit tier
/// </summary>
public record PolymarketLimitTier
{
    /// <summary>
    /// ["<c>min_volume_14d</c>"] Min volume14d
    /// </summary>
    [JsonPropertyName("min_volume_14d")]
    public decimal MinVolume14d { get; set; }
    /// <summary>
    /// ["<c>rate_per_minute_limit</c>"] Rate per minute limit
    /// </summary>
    [JsonPropertyName("rate_per_minute_limit")]
    public int RatePerMinuteLimit { get; set; }
    /// <summary>
    /// ["<c>rate_burst_limit</c>"] Rate burst limit
    /// </summary>
    [JsonPropertyName("rate_burst_limit")]
    public int RateBurstLimit { get; set; }
    /// <summary>
    /// ["<c>actions_per_minute_limit</c>"] Actions per minute limit
    /// </summary>
    [JsonPropertyName("actions_per_minute_limit")]
    public int ActionsPerMinuteLimit { get; set; }
    /// <summary>
    /// ["<c>actions_burst_limit</c>"] Actions burst limit
    /// </summary>
    [JsonPropertyName("actions_burst_limit")]
    public int ActionsBurstLimit { get; set; }
    /// <summary>
    /// ["<c>open_orders_limit</c>"] Open orders limit
    /// </summary>
    [JsonPropertyName("open_orders_limit")]
    public int OpenOrdersLimit { get; set; }
    /// <summary>
    /// ["<c>messages_per_minute</c>"] Messages per minute
    /// </summary>
    [JsonPropertyName("messages_per_minute")]
    public int MessagesPerMinute { get; set; }
}

