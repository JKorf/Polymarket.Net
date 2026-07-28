using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
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
		/// <para>
		/// Docs:<br />
		/// <a href="https://docs.polymarket.com/api-reference/core/get-current-positions-for-a-user" /><br />
		/// Endpoint:<br />
		/// GET /positions
		/// </para>
		/// </summary>
		/// <param name="user">["<c>user</c>"] By user</param>
		/// <param name="market">["<c>market</c>"] Comma-separated list of condition IDs. Mutually exclusive with eventId.</param>
		/// <param name="eventId">["<c>eventId</c>"] Comma-separated list of event IDs. Mutually exclusive with market.</param>
		/// <param name="sizeThreshold">["<c>sizeThreshold</c>"]</param>
		/// <param name="redeemable">["<c>redeemable</c>"]</param>
		/// <param name="mergeable">["<c>mergeable</c>"]</param>
		/// <param name="limit">["<c>limit</c>"]</param>
		/// <param name="offset">["<c>offset</c>"]</param>
		/// <param name="sortBy">["<c>sortBy</c>"]</param>
		/// <param name="sortDirection">["<c>sortDirection</c>"]</param>
		/// <param name="title">["<c>title</c>"]</param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<HttpResult<PolymarketPosition[]>> GetPositionsAsync(
			string user, 
			string? market = null, 
			string? eventId = null,
			decimal? sizeThreshold = null,
			bool? redeemable = null,
			bool? mergeable = null,
			int? limit = null,
			int? offset = null,
			PositionSortBy? sortBy = null,
			SortDirection? sortDirection = null,
			string? title = null,
			CancellationToken ct = default);

		/// <summary>
		/// Get list of closed positions for a user
		/// <para>
		/// Docs:<br />
		/// <a href="https://docs.polymarket.com/api-reference/core/get-closed-positions-for-a-user" /><br />
		/// Endpoint:<br />
		/// GET /closed-positions
		/// </para>
		/// </summary>
		/// <param name="user">["<c>user</c>"] By user</param>
		/// <param name="market">["<c>market</c>"] Comma-separated list of condition IDs. Mutually exclusive with eventId.</param>
		/// <param name="eventId">["<c>eventId</c>"] Comma-separated list of event IDs. Mutually exclusive with market.</param>
		/// <param name="limit">["<c>limit</c>"]</param>
		/// <param name="offset">["<c>offset</c>"]</param>
		/// <param name="sortBy">["<c>sortBy</c>"]</param>
		/// <param name="sortDirection">["<c>sortDirection</c>"]</param>
		/// <param name="title">["<c>title</c>"]</param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<HttpResult<PolymarketClosedPosition[]>> GetClosedPositionsAsync(
			string user,
			string? market = null,
			string? eventId = null,
			int? limit = null,
			int? offset = null,
			ClosedPositionSortBy? sortBy = null,
			SortDirection? sortDirection = null,
			string? title = null,
			CancellationToken ct = default);
	}
}