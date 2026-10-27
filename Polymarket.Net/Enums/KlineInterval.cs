using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Kline interval
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
    /// <summary>
    /// ["<c>1s</c>"] One second
    /// </summary>
    [Map("1s")]
    OneSecond,
    /// <summary>
    /// ["<c>1m</c>"] One minute
    /// </summary>
    [Map("1m")]
    OneMinute,
    /// <summary>
    /// ["<c>5m</c>"] Five minutes
    /// </summary>
    [Map("5m")]
    FiveMinutes,
    /// <summary>
    /// ["<c>15m</c>"] Fifteen minutes
    /// </summary>
    [Map("15m")]
    FifteenMinutes,
    /// <summary>
    /// ["<c>30m</c>"] Thirty minutes
    /// </summary>
    [Map("30m")]
    ThirtyMinutes,
    /// <summary>
    /// ["<c>1h</c>"] One hour
    /// </summary>
    [Map("1h")]
    OneHour,
    /// <summary>
    /// ["<c>4h</c>"] Four hours
    /// </summary>
    [Map("4h")]
    FourHours,
    /// <summary>
    /// ["<c>6h</c>"] Six hours
    /// </summary>
    [Map("6h")]
    SixHours,
    /// <summary>
    /// ["<c>12h</c>"] Twelve hours
    /// </summary>
    [Map("12h")]
    TwelveHours,
    /// <summary>
    /// ["<c>1d</c>"] One day
    /// </summary>
    [Map("1d")]
    OneDay,
    /// <summary>
    /// ["<c>1w</c>"] One week
    /// </summary>
    [Map("1w")]
    OneWeek,
}
