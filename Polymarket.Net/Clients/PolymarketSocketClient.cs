using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Clients.ClobApi;
using Polymarket.Net.Clients.GammaApi;
using Polymarket.Net.Interfaces.Clients.GammaApi;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc cref="IPolymarketSocketClient" />
    public class PolymarketSocketClient : BaseSocketClient, IPolymarketSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        
         /// <inheritdoc />
        public IPolymarketSocketClientClobApi ClobApi { get; }


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
                        
            ClobApi = AddApiClient(new PolymarketSocketClientClobApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            ClobApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<PolymarketSocketOptions> optionsDelegate)
        {
            PolymarketSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {            
            ClobApi.SetApiCredentials(credentials);
        }
    }
}
