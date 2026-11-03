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

        public SubscribePositionOptions SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, false)
        {
        };

        #region Subscribe To Position Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.Account.SubscribeToPortfolioUpdatesAsync(update => handler(update.ToType(
                update.Data.Positions.Select(x => 
                new SharedPosition(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    new SharedOrderQuantity(Math.Abs(x.Quantity)),
                    null
                    )
                {
                    AverageOpenPrice = x.EntryPrice,
                    Leverage = x.Leverage,
                    LiquidationPrice = x.LiquidationPrice,
                    PositionSide = x.Quantity > 0 ? SharedPositionSide.Long : SharedPositionSide.Short,
                    UnrealizedPnl = x.UnrealizedPnl,                    
                }).ToArray()
                )), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
