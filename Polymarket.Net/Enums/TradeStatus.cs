using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Trade status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeStatus>))]
    public enum TradeStatus
    {
        /// <summary>
        /// ["<c>MATCHED</c>", "<c>TRADE_STATUS_MATCHED</c>"] Matched
        /// </summary>
        [Map("MATCHED", "TRADE_STATUS_MATCHED")]
        Matched,
        /// <summary>
        /// ["<c>MINED</c>", "<c>TRADE_STATUS_MINED</c>"] Mined
        /// </summary>
        [Map("MINED", "TRADE_STATUS_MINED")]
        Mined,
        /// <summary>
        /// ["<c>CONFIRMED</c>", "<c>TRADE_STATUS_CONFIRMED</c>"] Confirmed
        /// </summary>
        [Map("CONFIRMED", "TRADE_STATUS_CONFIRMED")]
        Confirmed,
        /// <summary>
        /// ["<c>RETRYING</c>", "<c>TRADE_STATUS_RETRYING</c>"] Retrying
        /// </summary>
        [Map("RETRYING", "TRADE_STATUS_RETRYING")]
        Retrying,
        /// <summary>
        /// ["<c>FAILED</c>", "<c>TRADE_STATUS_FAILED</c>"] Failed
        /// </summary>
        [Map("FAILED", "TRADE_STATUS_FAILED")]
        Failed,
    }
}
