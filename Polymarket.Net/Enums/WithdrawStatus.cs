using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Withdrawal status
/// </summary>
[JsonConverter(typeof(EnumConverter<WithdrawStatus>))]
public enum WithdrawStatus
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
    /// <summary>
    /// ["<c>failed</c>"] Failed
    /// </summary>
    [Map("failed")]
    Failed,
}
