using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientGammaApiTrading : IPolymarketRestClientGammaApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PolymarketRestClientGammaApi _baseClient;
        private readonly ILogger _logger;

        internal PolymarketRestClientGammaApiTrading(ILogger logger, PolymarketRestClientGammaApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

    }
}
