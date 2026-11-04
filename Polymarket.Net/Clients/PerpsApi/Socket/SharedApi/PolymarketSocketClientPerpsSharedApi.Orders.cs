using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
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
        #region Subscribe To FuturesOrder Updates

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, false)
        {
        };

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.Trading.SubscribeToOrderUpdatesAsync(update => handler(update.ToType(
                new[] {
                 new SharedFuturesOrderUpdate(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, update.Data.SymbolId)),
                    PolymarketRestClientPerpsApi.GetSymbolNameById(_api.EnvironmentName, update.Data.SymbolId) ?? string.Empty,
                    update.Data.OrderId.ToString(),
                    update.Data.Price == decimal.MaxValue ? SharedOrderType.Market : SharedOrderType.Limit,
                    update.Data.Buy ? SharedOrderSide.Buy: SharedOrderSide.Sell,
                    ParseOrderStatus(update.Data.Status),
                    update.Data.CreateTime
                )
                {
                     ClientOrderId = update.Data.ClientOrderId,
                     OrderQuantity = new SharedOrderQuantity(update.Data.Quantity),
                     QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled),
                     ReduceOnly = update.Data.ReduceOnly,
                     UpdateTime = update.Data.UpdateTime,
                     TimeInForce = ParseTimeInForce(update.Data.TimeInForce)
                 }
                 }
                )), ct).ConfigureAwait(false);

            return result;
        }

        private SharedTimeInForce? ParseTimeInForce(PerpTimeInForce timeInForce)
            => timeInForce switch
            {
                PerpTimeInForce.GoodTillCanceled => SharedTimeInForce.GoodTillCanceled,
                PerpTimeInForce.ImmediateOrCancel => SharedTimeInForce.ImmediateOrCancel,
                PerpTimeInForce.FillOrKill => SharedTimeInForce.FillOrKill,
                _ => null
            };

        private SharedOrderStatus ParseOrderStatus(PerpOrderStatus status)
            => status switch
            {
                PerpOrderStatus.Accepted => SharedOrderStatus.Open,
                PerpOrderStatus.Open => SharedOrderStatus.Open,
                PerpOrderStatus.Partial => SharedOrderStatus.Open,
                PerpOrderStatus.Filled => SharedOrderStatus.Filled,
                PerpOrderStatus.Cancelled => SharedOrderStatus.Canceled,
                _ => SharedOrderStatus.Unknown
            };
        #endregion

        #region Place Futures Order

        async Task<IExchangeCallResult<SharedId>> IPlaceFuturesOrder.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
            => await PlaceFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public SharedFeeDeductionType FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType FuturesFeeAssetType => SharedFeeAssetType.InputAsset;
        public SharedOrderType[] FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        public SharedTimeInForce[] FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        public SharedQuantitySupport FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomHexString(32);

        public PlaceFuturesOrderOptions PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, true);
        public async Task<QueryResult<SharedId>> PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var id = PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, request.SymbolName(FormatSymbol));
            if (id == null)
                return QueryResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var result = await _api.Trading.PlaceOrderAsync(
                id.Value,
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.Quantity?.QuantityInBaseAsset ?? 0,
                request.Price,
                timeInForce: GetTimeInForce(request),
                postOnly: request.OrderType == SharedOrderType.LimitMaker ? true : null,
                reduceOnly: request.ReduceOnly,
                clientOrderId: request.ClientOrderId,
                ct: ct
                ).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        private PerpTimeInForce? GetTimeInForce(PlaceFuturesOrderRequest request)
        {
            if (request.TimeInForce == null && request.OrderType == SharedOrderType.Market)
                // Market orders require immediate or cancel
                return PerpTimeInForce.ImmediateOrCancel;

            return request.TimeInForce switch
            {
                SharedTimeInForce.GoodTillCanceled => PerpTimeInForce.GoodTillCanceled,
                SharedTimeInForce.ImmediateOrCancel => PerpTimeInForce.ImmediateOrCancel,
                SharedTimeInForce.FillOrKill => PerpTimeInForce.FillOrKill,
                _ => null
            };
        }

        #endregion

        #region Place Multiple Futures Orders

        /// <inheritdoc />
        public bool PlaceMultipleFuturesOrdersAllowsMultipleSymbols => true;
        public int? MaxFuturesOrdersPerRequest => null;

        async Task<IExchangeCallResult<CallResult<SharedId>[]>> IPlaceMultipleFuturesOrders.PlaceMultipleFuturesOrdersAsync(PlaceMultipleFuturesOrdersRequest request, CancellationToken ct)
            => await PlaceMultipleFuturesOrdersAsync(request, ct).ConfigureAwait(false);

        public PlaceMultipleFuturesOrdersOptions PlaceMultipleFuturesOrdersOptions { get; } = new PlaceMultipleFuturesOrdersOptions(_exchangeName);
        public async Task<QueryResult<CallResult<SharedId>[]>> PlaceMultipleFuturesOrdersAsync(PlaceMultipleFuturesOrdersRequest request, CancellationToken ct)
        {
            var validationError = PlaceMultipleFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<CallResult<SharedId>[]>(Exchange, validationError);

            var orderRequests = new List<PolymarketPerpsOrderRequest>();
            foreach (var order in request.Orders)
            {
                var symbolId = PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, order.SymbolName(FormatSymbol));
                if (symbolId == null)
                    return QueryResult.Fail<CallResult<SharedId>[]>(Exchange, ArgumentError.Invalid("Symbol", $"Symbol {order.SymbolName(FormatSymbol)} not found, make sure GetFuturesSymbolsAsync has been called"));

                var req = new PolymarketPerpsOrderRequest
                {
                    SymbolId = symbolId.Value,
                    Side = order.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                    Quantity = order.Quantity?.QuantityInBaseAsset ?? 0,
                    Price = order.Price,
                    PostOnly = order.OrderType == SharedOrderType.LimitMaker ? true : null,
                    TimeInForce = GetTimeInForce(order),
                    ClientOrderId = order.ClientOrderId,
                    ReduceOnly = order.ReduceOnly
                };
                orderRequests.Add(req);
            }

            var result = await _api.Trading.PlaceMultipleOrdersAsync(orderRequests, ct: ct).ConfigureAwait(false);
            var resultData = result.Data?.Select(x => new CallResult<SharedId>()
            {
                Data = x.Success ? new SharedId(x.Data!.OrderId.ToString()) : null,
                Error = x.Error,
                OriginalData = x.OriginalData
            }).ToArray();

            if (!result.Success)
                return QueryResult.Fail<CallResult<SharedId>[]>(result, data: resultData);

            return QueryResult.Ok(result, resultData!);
        }

        #endregion

        #region Cancel Futures Order

        async Task<IExchangeCallResult<SharedId>> ICancelFuturesOrder.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderOptions CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        public async Task<QueryResult<SharedId>> CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return QueryResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("OrderId", "OrderId is not a valid long"));

            var order = await _api.Trading.CancelOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return QueryResult.Fail<SharedId>(order);

            return QueryResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Cancel Futures Order By Client Order Id

        async Task<IExchangeCallResult<SharedId>> ICancelFuturesOrderByClientOrderId.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderByClientOrderIdOptions CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<QueryResult<SharedId>> CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderByClientOrderIdAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return QueryResult.Fail<SharedId>(order);

            return QueryResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Cancel All Futures Symbol Orders

        async Task<IExchangeCallResult> ICancelAllFuturesSymbolOrders.CancelAllFuturesSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
            => await CancelAllFuturesSymbolOrdersAsync(request, ct).ConfigureAwait(false);

        public CancelAllFuturesSymbolOrdersOptions CancelAllFuturesSymbolOrdersOptions { get; } = new CancelAllFuturesSymbolOrdersOptions(_exchangeName, true);
        public async Task<QueryResult<SharedId>> CancelAllFuturesSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
        {
            var validationError = CancelAllFuturesSymbolOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var id = PolymarketRestClientPerpsApi.GetSymbolIdByName(_api.EnvironmentName, request.SymbolName(FormatSymbol));
            if (id == null)
                return QueryResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var order = await _api.Trading.CancelAllOrdersAsync(id.Value, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return QueryResult.Fail<SharedId>(order);

            return QueryResult.Ok(order, new SharedId(""));
        }

        #endregion

        #region Edit Futures Order

        async Task<IExchangeCallResult<SharedId>> IEditFuturesOrder.EditFuturesOrderAsync(EditOrderRequest request, CancellationToken ct)
            => await EditFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public EditFuturesOrderOptions EditFuturesOrderOptions { get; } = new EditFuturesOrderOptions(_exchangeName)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<EditOrderRequest>.Required(x => x.Price),
                RequestParameterRuleOverride<EditOrderRequest>.Required(x => x.Quantity)
                ]
        };
        public async Task<QueryResult<SharedId>> EditFuturesOrderAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return QueryResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("OrderId", "OrderId is not a valid long"));

            var result = await _api.Trading.EditOrderAsync(
                orderId,
                request.Price!.Value,
                request.Quantity!.QuantityInBaseAsset!.Value,
                ct: ct
                ).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Order.OrderId.ToString()));
        }

        #endregion

        #region Edit Futures Order By Client Order Id

        async Task<IExchangeCallResult<SharedId>> IEditFuturesOrderByClientOrderId.EditFuturesOrderByClientOrderIdAsync(EditOrderRequest request, CancellationToken ct)
            => await EditFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public EditFuturesOrderByClientOrderIdOptions EditFuturesOrderByClientOrderIdOptions { get; } = new EditFuturesOrderByClientOrderIdOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<EditOrderRequest>.Required(x => x.Price),
                RequestParameterRuleOverride<EditOrderRequest>.Required(x => x.Quantity)
                ]
        };
        public async Task<QueryResult<SharedId>> EditFuturesOrderByClientOrderIdAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.EditOrderByClientOrderIdAsync(
                request.OrderId,
                request.Price!.Value,
                request.Quantity!.QuantityInBaseAsset!.Value,
                ct: ct
                ).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Order.OrderId.ToString()));
        }

        #endregion
    }
}
