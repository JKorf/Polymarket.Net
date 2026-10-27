using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Order update type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderUpdateType>))]
    public enum OrderUpdateType
    {
        /// <summary>
        /// ["<c>PLACEMENT</c>"] Order placed
        /// </summary>
        [Map("PLACEMENT")]
        Placed,
        /// <summary>
        /// ["<c>UPDATE</c>"] Order updated
        /// </summary>
        [Map("UPDATE")]
        Update,
        /// <summary>
        /// ["<c>CANCELLATION</c>"] Order canceled
        /// </summary>
        [Map("CANCELLATION")]
        Canceled,

    }
}
