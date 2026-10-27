using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TagStatus>))]
    public enum TagStatus
    {
        /// <summary>
        /// ["<c>active</c>"] Active
        /// </summary>
        [Map("active")]
        Active,
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// ["<c>all</c>"] All
        /// </summary>
        [Map("all")]
        All
    }
}
