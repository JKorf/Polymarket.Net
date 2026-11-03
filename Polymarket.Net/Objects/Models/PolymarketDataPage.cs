using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Data API page
    /// </summary>
    public record PolymarketDataPage<T>
    {
        /// <summary>
        /// ["<c>data</c>"] Data
        /// </summary>
        [JsonPropertyName("data")]
        public T[] Data { get; set; } = [];

        /// <summary>
        /// ["<c>pagination</c>"] Data
        /// </summary>
        [JsonPropertyName("pagination")]
        public PolymarketDataPagination Pagination { get; set; } = default!;
    }

    /// <summary>
    /// Pagination data
    /// </summary>
    public record PolymarketDataPagination
    {
        /// <summary>
        /// ["<c>limit</c>"] Max number of results
        /// </summary>
        public int? Limit { get; set; }
        /// <summary>
        /// ["<c>offset</c>"] Offset
        /// </summary>
        public int? Offset { get; set; }
        /// <summary>
        /// ["<c>has_more</c>"] Has more pages
        /// </summary>
        public bool HasMore { get; set; }
        /// <summary>
        /// ["<c>next_cursor</c>"] Next pagination cursor
        /// </summary>
        public string? NextCursor { get; set; }

    }
}
