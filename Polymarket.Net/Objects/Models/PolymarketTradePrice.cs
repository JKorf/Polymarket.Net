using Polymarket.Net.Enums;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Last trade price
    /// </summary>
    public record PolymarketTradePrice
    {
        /// <summary>
        /// ["<c>price</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal LastTradePrice { get; set; }

        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>token_id</c>"] Token id
        /// </summary>
        [JsonPropertyName("token_id")]
        public string? TokenId { get; set; }
    }
}
