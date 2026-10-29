using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    internal record PolymarketTime
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
