using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Market status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarketStatus>))]
    public enum MarketStatus
    {
        /// <summary>
        /// ["<c>open</c>"] Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// ["<c>unknown</c>"] Unknown
        /// </summary>
        [Map("unknown")]
        Unknown
    }
}
