using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Rate limit info
/// </summary>
public record PolymarketRateLimit
{
    /// <summary>
    /// ["<c>tier</c>"] Tier
    /// </summary>
    [JsonPropertyName("tier")]
    public string Tier { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>next_tier_threshold</c>"] Volume needed for next tier
    /// </summary>
    [JsonPropertyName("next_tier_threshold")]
    public decimal NextTierThreshold { get; set; }
    /// <summary>
    /// ["<c>usdc_volume</c>"] USDC trade volume
    /// </summary>
    [JsonPropertyName("usdc_volume")]
    public decimal? UsdcVolume { get; set; }
    /// <summary>
    /// ["<c>period</c>"] Volume window in days
    /// </summary>
    [JsonPropertyName("period")]
    public int Period { get; set; }
    /// <summary>
    /// ["<c>tier_details</c>"] Tier details
    /// </summary>
    [JsonPropertyName("tier_details")]
    public PolymarketRateLimitTier TierDetails { get; set; } = null!;
}

/// <summary>
/// Rate limit tier
/// </summary>
public record PolymarketRateLimitTier
{
    /// <summary>
    /// ["<c>name</c>"] Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>volume_threshold</c>"] Volume threshold
    /// </summary>
    [JsonPropertyName("volume_threshold")]
    public decimal VolumeThreshold { get; set; }
    /// <summary>
    /// ["<c>cancel_rate_per_sec</c>"] Cancel rate per second
    /// </summary>
    [JsonPropertyName("cancel_rate_per_sec")]
    public int CancelRatePerSecond { get; set; }
    /// <summary>
    /// ["<c>order_rate_per_sec</c>"] Order rate per second
    /// </summary>
    [JsonPropertyName("order_rate_per_sec")]
    public int OrderRatePerSecond { get; set; }
    /// <summary>
    /// ["<c>order_burst</c>"] Order burst
    /// </summary>
    [JsonPropertyName("order_burst")]
    public int OrderBurst { get; set; }
    /// <summary>
    /// ["<c>cancel_burst</c>"] Cancel burst
    /// </summary>
    [JsonPropertyName("cancel_burst")]
    public int CancelBurst { get; set; }
    /// <summary>
    /// ["<c>allow_negative</c>"] Allow negative
    /// </summary>
    [JsonPropertyName("allow_negative")]
    public bool AllowNegative { get; set; }
}

