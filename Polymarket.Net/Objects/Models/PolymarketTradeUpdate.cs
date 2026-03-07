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
    /// User trade update
    /// </summary>
    public record PolymarketTradeUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// ["<c>id</c>"] Trade id
        /// </summary>
        [JsonPropertyName("id")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>taker_order_id</c>"] Taker order id
        /// </summary>
        [JsonPropertyName("taker_order_id")]
        public string TakerOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_owner</c>"] Trade owner
        /// </summary>
        [JsonPropertyName("trade_owner")]
        public string TradeOwner { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>market</c>"] Condition/market id
        /// </summary>
        [JsonPropertyName("market")]
        public string ConditionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset_id</c>"] Asset/token id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>maker_address</c>"] Maker address
        /// </summary>
        [JsonPropertyName("maker_address")]
        public string MakerAddress { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transaction_hash</c>"] Transaction hash
        /// </summary>
        [JsonPropertyName("transaction_hash")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>fee_rate_bps</c>"] Fee rate BPS
        /// </summary>
        [JsonPropertyName("fee_rate_bps")]
        public decimal FeeRateBps { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Trade status
        /// </summary>
        [JsonPropertyName("status")]
        public TradeStatus Status { get; set; }
        /// <summary>
        /// ["<c>match_time</c>"] Matching time
        /// </summary>
        [JsonPropertyName("match_time")]
        public DateTime MatchTime { get; set; }
        /// <summary>
        /// ["<c>bucket_index</c>"] Bucket index
        /// </summary>
        [JsonPropertyName("bucket_index")]
        public long BucketIndex { get; set; }
        /// <summary>
        /// ["<c>last_update</c>"] Last update time
        /// </summary>
        [JsonPropertyName("last_update")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>outcome</c>"] Outcome string
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>owner</c>"] API key of the taker of the trade
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trader_side</c>"] Role
        /// </summary>
        [JsonPropertyName("trader_side")]
        public TradeRole TradeRole { get; set; }
        /// <summary>
        /// ["<c>maker_orders</c>"] List of the maker trades the taker trade was filled against
        /// </summary>
        [JsonPropertyName("maker_orders")]
        public PolymarketOrderBase[] MakerOrders { get; set; } = [];
    }
}
