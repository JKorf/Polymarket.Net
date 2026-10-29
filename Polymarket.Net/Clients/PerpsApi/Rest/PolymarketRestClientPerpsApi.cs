using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Web3;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Clients.PerpsApi.Rest;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    /// <inheritdoc cref="IPolymarketRestClientPerpsApi" />
    internal partial class PolymarketRestClientPerpsApi : RestApiClient<PolymarketEnvironment, PolymarketPerpsAuthenticationProvider, PolymarketCredentials>, IPolymarketRestClientPerpsApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;
        public new PolymarketRestOptions ClientOptions => (PolymarketRestOptions)base.ClientOptions;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PolymarketRestPerpsMessageHandler(PolymarketErrors.Errors);

        public IPolymarketRestClientPerpsApiAccount Account { get; set; }
        public IPolymarketRestClientPerpsApiExchangeData ExchangeData { get; set; }
        public IPolymarketRestClientPerpsApiTrading Trading { get; set; }

        #endregion
        /// <inheritdoc />
        public string ExchangeName => "Polymarket";
        
        #region constructor/destructor
        internal PolymarketRestClientPerpsApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, PolymarketRestOptions options)
            : base(loggerFactory, PolymarketPlatform.Metadata.Id, httpClient, options.Environment.PerpsRestClientAddress, options, options.PerpsOptions)
        {
            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InBody;

            Account = new PolymarketRestClientPerpsApiAccount(this);
            ExchangeData = new PolymarketRestClientPerpsApiExchangeData(this);
            Trading = new PolymarketRestClientPerpsApiTrading(this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);

        /// <inheritdoc />
        protected override PolymarketPerpsAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketPerpsAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null, bool skipBuilderCheck = false)
        {
            if (!skipBuilderCheck)
                await PolymarketUtils.CheckPerpBuilderAsync(this, cancellationToken).ConfigureAwait(false);

            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync() =>
            ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();


        internal bool ValidateProxyCredentialsAvailable(bool requiresSecret, bool requiresPrivateKey, out Error? error)
        {
            var result = ValidateCredentialsAvailable(
                ApiCredentials?.Perps?.ProxyKey,
                "ProxyKey",
                requiresPrivateKey: requiresPrivateKey,
                out error);
            if (!result)
                return result;

            if (string.IsNullOrEmpty(ApiCredentials?.Perps?.Secret))
            {
                error = new NoApiCredentialsError($"No secret provided in the ApiCredentials.");
                return false;
            }

            return true;
        }

        internal bool ValidateSignerCredentialsAvailable(out Error? error)
        {
            var result = ValidateCredentialsAvailable(
                ApiCredentials?.Perps?.SignerKey,
                "SignerKey",
                requiresPrivateKey: true,
                out error);
            return result;
        }

        internal bool ValidateCredentialsAvailable(
            EthKey? key,
            string keyName,
            bool requiresPrivateKey,
            out Error? error)
        {
            error = null;
            if (key == null)
            {
                error = new NoApiCredentialsError($"Missing credentials, {keyName} is not set");
                return false;
            }

            if (requiresPrivateKey && key.PrivateKey == null)
            {
                error = new NoApiCredentialsError($"The {keyName} is not provided. Please provide {keyName} in the ApiCredentials.");
                return false;
            }

            return true;
        }
    }
}
