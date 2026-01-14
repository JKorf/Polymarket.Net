using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.GammaApi
{
    /// <summary>
    /// Polymarket Gamma exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPolymarketRestClientGammaApiExchangeData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

    }
}
