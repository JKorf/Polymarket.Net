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
    /// User order update
    /// </summary>
    public record PolymarketOrderUpdate : PolymarketSocketUpdate
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>owner</c>"] Owner
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>market</c>"] Market id
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset_id</c>"] Asset id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>order_owner</c>"] Order owner
        /// </summary>
        [JsonPropertyName("order_owner")]
        public string OrderOwner { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>original_size</c>"] Original quantity
        /// </summary>
        [JsonPropertyName("original_size")]
        public decimal OriginalQuantity { get; set; }
        /// <summary>
        /// ["<c>size_matched</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("size_matched")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>associate_trades</c>"] Order trades
        /// </summary>
        [JsonPropertyName("associate_trades")]
        public string[] Trades { get; set; } = [];
        /// <summary>
        /// ["<c>outcome</c>"] Outcome
        /// </summary>
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>type</c>"] Order update type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderUpdateType UpdateType { get; set; }
        /// <summary>
        /// ["<c>created_at</c>"] Create time
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>expiration</c>"] Expiration
        /// </summary>
        [JsonPropertyName("expiration")]
        public string Expiration { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order_type</c>"] Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>maker_address</c>"] Maker address
        /// </summary>
        [JsonPropertyName("maker_address")]
        public string MakerAddress { get; set; } = string.Empty;
    }
}
