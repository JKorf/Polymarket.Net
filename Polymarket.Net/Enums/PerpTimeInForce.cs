using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Time in force
/// </summary>
[JsonConverter(typeof(EnumConverter<PerpTimeInForce>))]
public enum PerpTimeInForce
{
    /// <summary>
    /// ["<c>gtc</c>"] Good till canceled
    /// </summary>
    [Map("gtc")]
    GoodTillCanceled,
    /// <summary>
    /// ["<c>ioc</c>"] Immediate or cancel
    /// </summary>
    [Map("ioc")]
    ImmediateOrCancel,
    /// <summary>
    /// ["<c>fok</c>"] Fill or kill
    /// </summary>
    [Map("fok")]
    FillOrKill,
}
