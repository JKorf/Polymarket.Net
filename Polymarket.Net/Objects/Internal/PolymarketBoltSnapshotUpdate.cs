using System.Text.Json.Serialization;

namespace Polymarket.Net.Objects.Internal
{
    /// <summary>
    /// Snapshot update
    /// </summary>
    public record PolymarketBoltSnapshotUpdate<T>
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>source</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
