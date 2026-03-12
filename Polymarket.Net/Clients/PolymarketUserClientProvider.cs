using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Polymarket.Net.Objects;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc />
    public class PolymarketUserClientProvider : IPolymarketUserClientProvider
    {
        private ConcurrentDictionary<string, IPolymarketRestClient> _restClients = new ConcurrentDictionary<string, IPolymarketRestClient>();
        private ConcurrentDictionary<string, IPolymarketSocketClient> _socketClients = new ConcurrentDictionary<string, IPolymarketSocketClient>();
        
        private readonly IOptions<PolymarketRestOptions> _restOptions;
        private readonly IOptions<PolymarketSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

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
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.Timeout = restOptions.Value.RequestTimeout;
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, PolymarketCredentials credentials, PolymarketEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IPolymarketRestClient GetRestClient(string userIdentifier, PolymarketCredentials? credentials = null, PolymarketEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IPolymarketSocketClient GetSocketClient(string userIdentifier, PolymarketCredentials? credentials = null, PolymarketEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IPolymarketRestClient CreateRestClient(string userIdentifier, PolymarketCredentials? credentials, PolymarketEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new PolymarketRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IPolymarketSocketClient CreateSocketClient(string userIdentifier, PolymarketCredentials? credentials, PolymarketEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new PolymarketSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<PolymarketRestOptions> SetRestEnvironment(PolymarketEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new PolymarketRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<PolymarketSocketOptions> SetSocketEnvironment(PolymarketEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new PolymarketSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
