using CryptoExchange.Net.Clients;
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
    {
        private readonly SocketApiClient _client;

        public PolymarketPerpsQuery(SocketApiClient client, PolymarketPerpsSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            _client = client;

            MessageRouter = MessageRouter.CreateForQuery<PolymarketPerpsSocketResponse<T>, T>(request.Id.ToString(), HandleMessage);
        }

        private CallResult<T>? HandleMessage(SocketConnection connection, DateTime time, string? originalData, PolymarketPerpsSocketResponse<T> message)
        {
            if (message.Data is PolymarketStatus status && status.Status == "err")
                return CallResult.Fail<T>(new ServerError(status.Error!, _client.GetErrorInfo(status.Error!, status.Error)), originalData);
            if (message.Data is PolymarketStatus[] statusArr && statusArr[0].Status == "err")
                return CallResult.Fail<T>(new ServerError(statusArr[0].Error!, _client.GetErrorInfo(statusArr[0].Error!, statusArr[0].Error)), originalData);

            return CallResult.Ok(message.Data, originalData);
        }
    }
}
