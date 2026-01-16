using Polymarket.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Market resolved update
    /// </summary>
    public record PolymarketMarketResolvedUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Market Id
        /// </summary>
        [JsonPropertyName("market")]
        public string MarketId { get; set; } = string.Empty;
        /// <summary>
        /// Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Asset ids
        /// </summary>
        [JsonPropertyName("assets_ids")]
        public string[] AssetIds { get; set; } = [];
        /// <summary>
        /// Outcomes
        /// </summary>
        [JsonPropertyName("outcomes")]
        public string[] Outcomes { get; set; } = [];
        /// <summary>
        /// Question
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// Winning asset id
        /// </summary>
        [JsonPropertyName("winning_asset_id")]
        public string WinningAssetId { get; set; } = string.Empty;
        /// <summary>
        /// Winning outcome
        /// </summary>
        [JsonPropertyName("winning_outcome")]
        public string WinningOutcome { get; set; } = string.Empty;
        /// <summary>
        /// Event message
        /// </summary>
        [JsonPropertyName("event_message")]
        public PolymarketNewMarketEvent EventMessage { get; set; } = null!;
    }
}
