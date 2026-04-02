using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Clients.GammaApi;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Net.Http;
using Polymarket.Net.Interfaces.Clients.DataApi;
using Polymarket.Net.Clients.DataApi;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc cref="IPolymarketRestClient" />
    public class PolymarketRestClient : BaseRestClient<PolymarketEnvironment, PolymarketCredentials>, IPolymarketRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public IPolymarketRestClientClobApi ClobApi { get; }

        /// <inheritdoc />
        public IPolymarketRestClientGammaApi GammaApi { get; }

        /// <inheritdoc />
        public IPolymarketRestClientDataApi DataApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the PolymarketRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public PolymarketRestClient(Action<PolymarketRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the PolymarketRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public PolymarketRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<PolymarketRestOptions> options) : base(loggerFactory, "Polymarket")
        {
            Initialize(options.Value);
            
            ClobApi = AddApiClient(new PolymarketRestClientClobApi(_logger, httpClient, options.Value));
            GammaApi = AddApiClient(new PolymarketRestClientGammaApi(_logger, httpClient, options.Value));
            DataApi = AddApiClient(new PolymarketRestClientDataApi(_logger, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<PolymarketRestOptions> optionsDelegate)
        {
            PolymarketRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void UpdateL2Credentials(PolymarketCreds credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            var existingCreds = ((PolymarketRestClientClobApi)ClobApi).ApiCredentials
                ?? throw new InvalidOperationException("UpdateL2Credentials can not be called without having initial L1 credentials. Use `SetApiCredentials` to set full credentials");

            SetApiCredentials(
                new PolymarketCredentials()
                    .WithL1(existingCreds.L1.SignType, existingCreds.L1.PrivateKey, existingCreds.L1.PolymarketFundingAddress)
                    .WithL2(credentials.ApiKey, credentials.Secret, credentials.Passphrase));
        }
    }
}
