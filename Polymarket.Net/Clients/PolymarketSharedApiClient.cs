using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Options;

namespace Polymarket.Net.Clients
{
    /// <inheritdoc />
    public class PolymarketSharedApiClient : SharedApiClientBase, IPolymarketSharedApiClient
    {
        /// <inheritdoc />
        public IPolymarketRestClientPerpsSharedApi PerpsRest { get; }
        /// <inheritdoc />
        public IPolymarketSocketClientPerpsSharedApi PerpsSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public PolymarketSharedApiClient(
            IPolymarketRestClient restClient,
            IPolymarketSocketClient socketClient,
            IOptions<PolymarketOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                    restClient.PerpsApi.SharedApi,
                    socketClient.PerpsApi.SharedApi
                  )
        {
            PerpsRest = restClient.PerpsApi.SharedApi;
            PerpsSocket = socketClient.PerpsApi.SharedApi;
        }
    }
}
