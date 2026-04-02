using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.DataApi
{
    /// <summary>
    /// Polymarket Data API endpoints
    /// </summary>
    public interface IPolymarketRestClientDataApi : IRestApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Get list of all positions for a user
        /// /// <para>
        /// Endpoint:<br />
        /// GET /positions
        /// </para>
        /// </summary>
        /// <param name="user">["<c>user</c>"] By user</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<PolymarketPosition[]>> GetPositionsAsync(string user, CancellationToken ct = default);
    }
}