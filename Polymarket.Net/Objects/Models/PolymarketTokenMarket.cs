using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Market ref
/// </summary>
public record PolymarketTokenMarket
{
    /// <summary>
    /// ["<c>condition_id</c>"] Market id
    /// </summary>
    [JsonPropertyName("condition_id")]
    public string MarketId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>primary_token_id</c>"] The primary (Yes) token id
    /// </summary>
    [JsonPropertyName("primary_token_id")]
    public string PrimaryTokenId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>secondary_token_id</c>"] The secondary (No) token id
    /// </summary>
    [JsonPropertyName("secondary_token_id")]
    public string SecondaryTokenId { get; set; } = string.Empty;
}

