using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polymarket.Net.Clients.PredictionApi;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc cref="IPolymarketSocketClient" />
    public class PolymarketSocketClient : BaseSocketClient<PolymarketEnvironment, PolymarketCredentials>, IPolymarketSocketClient
    {
        #region fields
        #endregion

        #region Api clients
        
         /// <inheritdoc />
        public IPolymarketSocketClientPredictionApi PredictionApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of PolymarketSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public PolymarketSocketClient(Action<PolymarketSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of PolymarketSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public PolymarketSocketClient(IOptions<PolymarketSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Polymarket")
        {
            Initialize(options.Value);

            PredictionApi = AddApiClient(new PolymarketSocketClientPredictionApi(loggerFactory, options.Value));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<PolymarketSocketOptions> optionsDelegate)
        {
            PolymarketSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void UpdateL2Credentials(PolymarketCreds credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            var existingCreds = ((PolymarketSocketClientPredictionApi)PredictionApi).ApiCredentials
                ?? throw new InvalidOperationException("UpdateL2Credentials can not be called without having initial L1 credentials. Use `SetApiCredentials` to set full credentials");

            SetApiCredentials(
                new PolymarketCredentials()
                    .WithL1(existingCreds.L1.SignType, existingCreds.L1.PrivateKey, existingCreds.L1.PolymarketFundingAddress)
                    .WithL2(credentials.ApiKey, credentials.Secret, credentials.Passphrase));
        }

    }
}
