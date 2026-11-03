using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Tp/Sl type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TpSlType>))]
    public enum TpSlType
    {
        /// <summary>
        /// ["<c>tp</c>"] Take profit
        /// </summary>
        [Map("tp")]
        TakeProfit,
        /// <summary>
        /// ["<c>sl</c>"] Stop loss

        /// </summary>
        [Map("sl")]
        StopLoss
    }
}
