using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Credentials
/// </summary>
public record PolymarketPerpsCredentials
{
    /// <summary>
    /// ["<c>address</c>"] Address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>keys</c>"] Keys
    /// </summary>
    [JsonPropertyName("keys")]
    public PolymarketPerpsCredential[] Keys { get; set; } = [];
}

/// <summary>
/// Credential info
/// </summary>
public record PolymarketPerpsCredential
{
    /// <summary>
    /// ["<c>proxy</c>"] Proxy
    /// </summary>
    [JsonPropertyName("proxy")]
    public string Proxy { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>expiry</c>"] Expiry
    /// </summary>
    [JsonPropertyName("expiry")]
    public DateTime Expiry { get; set; }
    /// <summary>
    /// ["<c>label</c>"] Label
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

