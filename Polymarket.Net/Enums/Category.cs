using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums;

/// <summary>
/// Category
/// </summary>
[JsonConverter(typeof(EnumConverter<Category>))]
public enum Category
{
    /// <summary>
    /// ["<c>pre_ipo</c>"] Pre Ipo
    /// </summary>
    [Map("pre_ipo")]
    PreIpo,
    /// <summary>
    /// ["<c>equity</c>"] Equity
    /// </summary>
    [Map("equity")]
    Equity,
    /// <summary>
    /// ["<c>commodity</c>"] Commodity
    /// </summary>
    [Map("commodity")]
    Commodity,
    /// <summary>
    /// ["<c>index</c>"] Index
    /// </summary>
    [Map("index")]
    Index,
    /// <summary>
    /// ["<c>crypto</c>"] Crypto
    /// </summary>
    [Map("crypto")]
    Crypto,
}
