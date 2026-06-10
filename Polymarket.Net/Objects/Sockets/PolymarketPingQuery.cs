using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Interfaces;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using System.Collections.Generic;

namespace Polymarket.Net.Objects.Sockets
{
    internal class PolymarketPingQuery : Query<string>
    {
        public PolymarketPingQuery() : base("ping", false, 0)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageRouter = MessageRouter.CreateVoid<string>("pong");
        }
    }
}
