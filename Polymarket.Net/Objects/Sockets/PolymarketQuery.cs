using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;

namespace Polymarket.Net.Objects.Sockets
{
    internal class PolymarketQuery<T> : Query<T>
    {
        public PolymarketQuery(string type, string[] tokenIds) : base(new PolymarketSocketRequest
        {
            Type = type,
            TokenIds = tokenIds,
            CustomFeatureEnabled = true
        }, false, 1)
        {
            ExpectsResponse = false;

            MessageRouter = MessageRouter.CreateWithoutHandler<T>("");
        }
    }
}
