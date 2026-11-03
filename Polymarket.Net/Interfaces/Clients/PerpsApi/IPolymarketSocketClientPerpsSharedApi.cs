using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PerpsApi
{
    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IPolymarketSocketClientPerpsSharedApi :
        ISubscribeTradesSocket,
        ISubscribeBookTickerSocket,
        ISubscribeOrderBookSocket,
        ISubscribeKlinesSocket,
        ISubscribeTickerSocket,
        ISubscribeAllTickersSocket,
        ISubscribeBalancesSocket,
        ISubscribeUserTradesSocket,
        ISubscribeFuturesOrdersSocket,
        ISubscribePositionsSocket,
        IPlaceFuturesOrderSocket,
        ICancelFuturesOrderSocket,
        ICancelFuturesOrderByClientOrderIdSocket,
        ICancelAllFuturesSymbolOrdersSocket,
        IEditFuturesOrderSocket,
        IEditFuturesOrderByClientOrderIdSocket,
        IPlaceMultipleFuturesOrdersSocket
    {
    }
}
