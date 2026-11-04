using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Kline info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<PolymarketKline>))]
    public record PolymarketKline
    {
        /// <summary>
        /// Open timestamp
        /// </summary>
        [ArrayProperty(0)]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [ArrayProperty(1)]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [ArrayProperty(2)]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [ArrayProperty(3)]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [ArrayProperty(4)]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [ArrayProperty(5)]
        public decimal Volume { get; set; }
        /// <summary>
        /// Trades
        /// </summary>
        [ArrayProperty(6)]
        public int Trades { get; set; }
    }
}
