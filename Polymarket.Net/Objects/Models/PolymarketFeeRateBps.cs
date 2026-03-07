using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Fee rate in BPS
    /// </summary>
    public record PolymarketFeeRateBps
    {
        /// <summary>
        /// ["<c>base_fee</c>"] Fee rate in BPS
        /// </summary>
        [JsonPropertyName("base_fee")]
        public decimal BaseFee { get; set; }
    }
}
