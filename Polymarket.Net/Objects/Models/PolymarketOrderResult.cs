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
    /// Order result
    /// </summary>
    public record PolymarketOrderResult
    {
        /// <summary>
        /// ["<c>success</c>"] Whether order was successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        /// <summary>
        /// ["<c>errorMsg</c>"] Error message if failed
        /// </summary>
        [JsonPropertyName("errorMsg")]
        public string? Error { get; set; }
        /// <summary>
        /// ["<c>orderID</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>takingAmount</c>"] Order taker quantity executed
        /// </summary>
        [JsonPropertyName("takingAmount")]
        public decimal? TakingQuantity { get; set; }
        /// <summary>
        /// ["<c>makingAmount</c>"] Order maker quantity executed
        /// </summary>
        [JsonPropertyName("makingAmount")]
        public decimal? MakingQuantity { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus? Status { get; set; }
        /// <summary>
        /// ["<c>transactionsHashes</c>"] Hashes of trades which were executed immediately
        /// </summary>
        [JsonPropertyName("transactionsHashes")]
        public string[] TradeHashes { get; set; } = [];
    }
}
