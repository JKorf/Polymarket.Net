using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketSocketClientPerpsSharedApi
    {
        public SubscribeUserTradeOptions SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, false)
        {
        };

        #region Subscribe To UserTrade Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.Trading.SubscribeToUserTradeUpdatesAsync(update => handler(update.ToType(
                update.Data.Select(x => new SharedUserTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, x.SymbolId)),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, x.SymbolId) ?? string.Empty,
                    x.OrderId.ToString(),
                    x.TradeId.ToString(),
                    x.Side == Enums.PositionSide.Long ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    new SharedOrderQuantity(x.Quantity),
                    x.Price,
                    x.Timestamp
                )
                {
                    Fee = x.Fee,
                    FeeAsset = x.FeeAsset
                }).ToArray()
                )), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
