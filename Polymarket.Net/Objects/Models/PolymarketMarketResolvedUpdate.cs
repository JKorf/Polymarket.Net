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
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>market</c>"] Market Id
        /// </summary>
        [JsonPropertyName("market")]
        public string MarketId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>slug</c>"] Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>assets_ids</c>"] Asset ids
        /// </summary>
        [JsonPropertyName("assets_ids")]
        public string[] AssetIds { get; set; } = [];
        /// <summary>
        /// ["<c>outcomes</c>"] Outcomes
        /// </summary>
        [JsonPropertyName("outcomes")]
        public string[] Outcomes { get; set; } = [];
        /// <summary>
        /// ["<c>question</c>"] Question
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>winning_asset_id</c>"] Winning asset id
        /// </summary>
        [JsonPropertyName("winning_asset_id")]
        public string WinningAssetId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>winning_outcome</c>"] Winning outcome
        /// </summary>
        [JsonPropertyName("winning_outcome")]
        public string WinningOutcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>event_message</c>"] Event message
        /// </summary>
        [JsonPropertyName("event_message")]
        public PolymarketNewMarketEvent EventMessage { get; set; } = null!;
    }
}
