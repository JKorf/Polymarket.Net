using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums {
	/// <summary>
	/// Trade status
	/// </summary>
	[JsonConverter(typeof(EnumConverter<SortDirection>))]
	public enum SortDirection {
		/// <summary>
		/// ["<c>ASC</c>"] Asc
		/// </summary>
		[Map("ASC")]
		Asc,
		/// <summary>
		/// ["<c>DESC</c>"] Desc
		/// </summary>
		[Map("DESC")]
		Desc
	}
}
