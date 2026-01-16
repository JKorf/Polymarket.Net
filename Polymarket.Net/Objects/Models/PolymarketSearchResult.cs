using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Search results
    /// </summary>
    public record PolymarketSearchResult
    {
        /// <summary>
        /// Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[] Events { get; set; } = [];
        /// <summary>
        /// Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public PolymarketTag[] Tags { get; set; } = [];
        ///// <summary>
        ///// Profiles
        ///// </summary>
        //[JsonPropertyName("profiles")]
        //public PolymarketProfile[] Profiles { get; set; } = [];
        /// <summary>
        /// Pagination data
        /// </summary>
        [JsonPropertyName("pagination")]
        public PolymarketSearchPagination Pagination { get; set; } = null!;
    }

    /// <summary>
    /// Pagination data
    /// </summary>
    public record PolymarketSearchPagination
    {
        /// <summary>
        /// Has more
        /// </summary>
        [JsonPropertyName("hasMore")]
        public bool HasMore { get; set; }
        /// <summary>
        /// Total results
        /// </summary>
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }
    }
}
