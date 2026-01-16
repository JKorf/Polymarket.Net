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
    /// Tick size update
    /// </summary>
    public record PolymarketTickSizeUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// Old tick size
        /// </summary>
        [JsonPropertyName("old_tick_size")]
        public decimal OldTickSize { get; set; }
        /// <summary>
        /// New tick size
        /// </summary>
        [JsonPropertyName("new_tick_size")]
        public decimal NewTickSize { get; set; }
    }
}
