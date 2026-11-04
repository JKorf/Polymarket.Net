using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Transfer direction
/// </summary>
[JsonConverter(typeof(EnumConverter<TransferDirection>))]
public enum TransferDirection
{
    /// <summary>
    /// ["<c>in</c>"] In
    /// </summary>
    [Map("in")]
    In,
    /// <summary>
    /// ["<c>out</c>"] Out
    /// </summary>
    [Map("out")]
    Out,
}
