using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polymarket.Net.Clients.PerpsApi;
using Polymarket.Net.Clients.PredictionApi;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Net.Http;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc cref="IPolymarketRestClient" />
    public class PolymarketRestClient : BaseRestClient<PolymarketEnvironment, PolymarketCredentials>, IPolymarketRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public IPolymarketRestClientPredictionApi PredictionApi { get; }
        /// <inheritdoc />
        public IPolymarketRestClientPerpsApi PerpsApi { get; }

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
            
            PredictionApi = AddApiClient(new PolymarketRestClientPredictionApi(loggerFactory, httpClient, options.Value));
            PerpsApi = AddApiClient(new PolymarketRestClientPerpsApi(loggerFactory, httpClient, options.Value));
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

            var existingCreds = ((PolymarketRestClientPredictionApi)PredictionApi).ApiCredentials
                ?? throw new InvalidOperationException("UpdateL2Credentials can not be called without having initial L1 credentials. Use `SetApiCredentials` to set full credentials");

            SetApiCredentials(
                new PolymarketCredentials()
                    .WithL1(existingCreds.L1.SignType, existingCreds.L1.PrivateKey, existingCreds.L1.PolymarketFundingAddress)
                    .WithL2(credentials.ApiKey, credentials.Secret, credentials.Passphrase));
        }
    }
}
