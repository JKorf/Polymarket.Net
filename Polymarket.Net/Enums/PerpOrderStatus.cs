using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Order status
/// </summary>
[JsonConverter(typeof(EnumConverter<PerpOrderStatus>))]
public enum PerpOrderStatus
{
    /// <summary>
    /// ["<c>accepted</c>"] Accepted
    /// </summary>
    [Map("accepted")]
    Accepted,
    /// <summary>
    /// ["<c>open</c>"] Open
    /// </summary>
    [Map("open")]
    Open,
    /// <summary>
    /// ["<c>partial</c>"] Partial
    /// </summary>
    [Map("partial")]
    Partial,
    /// <summary>
    /// ["<c>filled</c>"] Filled
    /// </summary>
    [Map("filled")]
    Filled,
    /// <summary>
    /// ["<c>cancelled</c>"] Cancelled
    /// </summary>
    [Map("cancelled")]
    Cancelled,
}
