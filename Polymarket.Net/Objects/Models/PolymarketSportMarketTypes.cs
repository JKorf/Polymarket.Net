using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    internal record PolymarketSportMarketTypes
    {
        /// <summary>
        /// ["<c>marketTypes</c>"] Tags
        /// </summary>
        [JsonPropertyName("marketTypes")]
        public string[] MarketTypes { get; set; } = [];
    }
}
