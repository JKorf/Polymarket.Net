using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi.Rest
{
    internal class PolymarketRestClientPerpsApiAccount : IPolymarketRestClientPerpsApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPerpsApi _client;

        public PolymarketRestClientPerpsApiAccount(PolymarketRestClientPerpsApi client)
        {
            _client = client;
        }

    }
}
