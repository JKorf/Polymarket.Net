using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientGammaApiExchangeData : IPolymarketRestClientGammaApiExchangeData
    {
        private readonly PolymarketRestClientGammaApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal PolymarketRestClientGammaApiExchangeData(ILogger logger, PolymarketRestClientGammaApi baseClient)
        {
            _baseClient = baseClient;
        
        }

        #region Get Sport Teams

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptional("league", league);
            parameters.AddOptional("name", name);
            parameters.AddOptional("abbreviation", abbreviation);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", 20);
            parameters.Add("offset", 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "teams", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await _baseClient.SendAsync<PolymarketSportsTeam[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
