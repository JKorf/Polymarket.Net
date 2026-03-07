using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Balance allowance
    /// </summary>
    public record PolymarketBalanceAllowance
    {
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Balance in USD
        /// </summary>
        public decimal BalanceUsd => Balance / 1000000;
        /// <summary>
        /// ["<c>allowances</c>"] Allowances
        /// </summary>
        [JsonPropertyName("allowances")]
        public Dictionary<string, string> Allowances { get; set; } = new();
    }
}
