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
    /// Order info
    /// </summary>
    public record PolymarketPerpOrder
    {
        /// <summary>
        /// ["<c>oid</c>"] Order id
        /// </summary>
        [JsonPropertyName("oid")]
        public long OrderId { get; set; }
        [JsonInclude, JsonPropertyName("order_id")]
        internal long OrderIdInt { set => OrderId = value; }
        /// <summary>
        /// ["<c>iid</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("iid")]
        public long SymbolId { get; set; }
        [JsonInclude, JsonPropertyName("instrument_id")]
        internal long SymbolIdInt { set => SymbolId = value; }
        /// <summary>
        /// ["<c>buy</c>"] Buy
        /// </summary>
        [JsonPropertyName("buy")]
        public bool Buy { get; set; }
        /// <summary>
        /// ["<c>p</c>"] Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
        [JsonInclude, JsonPropertyName("price")]
        internal decimal PriceInt { set => Price = value; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        [JsonInclude, JsonPropertyName("quantity")]
        internal decimal QuantityInt { set => Quantity = value; }
        /// <summary>
        /// ["<c>tif</c>"] Time in force
        /// </summary>
        [JsonPropertyName("tif")]
        public PerpTimeInForce TimeInForce { get; set; }
        /// <summary>
        /// ["<c>po</c>"] Post only
        /// </summary>
        [JsonPropertyName("po")]
        public bool PostOnly { get; set; }
        [JsonInclude, JsonPropertyName("post_only")]
        internal bool PostOnlyInt { set => PostOnly = value; }
        /// <summary>
        /// ["<c>ro</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("ro")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>rest</c>"] Open quantity
        /// </summary>
        [JsonPropertyName("rest")]
        public decimal QuantityOpen { get; set; }
        [JsonInclude, JsonPropertyName("resting_quantity")]
        internal decimal QuantityOpenInt { set => QuantityOpen = value; }
        /// <summary>
        /// ["<c>fill</c>"] Filled quantity
        /// </summary>
        [JsonPropertyName("fill")]
        public decimal QuantityFilled { get; set; }
        [JsonInclude, JsonPropertyName("filled_quantity")]
        internal decimal QuantityFilledInt { set => QuantityFilled = value; }
        /// <summary>
        /// ["<c>cts</c>"] Create timestamp
        /// </summary>
        [JsonPropertyName("cts")]
        public DateTime CreateTime { get; set; }
        [JsonInclude, JsonPropertyName("created_timestamp")]
        internal DateTime CreateTimeInt { set => CreateTime = value; }
        /// <summary>
        /// ["<c>uts</c>"] Update timestamp
        /// </summary>
        [JsonPropertyName("uts")]
        public DateTime UpdateTime { get; set; }
        [JsonInclude, JsonPropertyName("updated_timestamp")]
        internal DateTime UpdateTimeInt { set => UpdateTime = value; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public PerpOrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>coid</c>"] Client order id
        /// </summary>
        [JsonPropertyName("coid")]
        public string? ClientOrderId { get; set; }
        [JsonInclude, JsonPropertyName("client_order_id")]
        internal string? ClientOrderIdInt { set => ClientOrderId = value; }
    }

}
