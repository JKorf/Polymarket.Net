using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketTokenRequest
    {
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; } = string.Empty;
    }
}
