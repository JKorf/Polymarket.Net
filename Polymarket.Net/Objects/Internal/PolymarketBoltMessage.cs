using CryptoExchange.Net.Objects;
using System;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    internal record PolymarketBoltMessage
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = string.Empty;
        [JsonPropertyName("rid")]
        public string RequestId { get; set; } = string.Empty;
        [JsonPropertyName("code"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Code { get; set; }
    }

    internal record PolymarketBoltAuthRequest : PolymarketBoltMessage
    {
        [JsonPropertyName("auth")]
        public PolymarketBoltAuth Auth { get; set; } = default!;
    }

    internal record PolymarketBoltAuth
    {
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("secret")]
        public string Secret { get; set; } = string.Empty;
        [JsonPropertyName("passphrase")]
        public string Passphrase { get; set; } = string.Empty;
    }


    internal record PolymarketBoltSubscribeRequest : PolymarketBoltMessage
    {
        [JsonPropertyName("subscriptions")]
        public PolymarketBoltSub[] Subscriptions { get; set; } = [];
    }

    internal record PolymarketBoltSub
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("filter")]
        public Parameters? Filters { get; set; }
    }

    internal record PolymarketBoltUpdate
    {
        [JsonPropertyName("v")]
        public long V { get; set; }
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        [JsonPropertyName("snapshot")]
        public bool Snapshot { get; set; }
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }

    internal record PolymarketBoltUpdate<T> : PolymarketBoltUpdate
    {
        [JsonPropertyName("payload")]
        public T Payload { get; set; } = default!;
    }
}
