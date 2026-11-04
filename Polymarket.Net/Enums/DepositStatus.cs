using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Deposit status
/// </summary>
[JsonConverter(typeof(EnumConverter<DepositStatus>))]
public enum DepositStatus
{
    /// <summary>
    /// ["<c>pending</c>"] Pending
    /// </summary>
    [Map("pending")]
    Pending,
    /// <summary>
    /// ["<c>confirmed</c>"] Confirmed
    /// </summary>
    [Map("confirmed")]
    Confirmed,
    /// <summary>
    /// ["<c>removed</c>"] Removed
    /// </summary>
    [Map("removed")]
    Removed,
}
