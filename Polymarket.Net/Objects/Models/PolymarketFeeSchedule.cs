using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Fee schedule info
/// </summary>
public record PolymarketFeeSchedule
{
    /// <summary>
    /// ["<c>exponent</c>"] Exponent
    /// </summary>
    [JsonPropertyName("exponent")]
    public decimal Exponent { get; set; }
    
    /// <summary>
    /// ["<c>rate</c>"] Rate
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
    
    /// <summary>
    /// ["<c>takerOnly</c>"] TaketOnly
    /// </summary>
    [JsonPropertyName("takerOnly")]
    public bool TaketOnly { get; set; }
    
    /// <summary>
    /// ["<c>rebateRate</c>"] RebateRate
    /// </summary>
    [JsonPropertyName("rebateRate")]
    public decimal RebateRate { get; set; }
}