using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums {
	/// <summary>
	/// Trade status
	/// </summary>
	[JsonConverter(typeof(EnumConverter<PositionSortBy>))]
	public enum PositionSortBy {
		/// <summary>
		/// ["<c>TOKENS</c>"] Tokens
		/// </summary>
		[Map("TOKENS")]
		Tokens,
        /// <summary>
        /// ["<c>CURRENT_VALUE</c>"] Current value
        /// </summary>
        [Map("CURRENT_VALUE")]
		CurrentValue,
        /// <summary>
        /// ["<c>PRICE</c>"] Price
        /// </summary>
        [Map("PRICE")]
        Price,
        /// <summary>
        /// ["<c>UNREALIZED_PNL</c>"] Unrealized profit and loss
        /// </summary>
        [Map("UNREALIZED_PNL")]
        UnrealizedPnl,
        /// <summary>
        /// ["<c>REALIZED_PNL</c>"] Realized profit and loss
        /// </summary>
        [Map("REALIZED_PNL")]
        RealizedPnl,
        /// <summary>
        /// ["<c>TOTAL_PNL</c>"] Total profit and loss
        /// </summary>
        [Map("TOTAL_PNL")]
        TotalPnl,
        /// <summary>
        /// ["<c>TIMESTAMP </c>"] Timestamp
        /// </summary>
        [Map("TIMESTAMP ")]
		Timestamp
	}
}
