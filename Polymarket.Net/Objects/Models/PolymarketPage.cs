using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Data page
    /// </summary>
    public record PolymarketPage<T>
    {
        /// <summary>
        /// ["<c>next_cursor</c>"] Pagination cursor
        /// </summary>
        [JsonPropertyName("next_cursor")]
        public string NextPageCursor { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>limit</c>"] Max number of results
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        /// <summary>
        /// ["<c>count</c>"] Number of results
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
        /// <summary>
        /// ["<c>data</c>"] Page data
        /// </summary>
        [JsonPropertyName("data")]
        public T[] Data { get; set; } = [];
    }
}
