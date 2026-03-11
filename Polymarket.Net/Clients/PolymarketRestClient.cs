using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Objects;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Clients.GammaApi;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc cref="IPolymarketRestClient" />
    public class PolymarketRestClient : BaseRestClient, IPolymarketRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public IPolymarketRestClientClobApi ClobApi { get; }

        /// <inheritdoc />
        public IPolymarketRestClientGammaApi GammaApi { get; }

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
            var existingCredential = existingCreds.GetCredential<PolymarketCredential>()
                ?? throw new InvalidOperationException("UpdateL2Credentials can not be called without having initial L1 credentials. Use `SetApiCredentials` to set full credentials");
            var newCredentials = new PolymarketCredential(
                existingCredential.SignatureType,
                existingCredential.L1PrivateKey,
                credentials.ApiKey,
                credentials.Secret,
                credentials.Passphrase,
                existingCredential.PolymarketFundingAddress
                );

            SetApiCredentials(new ApiCredentials(newCredentials));
        }
    }
}
