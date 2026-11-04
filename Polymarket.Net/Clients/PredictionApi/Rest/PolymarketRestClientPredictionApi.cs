using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Clients.PredictionApi.Rest;
using Polymarket.Net.Interfaces.Clients.PredictionApi;

namespace Polymarket.Net.Clients.PredictionApi
{
    /// <inheritdoc cref="IPolymarketRestClientPredictionApi" />
    internal partial class PolymarketRestClientPredictionApi : RestApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketRestClientPredictionApi
    {
        #region fields 
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        public new PolymarketRestOptions ClientOptions => (PolymarketRestOptions)base.ClientOptions;

        internal string DataApiAddress => ClientOptions.Environment.DataRestClientAddress;
        internal string GammaApiAddress => ClientOptions.Environment.GammaRestClientAddress;
        internal string ClobApiAddress => ClientOptions.Environment.ClobRestClientAddress;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PolymarketRestMessageHandler(PolymarketErrors.Errors);

        public IPolymarketRestClientPredictionApiAccount Account { get; set; }
        public IPolymarketRestClientPredictionApiExchangeData ExchangeData { get; set; }
        public IPolymarketRestClientPredictionApiTrading Trading { get; set; }

        #endregion
        /// <inheritdoc />
        public string ExchangeName => "Polymarket";
        
        #region constructor/destructor
        internal PolymarketRestClientPredictionApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, PolymarketRestOptions options)
            : base(loggerFactory, PolymarketPlatform.Metadata.Id, httpClient, options.Environment.GammaRestClientAddress, options, options.PredictionOptions)
        {
            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InBody;

            Account = new PolymarketRestClientPredictionApiAccount(this);
            ExchangeData = new PolymarketRestClientPredictionApiExchangeData(this);
            Trading = new PolymarketRestClientPredictionApiTrading(this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);


        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendV2Async<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<PolymarketResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            return HttpResult.Ok(result, result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();


    }
}
