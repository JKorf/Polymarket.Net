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
    /// Order edit request
    /// </summary>
    public record PolymarketPerpsEditRequest
    {
        /// <summary>
        /// ["<c>oid</c>"] Order id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>p</c>"] Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
