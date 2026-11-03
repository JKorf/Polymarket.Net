using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Tp/Sl grouping
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TpSlGrouping>))]
    public enum TpSlGrouping
    {
        /// <summary>
        /// ["<c>order</c>"] Order
        /// </summary>
        [Map("order")]
        Order,
        /// <summary>
        /// ["<c>position</c>"] Position
        /// </summary>
        [Map("position")]
        Position
    }
}
