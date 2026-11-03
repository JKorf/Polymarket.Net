using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Position status
/// </summary>
[JsonConverter(typeof(EnumConverter<PositionStatus>))]
public enum PositionStatus
{
    /// <summary>
    /// ["<c>OPEN</c>"] Open
    /// </summary>
    [Map("OPEN")]
    Open,
    /// <summary>
    /// ["<c>REDEEMABLE</c>"] Redeemable
    /// </summary>
    [Map("REDEEMABLE")]
    Redeemable,
    /// <summary>
    /// ["<c>REDEEMABLE_LOST</c>"] Redeemable lost
    /// </summary>
    [Map("REDEEMABLE_LOST")]
    RedeemableLost,
    /// <summary>
    /// ["<c>MERGEABLE</c>"] Mergeable
    /// </summary>
    [Map("MERGEABLE")]
    Mergeable,
    /// <summary>
    /// ["<c>CLOSED</c>"] Closed
    /// </summary>
    [Map("CLOSED")]
    Closed
}
