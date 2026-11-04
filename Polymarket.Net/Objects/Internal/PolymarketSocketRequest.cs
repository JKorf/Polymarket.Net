using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    internal class PolymarketSocketRequest
    {
        [JsonPropertyName("assets_ids")]
        public string[] TokenIds { get; set; } = [];
        [JsonPropertyName("operation")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("custom_feature_enabled")]
        public bool CustomFeatureEnabled { get; set; }
    }
}
