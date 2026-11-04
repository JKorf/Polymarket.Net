using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Resolution state
/// </summary>
[JsonConverter(typeof(EnumConverter<ResolutionStatus>))]
public enum ResolutionStatus
{
    /// <summary>
    /// ["<c>initialized</c>"] Initialized
    /// </summary>
    [Map("initialized")]
    Initialized,
    /// <summary>
    /// ["<c>posed</c>"] Posed
    /// </summary>
    [Map("posed")]
    Posed,
    /// <summary>
    /// ["<c>proposed</c>"] Proposed
    /// </summary>
    [Map("proposed")]
    Proposed,
    /// <summary>
    /// ["<c>challenged</c>"] Challenged
    /// </summary>
    [Map("challenged")]
    Challenged,
    /// <summary>
    /// ["<c>reproposed</c>"] Reproposed
    /// </summary>
    [Map("reproposed")]
    Reproposed,
    /// <summary>
    /// ["<c>disputed</c>"] Disputed
    /// </summary>
    [Map("disputed")]
    Disputed,
    /// <summary>
    /// ["<c>resolved</c>"] Resolved
    /// </summary>
    [Map("resolved")]
    Resolved,
}
