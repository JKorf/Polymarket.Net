using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// 
/// </summary>
public record PolymarketAccountStats
{
    /// <summary>
    /// ["<c>volume_7d</c>"] Volume 7 days
    /// </summary>
    [JsonPropertyName("volume_7d")]
    public decimal Volume7d { get; set; }
    /// <summary>
    /// ["<c>taker_volume_7d</c>"] Taker volume 7 days
    /// </summary>
    [JsonPropertyName("taker_volume_7d")]
    public decimal TakerVolume7d { get; set; }
    /// <summary>
    /// ["<c>maker_volume_7d</c>"] Maker volume 7 days
    /// </summary>
    [JsonPropertyName("maker_volume_7d")]
    public decimal MakerVolume7d { get; set; }
    /// <summary>
    /// ["<c>account_maker_share_7d</c>"] Account maker share 7 days
    /// </summary>
    [JsonPropertyName("account_maker_share_7d")]
    public decimal AccountMakerShare7d { get; set; }
    /// <summary>
    /// ["<c>entity_maker_share_7d</c>"] Entity maker share 7 days
    /// </summary>
    [JsonPropertyName("entity_maker_share_7d")]
    public decimal? EntityMakerShare7d { get; set; }
    /// <summary>
    /// ["<c>entity_id</c>"] Entity id
    /// </summary>
    [JsonPropertyName("entity_id")]
    public long? EntityId { get; set; }
    /// <summary>
    /// ["<c>entity_name</c>"] Entity name
    /// </summary>
    [JsonPropertyName("entity_name")]
    public string? EntityName { get; set; } = string.Empty;
}

