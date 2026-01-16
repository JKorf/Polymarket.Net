using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Objects;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <inheritdoc cref="IPolymarketRestClientClobApi" />
    internal partial class PolymarketRestClientClobApi : RestApiClient, IPolymarketRestClientClobApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        public new PolymarketRestOptions ClientOptions => (PolymarketRestOptions)base.ClientOptions;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PolymarketRestMessageHandler(PolymarketErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IPolymarketRestClientClobApiAccount Account { get; }
        /// <inheritdoc />
        public IPolymarketRestClientClobApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IPolymarketRestClientClobApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "Polymarket";
        #endregion

        #region constructor/destructor
        internal PolymarketRestClientClobApi(ILogger logger, HttpClient? httpClient, PolymarketRestOptions options)
            : base(logger, httpClient, options.Environment.ClobRestClientAddress, options, options.ClobOptions)
        {
            Account = new PolymarketRestClientClobApiAccount(this);
            ExchangeData = new PolymarketRestClientClobApiExchangeData(logger, this);
            Trading = new PolymarketRestClientClobApiTrading(logger, this);

            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InBody;

            OrderParameters = false;
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(PolymarketExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new PolymarketAuthenticationProvider((PolymarketCredentials)credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

    }
}
