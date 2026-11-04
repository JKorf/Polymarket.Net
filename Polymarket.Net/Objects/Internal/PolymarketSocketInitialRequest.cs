using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    internal class PolymarketSocketInitialRequest
    {
        [JsonPropertyName("auth"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PolymarketSocketAuth? Auth { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("custom_feature_enabled"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? CustomFeatureEnabled { get; set; }
    }

    internal class PolymarketSocketAuth
    {
        [JsonPropertyName("apikey")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("secret")]
        public string ApiSecret { get; set; } = string.Empty;
        [JsonPropertyName("passphrase")]
        public string ApiPass { get; set; } = string.Empty;
    }
}
