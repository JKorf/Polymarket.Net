using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Event volumes
/// </summary>
public record PolymarketEventVolume
{
    /// <summary>
    /// ["<c>conditions</c>"] Markets
    /// </summary>
    [JsonPropertyName("conditions")]
    public PolymarketEventVolumeInfo[] Markets { get; set; } = [];
    /// <summary>
    /// ["<c>taker_volume_total</c>"] Taker volume total
    /// </summary>
    [JsonPropertyName("taker_volume_total")]
    public decimal TakerVolumeTotal { get; set; }
}

/// <summary>
/// Event volume
/// </summary>
public record PolymarketEventVolumeInfo
{
    /// <summary>
    /// ["<c>condition_id</c>"] Market id
    /// </summary>
    [JsonPropertyName("condition_id")]
    public string MarketId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>taker_volume</c>"] Taker volume
    /// </summary>
    [JsonPropertyName("taker_volume")]
    public decimal TakerVolume { get; set; }
}

