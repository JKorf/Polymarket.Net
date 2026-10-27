using Polymarket.Net.Enums;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketPriceRequest
    {
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; } = string.Empty;
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
    }
}
