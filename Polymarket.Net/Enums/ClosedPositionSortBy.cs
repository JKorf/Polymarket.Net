using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums {
	/// <summary>
	/// Closed position sort criteria
	/// </summary>
	[JsonConverter(typeof(EnumConverter<ClosedPositionSortBy>))]
	public enum ClosedPositionSortBy {
		/// <summary>
		/// ["<c>REALIZEDPNL</c>"] RealizedPnl
		/// </summary>
		[Map("REALIZEDPNL")]
		RealizedPnl,
		/// <summary>
		/// ["<c>TITLE</c>"] Title
		/// </summary>
		[Map("TITLE")]
		Title,
		/// <summary>
		/// ["<c>PRICE</c>"] Price
		/// </summary>
		[Map("PRICE")]
		Price,
		/// <summary>
		/// ["<c>AVGPRICE</c>"] AvgPrice
		/// </summary>
		[Map("AVGPRICE")]
		AvgPrice,
		/// <summary>
		/// ["<c>TIMESTAMP</c>"] Timestamp
		/// </summary>
		[Map("TIMESTAMP")]
		Timestamp
	}
}
