using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi.Rest
{
    internal class PolymarketRestClientPerpsApiTrading : IPolymarketRestClientPerpsApiTrading
    {

        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPerpsApi _client;

        public PolymarketRestClientPerpsApiTrading(PolymarketRestClientPerpsApi client)
        {
            _client = client;
        }

    }
}
