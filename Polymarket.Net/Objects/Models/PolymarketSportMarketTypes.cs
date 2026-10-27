using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    internal record PolymarketSportMarketTypes
    {
        /// <summary>
        /// ["<c>marketTypes</c>"] Tags
        /// </summary>
        [JsonPropertyName("marketTypes")]
        public string[] MarketTypes { get; set; } = [];
    }
}
