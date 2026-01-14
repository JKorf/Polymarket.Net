using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientGammaApiAccount : IPolymarketRestClientGammaApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PolymarketRestClientGammaApi _baseClient;

        internal PolymarketRestClientGammaApiAccount(PolymarketRestClientGammaApi baseClient)
        {
            _baseClient = baseClient;
        }

    }
}
