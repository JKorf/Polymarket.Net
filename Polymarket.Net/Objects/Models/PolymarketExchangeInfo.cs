using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Exchange info
/// </summary>
public record PolymarketExchangeInfo
{
    /// <summary>
    /// ["<c>name</c>"] Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>version</c>"] Version
    /// </summary>
    [JsonPropertyName("version")]
    public decimal Version { get; set; }
    /// <summary>
    /// ["<c>chain_id</c>"] Network id
    /// </summary>
    [JsonPropertyName("chain_id")]
    public long NetworkId { get; set; }
    /// <summary>
    /// ["<c>contract</c>"] Contract
    /// </summary>
    [JsonPropertyName("contract")]
    public string Contract { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>cancel_only</c>"] Cancel only
    /// </summary>
    [JsonPropertyName("cancel_only")]
    public bool CancelOnly { get; set; }
    /// <summary>
    /// ["<c>engine_version</c>"] Engine version
    /// </summary>
    [JsonPropertyName("engine_version")]
    public string EngineVersion { get; set; } = string.Empty;
}

