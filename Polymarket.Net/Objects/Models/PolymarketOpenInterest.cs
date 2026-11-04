using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Open interest
/// </summary>
public record PolymarketOpenInterest
{
    /// <summary>
    /// ["<c>condition_id</c>"] Market id
    /// </summary>
    [JsonPropertyName("condition_id")]
    public string MarketId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>value</c>"] Value
    /// </summary>
    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}

