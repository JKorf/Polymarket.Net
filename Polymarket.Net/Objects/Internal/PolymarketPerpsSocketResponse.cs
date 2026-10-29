using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketPerpsSocketResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    internal record PolymarketPerpsSocketResponse<T> : PolymarketPerpsSocketResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
