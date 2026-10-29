using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketPerpsSocketRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("req")]
        public string Request { get; set; } = string.Empty;
    }

    internal record PolymarketPerpsOpSocketRequest : PolymarketPerpsSocketRequest
    {
        [JsonPropertyName("op")]
        public Parameters Op { get; set; } = default!;
    }

    internal record PolymarketPerpsChannelSocketRequest : PolymarketPerpsSocketRequest
    {
        [JsonPropertyName("chs")]
        public string[] Channels { get; set; } = [];
    }

    internal record PolymarketPerpsAuthSocketRequest : PolymarketPerpsOpSocketRequest
    {
        public string Signature { get; set; } = string.Empty;
        public long Salt { get; set; }
        public long Timestamp { get; set; }
    }
}
