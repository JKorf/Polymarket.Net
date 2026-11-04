using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Enums {
	/// <summary>
	/// Sort direction
	/// </summary>
	[JsonConverter(typeof(EnumConverter<PerpSortDirection>))]
	public enum PerpSortDirection
    {
		/// <summary>
		/// ["<c>asc</c>"] Ascending
		/// </summary>
		[Map("asc")]
        Ascending,
        /// <summary>
        /// ["<c>desc</c>"] Descending
        /// </summary>
        [Map("desc")]
        Descending
    }
}
