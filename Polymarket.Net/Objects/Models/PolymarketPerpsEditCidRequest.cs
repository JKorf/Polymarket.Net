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
    public record PolymarketPerpsEditCidRequest
    {
        /// <summary>
        /// ["<c>coid</c>"] Client Order id
        /// </summary>
        public string ClientOrderId { get; set; } = string.Empty;
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
