using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Registered
/// </summary>
public record PolymarketRegistered
{
    /// <summary>
    /// ["<c>registered</c>"] Registered
    /// </summary>
    [JsonPropertyName("registered")]
    public bool Registered { get; set; }
}

