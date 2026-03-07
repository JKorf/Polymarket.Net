using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Price info
    /// </summary>
    public record PolymarketBuySellPrice
    {
        /// <summary>
        /// ["<c>BUY</c>"] Buy price
        /// </summary>
        [JsonPropertyName("BUY")]
        public decimal? BuyPrice { get; set; }
        /// <summary>
        /// ["<c>SELL</c>"] Sell price
        /// </summary>
        [JsonPropertyName("SELL")]
        public decimal? SellPrice { get; set; }
    }
}
