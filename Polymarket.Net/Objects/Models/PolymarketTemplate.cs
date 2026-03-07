using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Template
    /// </summary>
    public record PolymarketTemplate
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>eventTitle</c>"] Event title
        /// </summary>
        [JsonPropertyName("eventTitle")]
        public string EventTitle { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>eventSlug</c>"] Event slug
        /// </summary>
        [JsonPropertyName("eventSlug")]
        public string EventSlug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>eventImage</c>"] Event image
        /// </summary>
        [JsonPropertyName("eventImage")]
        public string EventImage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>marketTitle</c>"] Market title
        /// </summary>
        [JsonPropertyName("marketTitle")]
        public string MarketTitle { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>description</c>"] Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>resolutionSource</c>"] Resolution source
        /// </summary>
        [JsonPropertyName("resolutionSource")]
        public string ResolutionSource { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>negRisk</c>"] Negative risk
        /// </summary>
        [JsonPropertyName("negRisk")]
        public bool NegativeRisk { get; set; }
        /// <summary>
        /// ["<c>sortBy</c>"] Sort by
        /// </summary>
        [JsonPropertyName("sortBy")]
        public string SortBy { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>showMarketImages</c>"] Show market images
        /// </summary>
        [JsonPropertyName("showMarketImages")]
        public bool ShowMarketImages { get; set; }
        /// <summary>
        /// ["<c>seriesSlug</c>"] Series slug
        /// </summary>
        [JsonPropertyName("seriesSlug")]
        public string SeriesSlug { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>outcomes</c>"] Outcomes
        /// </summary>
        [JsonPropertyName("outcomes")]
        public string Outcomes { get; set; } = string.Empty;
    }
}
