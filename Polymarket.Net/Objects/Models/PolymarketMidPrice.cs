using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Mid price
    /// </summary>
    public record PolymarketMidPrice
    {
        /// <summary>
        /// ["<c>mid</c>"] Mid
        /// </summary>
        [JsonPropertyName("mid")]
        public decimal Mid { get; set; }
    }


}
