using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Polymarket.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Sockets.Subscriptions
{
    internal class PolymarketBoltQuery : Query<PolymarketBoltMessage>
    {
        public PolymarketBoltQuery(PolymarketBoltMessage request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateForQuery<PolymarketBoltMessage>(request.RequestId, HandleResponse);
        }

        private CallResult<PolymarketBoltMessage> HandleResponse(SocketConnection connection, DateTime receiveTime, string? originalData, PolymarketBoltMessage message)
        {
            if (message.Operation == "error")
            {
                return CallResult<PolymarketBoltMessage>.Fail(new ServerError(ErrorType.Unknown, message.Code!));
            }

            return CallResult.Ok(message, originalData);
        }
    }
}
