using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;

namespace Polymarket.Net.Objects.Sockets
{
    internal class PolymarketInitialQuery<T> : Query<T>
    {
        public PolymarketInitialQuery(string type, bool customFeatureEnabled, string? key = null, string? sec = null, string? pass = null) : base(new PolymarketSocketInitialRequest
        {
            Type = type,
            CustomFeatureEnabled = customFeatureEnabled,
            Auth = key == null ? null : new PolymarketSocketAuth
            {
                ApiKey = key,
                ApiPass = pass!,
                ApiSecret = sec!
            }
        }, false, 1)
        {
            ExpectsResponse = false;

            MessageRouter = MessageRouter.CreateVoid<T>("");
        }
    }
}
