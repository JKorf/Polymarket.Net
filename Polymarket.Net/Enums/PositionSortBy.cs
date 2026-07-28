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
		/// ["<c>CURRENT</c>"] Current
		/// </summary>
		[Map("CURRENT")]
		Current,
		/// <summary>
		/// ["<c>INITIAL</c>"] Initial
		/// </summary>
		[Map("INITIAL")]
		Initial,
		/// <summary>
		/// ["<c>CASHPNL</c>"] CashPnl
		/// </summary>
		[Map("CASHPNL")]
		CashPnl,
		/// <summary>
		/// ["<c>PERCENTPNL</c>"] PercentPnl
		/// </summary>
		[Map("PERCENTPNL")]
		PercentPnl,
		/// <summary>
		/// ["<c>TITLE</c>"] Title
		/// </summary>
		[Map("TITLE")]
		Title,
		/// <summary>
		/// ["<c>RESOLVING</c>"] Resolving
		/// </summary>
		[Map("RESOLVING")]
		Resolving,
		/// <summary>
		/// ["<c>PRICE</c>"] Price
		/// </summary>
		[Map("PRICE")]
		Price,
		/// <summary>
		/// ["<c>AVGPRICE</c>"] AvgPrice
		/// </summary>
		[Map("AVGPRICE")]
		AvgPrice
	}
}
