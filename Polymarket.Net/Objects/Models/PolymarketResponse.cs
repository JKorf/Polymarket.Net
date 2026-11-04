using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Models
{
    internal record PolymarketResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
