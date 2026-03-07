using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Token info
    /// </summary>
    public record PolymarketOrderBook
    {
        /// <summary>
        /// ["<c>market</c>"] Market
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset_id</c>"] Asset/token id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>hash</c>"] Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public PolymarketBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public PolymarketBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>min_order_size</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("min_order_size")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>tick_size</c>"] Tick quantity
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal TickQuantity { get; set; }
        /// <summary>
        /// ["<c>neg_risk</c>"] Negative risk enabled
        /// </summary>
        [JsonPropertyName("neg_risk")]
        public bool NegativeRisk { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record PolymarketBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
    }
}
