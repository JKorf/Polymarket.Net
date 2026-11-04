using Polymarket.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace Polymarket.Net
{
    /// <inheritdoc/>
    public class PolymarketUserPerpsDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc/>
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => true;

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketUserPerpsDataTracker(
            ILogger<PolymarketUserPerpsDataTracker> logger,
            IPolymarketRestClient restClient,
            IPolymarketSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig? config) : base(
                logger,
                restClient.PerpsApi.SharedApi,

                restClient.PerpsApi.SharedApi,
                socketClient.PerpsApi.SharedApi,

                restClient.PerpsApi.SharedApi,
                restClient.PerpsApi.SharedApi,
                socketClient.PerpsApi.SharedApi,

                restClient.PerpsApi.SharedApi,
                socketClient.PerpsApi.SharedApi,

                restClient.PerpsApi.SharedApi,
                socketClient.PerpsApi.SharedApi,
                userIdentifier,
                config ?? new FuturesUserDataTrackerConfig(),
                SharedAccountType.PerpetualLinearFutures)
        {
        }
    }
}
