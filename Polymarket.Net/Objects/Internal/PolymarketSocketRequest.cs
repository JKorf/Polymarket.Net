using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Internal
{
    internal class PolymarketSocketRequest
    {
        [JsonPropertyName("assets_ids")]
        public string[] Assets { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("custom_feature_enabled")]
        public bool CustomFeatureEnabled { get; set; }
    }
}
