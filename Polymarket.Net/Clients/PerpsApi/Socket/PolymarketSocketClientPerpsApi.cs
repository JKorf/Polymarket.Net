using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using CryptoExchange.Net.Web3;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    /// <summary>
    /// Client providing access to the Polymarket Perps websocket Api
    /// </summary>
    internal partial class PolymarketSocketClientPerpsApi : SocketApiClient<PolymarketEnvironment, PolymarketPerpsAuthenticationProvider, PolymarketCredentials>, IPolymarketSocketClientPerpsApi
    {
        #region fields
        public new PolymarketSocketOptions ClientOptions => (PolymarketSocketOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        #endregion

        /// <inheritdoc />
        public IPolymarketSocketClientPerpsSharedApi SharedApi { get; }
        /// <inheritdoc />
        public IPolymarketSocketClientPerpsApiAccount Account { get; }
        /// <inheritdoc />
        public IPolymarketSocketClientPerpsApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IPolymarketSocketClientPerpsApiTrading Trading { get; }

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PolymarketSocketClientPerpsApi(ILoggerFactory? loggerFactory, PolymarketSocketOptions options) :
            base(loggerFactory, PolymarketPlatform.Metadata.Id, options.Environment.PerpsSocketClientAddress!, options, options.PerpsOptions)
        {
            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(10),
                x => 
                    new PolymarketPerpsQuery<PolymarketStatus>(this, new PolymarketPerpsOpSocketRequest
                    {
                        Id = ExchangeHelpers.NextId(),
                        Request = "post",
                        Op = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings)
                        {
                            { "type", "ping" }
                        }
                    }, false),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });

            Account = new PolymarketSocketClientPerpsApiAccount(_logger, this);
            ExchangeData = new PolymarketSocketClientPerpsApiExchangeData(_logger, this);
            Trading = new PolymarketSocketClientPerpsApiTrading(_logger, this);

            SharedApi = new PolymarketSocketClientPerpsSharedApi(this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PolymarketSocketPerpsMessageHandler();

        /// <inheritdoc />
        protected override PolymarketPerpsAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketPerpsAuthenticationProvider(credentials);

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => PolymarketPlatform.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection, IMessageProcessor? processor)
        {
            var parameters = (Parameters)PolymarketUtils.GetOperationParameters("auth", new Parameters(PolymarketPlatform._perpsParameterSerializationSettings)
            {
                { "proxy", _authenticationProvider.ApiCredentials.Perps.ProxyKey.PublicAddress },
                { "secret", _authenticationProvider.ApiCredentials.Perps.Secret }
            })["op"];

            var request = new PolymarketPerpsOpSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "post",
                Op = parameters
            };

            return Task.FromResult<Query?>(new PolymarketPerpsQuery<PolymarketStatus>(this, request, false));
        }

        internal new Task<QueryResult<THandlerResponse>> QueryAsync<THandlerResponse>(string url, Query<THandlerResponse> query, CancellationToken ct = default)
            => base.QueryAsync(url, query, ct);

        internal async Task<QueryResult<T>> Query<T>(string type, Parameters parameters)
        {
            var opParameters = PolymarketUtils.GetOperationParameters(type, parameters);
            AuthenticationProvider?.AuthenticateOpRequest(this, opParameters, ApiCredentials!.Perps!.ProxyKey!);

            var request = new PolymarketPerpsAuthSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "post",
                Op = (Parameters)opParameters["op"],
                Salt = (long)opParameters["salt"],
                Timestamp = (long)opParameters["ts"],
                Signature = (string)opParameters["sig"]
            };

            return await base.QueryAsync<T>(BaseAddress.AppendPath("/v1/ws"), new PolymarketPerpsQuery<T>(this, request, false)).ConfigureAwait(false);
        }

        internal async Task<QueryResult<T>> Query<T>(string type, Parameters[] parameters)
        {
            var opParameters = PolymarketUtils.GetOperationParameters(type, parameters);
            AuthenticationProvider?.AuthenticateOpRequest(this, opParameters, ApiCredentials!.Perps!.ProxyKey!);

            var request = new PolymarketPerpsAuthSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "post",
                Op = (Parameters)opParameters["op"],
                Salt = (long)opParameters["salt"],
                Timestamp = (long)opParameters["ts"],
                Signature = (string)opParameters["sig"]
            };

            return await base.QueryAsync<T>(BaseAddress.AppendPath("/v1/ws"), new PolymarketPerpsQuery<T>(this, request, false)).ConfigureAwait(false);
        }

        internal new Task<WebSocketResult<UpdateSubscription>> SubscribeAsync(string url, Subscription subscription, CancellationToken ct)
            => base.SubscribeAsync(url, subscription, ct);

        internal bool ValidateProxyCredentialsAvailable(bool requiresSecret, bool requiresPrivateKey, out Error? error)
        {
            var result = ValidateCredentialsAvailable(
                ApiCredentials?.Perps?.ProxyKey,
                "ProxyKey",
                requiresPrivateKey: requiresPrivateKey,
                out error);
            if (!result)
                return result;

            if (requiresSecret && string.IsNullOrEmpty(ApiCredentials?.Perps?.Secret))
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
