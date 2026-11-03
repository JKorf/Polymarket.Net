using CryptoExchange.Net.Converters.SystemTextJson;
using Polymarket.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Order request
    /// </summary>
    public record PolymarketPerpsOrderRequest
    {
        /// <summary>
        /// ["<c>iid</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("iid")]
        public long SymbolId { get; set; }
        /// <summary>
        /// ["<c>buy</c>"] Side
        /// </summary>
        [JsonPropertyName("buy")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty"), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>p</c>"] Price
        /// </summary>
        [JsonPropertyName("p"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>tif</c>"] Time in force
        /// </summary>
        [JsonPropertyName("tif"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PerpTimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// ["<c>po</c>"] Post only
        /// </summary>
        [JsonPropertyName("po"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// ["<c>ro</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("ro"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>c</c>"] Client order id
        /// </summary>
        [JsonPropertyName("c"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>tr.market</c>"] Trigger order type
        /// </summary>
        public OrderType? TriggerOrderType { get; set; }
        /// <summary>
        /// ["<c>tr.trp</c>"] Trigger order price
        /// </summary>
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>tr.tpsl</c>"] Trigger type
        /// </summary>
        public TpSlType? TriggerType { get; set; }
    }
}
