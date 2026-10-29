using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Polymarket.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Sockets
{
    internal class PolymarketPerpsQuery<T> : Query<T>
        where T: PolymarketPerpsSocketResponse
    {
        public PolymarketPerpsQuery(PolymarketPerpsSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateForQuery<T>(request.Id.ToString(), HandleMessage);
        }

        private CallResult<T>? HandleMessage(SocketConnection connection, DateTime time, string? originalData, T message)
        {
            return CallResult.Ok(message, originalData);
        }
    }
}
