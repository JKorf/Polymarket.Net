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
    /// New market update
    /// </summary>
    public record PolymarketNewMarketUpdate : PolymarketSocketUpdate
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
        /// Event message
        /// </summary>
        [JsonPropertyName("event_message")]
        public PolymarketNewMarketEvent EventMessage { get; set; } = null!;
    }

    /// <summary>
    /// New market event
    /// </summary>
    public record PolymarketNewMarketEvent
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = string.Empty;
        /// <summary>
        /// Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
