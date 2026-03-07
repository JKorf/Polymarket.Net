using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Chat info
    /// </summary>
    public record PolymarketChat
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>channelId</c>"] Channel id
        /// </summary>
        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>channelName</c>"] Channel name
        /// </summary>
        [JsonPropertyName("channelName")]
        public string ChannelName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>channelImage</c>"] Channel image
        /// </summary>
        [JsonPropertyName("channelImage")]
        public string ChannelImage { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>live</c>"] Live
        /// </summary>
        [JsonPropertyName("live")]
        public bool Live { get; set; }
        /// <summary>
        /// ["<c>startTime</c>"] Start time
        /// </summary>
        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// ["<c>endTime</c>"] End time
        /// </summary>
        [JsonPropertyName("endTime")]
        public DateTime? EndTime { get; set; }
    }

}
