using Polymarket.Net.Enums;
using System;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Market order request. For market orders, the amount parameter represents
    /// the USDC spend for BUY orders or the asset amount for SELL orders.
    /// </summary>
    public record PolymarketMarketOrderRequest
    {
        /// <summary>
        /// Token id
        /// </summary>
        public string TokenId { get; set; } = string.Empty;
        /// <summary>
        /// Order side
        /// </summary>
        public OrderSide Side { get; set; }
        /// <summary>
        /// Amount: BUY = USDC spend amount, SELL = asset/shares amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Fee rate BPS
        /// </summary>
        public long? FeeRateBps { get; set; }
        /// <summary>
        /// Taker address
        /// </summary>
        public string? TakerAddress { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        public long? ClientOrderId { get; set; }
        /// <summary>
        /// Expiration
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// Nonce
        /// </summary>
        public long? Nonce { get; set; }
    }
}
