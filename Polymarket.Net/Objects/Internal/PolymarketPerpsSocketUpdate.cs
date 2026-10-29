using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketPerpsSocketUpdate
    {
        [JsonPropertyName("ch")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
    }

    internal record PolymarketPerpsSocketUpdate<T> : PolymarketPerpsSocketUpdate
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
