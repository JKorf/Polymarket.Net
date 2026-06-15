using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using CryptoExchange.Net.Clients;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc />
    public class PolymarketUserClientProvider : UserClientProvider<
        IPolymarketRestClient,
        IPolymarketSocketClient,
        PolymarketRestOptions,
        PolymarketSocketOptions,
        PolymarketCredentials,
        PolymarketEnvironment
        >, IPolymarketUserClientProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public PolymarketUserClientProvider(Action<PolymarketOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<PolymarketRestOptions> restOptions,
            IOptions<PolymarketSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        public override string ExchangeName => PolymarketPlatform.Metadata.Id;

        /// <inheritdoc />
        protected override IPolymarketRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<PolymarketRestOptions> options)
            => new PolymarketRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IPolymarketSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<PolymarketSocketOptions> options)
            => new PolymarketSocketClient(options, loggerFactory);
    }
}
