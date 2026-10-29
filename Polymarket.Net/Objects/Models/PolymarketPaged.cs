using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Paged data
    /// </summary>
    public record PolymarketPaged<T>
    {
        /// <summary>
        /// ["<c>data</c>"] Data
        /// </summary>
        [JsonPropertyName("data")]
        public T[] Data { get; set; } = default!;
        /// <summary>
        /// ["<c>more</c>"] Indicates if there are more pages
        /// </summary>
        [JsonPropertyName("more")]
        public bool More { get; set; }
        /// <summary>
        /// ["<c>cursor</c>"] Pagination cursor
        /// </summary>
        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }
    }
}
