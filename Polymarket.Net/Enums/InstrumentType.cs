using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Instrument type
/// </summary>
[JsonConverter(typeof(EnumConverter<InstrumentType>))]
public enum InstrumentType
{
    /// <summary>
    /// ["<c>perpetual</c>"] Perpetual
    /// </summary>
    [Map("perpetual")]
    Perpetual,
}
