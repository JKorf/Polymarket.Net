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
        /// ["<c>events</c>"] Events
        /// </summary>
        [JsonPropertyName("events")]
        public PolymarketEvent[] Events { get; set; } = [];
        /// <summary>
        /// ["<c>tags</c>"] Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public PolymarketTag[] Tags { get; set; } = [];
        ///// <summary>
        ///// Profiles
        ///// </summary>
        //[JsonPropertyName("profiles")]
        //public PolymarketProfile[] Profiles { get; set; } = [];
        /// <summary>
        /// ["<c>pagination</c>"] Pagination data
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
        /// ["<c>hasMore</c>"] Has more
        /// </summary>
        [JsonPropertyName("hasMore")]
        public bool HasMore { get; set; }
        /// <summary>
        /// ["<c>totalResults</c>"] Total results
        /// </summary>
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }
    }
}
