using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Asset type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetType>))]
    public enum AssetType
    {
        /// <summary>
        /// ["<c>COLLATERAL</c>"] Collateral
        /// </summary>
        [Map("COLLATERAL")]
        Collateral,
        /// <summary>
        /// ["<c>CONDITIONAL</c>"] Conditional
        /// </summary>
        [Map("CONDITIONAL")]
        Conditional,
        /// <summary>
        /// ["<c>CONDITIONAL-V2</c>"] Conditional V2
        /// </summary>
        [Map("CONDITIONAL-V2")]
        ConditionalV2,
    }
}
