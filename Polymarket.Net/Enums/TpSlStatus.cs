using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Enums
{
    /// <summary>
    /// Tp/Sl status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TpSlStatus>))]
    public enum TpSlStatus
    {
        /// <summary>
        /// ["<c>untriggered</c>"] Untriggered
        /// </summary>
        [Map("untriggered")]
        Untriggered,
        /// <summary>
        /// ["<c>armed</c>"] Armed
        /// </summary>
        [Map("armed")]
        Armed,
        /// <summary>
        /// ["<c>activated</c>"] Activated
        /// </summary>
        [Map("activated")]
        Activated,
        /// <summary>
        /// ["<c>cancelled</c>"] Cancelled
        /// </summary>
        [Map("cancelled")]
        Cancelled,
        /// <summary>
        /// ["<c>expired</c>"] Expired
        /// </summary>
        [Map("expired")]
        Expired
    }
}
