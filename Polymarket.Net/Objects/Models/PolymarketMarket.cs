using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Market info
    /// </summary>
    public record PolymarketMarket
    {
        /// <summary>
        /// ["<c>condition_id</c>"] Condition id
        /// </summary>
        [JsonPropertyName("condition_id")]
        public string ConditionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>rewards</c>"] Rewards
        /// </summary>
        [JsonPropertyName("rewards")]
        public PolymarketMarketReward Rewards { get; set; } = null!;
        /// <summary>
        /// ["<c>tokens</c>"] Tokens
        /// </summary>
        [JsonPropertyName("tokens")]
        public PolymarketMarketToken[] Tokens { get; set; } = [];
        /// <summary>
        /// ["<c>active</c>"] Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
        /// <summary>
        /// ["<c>archived</c>"] Archived
        /// </summary>
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
        /// <summary>
        /// ["<c>accepting_orders</c>"] Accepting orders
        /// </summary>
        [JsonPropertyName("accepting_orders")]
        public bool AcceptingOrders { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record PolymarketMarketReward
    {
        /// <summary>
        /// ["<c>rates</c>"] Rates
        /// </summary>
        [JsonPropertyName("rates")]
        public PolymarketMarketRewardRates[] Rates { get; set; } = [];
        /// <summary>
        /// ["<c>min_size</c>"] Min quantity
        /// </summary>
        [JsonPropertyName("min_size")]
        public decimal MinQuantity { get; set; }
        /// <summary>
        /// ["<c>max_spread</c>"] Max spread
        /// </summary>
        [JsonPropertyName("max_spread")]
        public decimal MaxSpread { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record PolymarketMarketRewardRates
    {
        /// <summary>
        /// ["<c>asset_address</c>"] Asset address
        /// </summary>
        [JsonPropertyName("asset_address")]
        public string AssetAddress { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>rewards_daily_rate</c>"] Rewards daily rate
        /// </summary>
        [JsonPropertyName("rewards_daily_rate")]
        public decimal RewardsDailyRate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record PolymarketMarketToken
    {
        /// <summary>
        /// ["<c>token_id</c>"] Token id
        /// </summary>
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>outcome</c>"] Outcome
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>winner</c>"] Winner
        /// </summary>
        [JsonPropertyName("winner")]
        public bool Winner { get; set; }
    }


}
