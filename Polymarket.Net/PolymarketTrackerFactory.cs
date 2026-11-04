using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Polymarket.Net.Clients;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Microsoft.Extensions.Logging;

namespace Polymarket.Net
{
    /// <inheritdoc />
    public class PolymarketTrackerFactory : IPolymarketTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public PolymarketTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval) => false;

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => false;


        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserPerpsDataTracker(FuturesUserDataTrackerConfig? config = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IPolymarketRestClient>() ?? new PolymarketRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IPolymarketSocketClient>() ?? new PolymarketSocketClient();
            return new PolymarketUserPerpsDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<PolymarketUserPerpsDataTracker>>() ?? new NullLogger<PolymarketUserPerpsDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserPerpsDataTracker(string userIdentifier, PolymarketCredentials credentials, FuturesUserDataTrackerConfig? config = null, PolymarketEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IPolymarketUserClientProvider>() ?? new PolymarketUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new PolymarketUserPerpsDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<PolymarketUserPerpsDataTracker>>() ?? new NullLogger<PolymarketUserPerpsDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}
