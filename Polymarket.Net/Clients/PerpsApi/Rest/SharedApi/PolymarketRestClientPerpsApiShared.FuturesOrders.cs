using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CryptoExchange.Net.SharedApis.SharedCapabilities;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal partial class PolymarketRestClientPerpsSharedApi
    {
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
        public async Task<HttpResult<SharedId>> PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

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
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
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
        public async Task<HttpResult<CallResult<SharedId>[]>> PlaceMultipleFuturesOrdersAsync(PlaceMultipleFuturesOrdersRequest request, CancellationToken ct)
        {
            var validationError = PlaceMultipleFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<CallResult<SharedId>[]>(Exchange, validationError);

            var orderRequests = new List<PolymarketPerpsOrderRequest>();
            foreach(var order in request.Orders)
            {
                var symbolId = _api.GetSymbolIdByName(order.SymbolName(FormatSymbol));
                if (symbolId == null)
                    return HttpResult.Fail<CallResult<SharedId>[]>(Exchange, ArgumentError.Invalid("Symbol", $"Symbol {order.SymbolName(FormatSymbol)} not found, make sure GetFuturesSymbolsAsync has been called"));

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
                return HttpResult.Fail<CallResult<SharedId>[]>(result, data: resultData);

            return HttpResult.Ok(result, resultData!);
        }

        #endregion

        #region Get Futures Order

        async Task<IExchangeCallResult<SharedFuturesOrder>> IGetFuturesOrder.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderOptions GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, ArgumentError.Invalid("OrderId", "OrderId is not a valid long"));

            var order = await _api.Trading.GetOrdersAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData == null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, ArgumentError.Invalid("OrderId", "Order not found"));

            var symbol = _api.GetSymbolNameById(orderData.SymbolId);
            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                symbol ?? string.Empty,
                orderData.OrderId.ToString(),
                orderData.Price == 0 ? SharedOrderType.Market : SharedOrderType.Limit,
                orderData.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData),
                orderData.CreateTime)
            {
                ClientOrderId = orderData.ClientOrderId,
                OrderPrice = orderData.Price == 0 ? null : orderData.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderData.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: orderData.QuantityFilled),
                TimeInForce = ParseTimeInForce(orderData.TimeInForce),
                UpdateTime = orderData.UpdateTime,
                ReduceOnly = orderData.ReduceOnly
            });
        }

        private SharedTimeInForce? ParseTimeInForce(PerpTimeInForce timeInForce)
            => timeInForce switch
            {
                PerpTimeInForce.GoodTillCanceled => SharedTimeInForce.GoodTillCanceled,
                PerpTimeInForce.ImmediateOrCancel => SharedTimeInForce.ImmediateOrCancel,
                PerpTimeInForce.FillOrKill => SharedTimeInForce.FillOrKill,
                _ => null
            };

        private SharedOrderStatus ParseOrderStatus(PolymarketPerpOrder data)
            => data.Status switch
            {
                PerpOrderStatus.Accepted => SharedOrderStatus.Open,
                PerpOrderStatus.Open => SharedOrderStatus.Open,
                PerpOrderStatus.Partial => SharedOrderStatus.Open,
                PerpOrderStatus.Filled => SharedOrderStatus.Filled,
                PerpOrderStatus.Cancelled => SharedOrderStatus.Canceled,
                _ => SharedOrderStatus.Unknown
            };
        #endregion

        #region Get Futures Order By Client Order Id

        async Task<IExchangeCallResult<SharedFuturesOrder>> IGetFuturesOrderByClientOrderId.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderByClientOrderIdOptions GetFuturesOrderByClientOrderIdOptions { get; } = new GetFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData == null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, ArgumentError.Invalid("OrderId", "Order not found"));

            var symbol = _api.GetSymbolNameById(orderData.SymbolId);
            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                symbol ?? string.Empty,
                orderData.OrderId.ToString(),
                orderData.Price == 0 ? SharedOrderType.Market : SharedOrderType.Limit,
                orderData.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData),
                orderData.CreateTime)
            {
                ClientOrderId = orderData.ClientOrderId,
                OrderPrice = orderData.Price == 0 ? null : orderData.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderData.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: orderData.QuantityFilled),
                TimeInForce = ParseTimeInForce(orderData.TimeInForce),
                UpdateTime = orderData.UpdateTime,
                ReduceOnly = orderData.ReduceOnly
            });
        }

        #endregion

        #region Get Open Futures Orders

        async Task<IExchangeCallResult<SharedFuturesOrder[]>> IGetOpenFuturesOrders.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenFuturesOrdersAsync(request, ct).ConfigureAwait(false);

        public GetOpenFuturesOrdersOptions GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder[]>> GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var id = request.Symbol == null ? null : _api.GetSymbolIdByName(request.Symbol.GetSymbol(FormatSymbol));

            var order = await _api.Trading.GetOpenOrdersAsync(id, ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(order);

            return HttpResult.Ok(order, order.Data.Select(x => {
                var symbol = _api.GetSymbolNameById(x.SymbolId);
                return new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                    symbol ?? string.Empty,
                    x.OrderId.ToString(),
                    x.Price == 0 ? SharedOrderType.Market : SharedOrderType.Limit,
                    x.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(x),
                    x.CreateTime)
                {
                    ClientOrderId = x.ClientOrderId,
                    OrderPrice = x.Price == 0 ? null : x.Price,
                    OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                    QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                    TimeInForce = ParseTimeInForce(x.TimeInForce),
                    UpdateTime = x.UpdateTime,
                    ReduceOnly = x.ReduceOnly
                };
            }).ToArray());
        }

        #endregion

        #region Get Closed Futures Orders

        async Task<IExchangeCallResult<SharedFuturesOrder[]>> IGetClosedFuturesOrders.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetClosedFuturesOrdersAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetFuturesClosedOrdersOptions GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedFuturesOrder[]>> GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            // Get data
            var result = await _api.Trading.GetOrdersAsync(
                symbolId: id,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                        () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.CreateTime)),
                        result.Data.Length,
                        result.Data.Select(x => x.CreateTime),
                        request.StartTime,
                        request.EndTime ?? DateTime.UtcNow,
                        pageParams);

            return HttpResult.Ok(result, result.Data
                .Where(x => x.Status == PerpOrderStatus.Cancelled || x.Status == PerpOrderStatus.Filled)
                .Select(x => {
                var symbol = _api.GetSymbolNameById(x.SymbolId);
                return new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                    symbol ?? string.Empty,
                    x.OrderId.ToString(),
                    x.Price == 0 ? SharedOrderType.Market : SharedOrderType.Limit,
                    x.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(x),
                    x.CreateTime)
                {
                    ClientOrderId = x.ClientOrderId,
                    OrderPrice = x.Price == 0 ? null : x.Price,
                    OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                    QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                    TimeInForce = ParseTimeInForce(x.TimeInForce),
                    UpdateTime = x.UpdateTime,
                    ReduceOnly = x.ReduceOnly
                };
            }).ToArray(), nextPageRequest);
        }

        #endregion

        #region Cancel Futures Order

        async Task<IExchangeCallResult<SharedId>> ICancelFuturesOrder.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderOptions CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("OrderId", "OrderId is not a valid long"));

            var order = await _api.Trading.CancelOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Cancel Futures Order By Client Order Id

        async Task<IExchangeCallResult<SharedId>> ICancelFuturesOrderByClientOrderId.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderByClientOrderIdOptions CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderByClientOrderIdAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Cancel All Futures Symbol Orders

        async Task<IExchangeCallResult> ICancelAllFuturesSymbolOrders.CancelAllFuturesSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
            => await CancelAllFuturesSymbolOrdersAsync(request, ct).ConfigureAwait(false);

        public CancelAllFuturesSymbolOrdersOptions CancelAllFuturesSymbolOrdersOptions { get; } = new CancelAllFuturesSymbolOrdersOptions(_exchangeName, true);
        public async Task<HttpResult> CancelAllFuturesSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
        {
            var validationError = CancelAllFuturesSymbolOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            var order = await _api.Trading.CancelAllOrdersAsync(id.Value, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order);
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
        public async Task<HttpResult<SharedId>> EditFuturesOrderAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("OrderId", "OrderId is not a valid long"));

            var result = await _api.Trading.EditOrderAsync(
                orderId,
                request.Price!.Value,
                request.Quantity!.QuantityInBaseAsset!.Value,
                ct: ct
                ).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.Order.OrderId.ToString()));
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
        public async Task<HttpResult<SharedId>> EditFuturesOrderByClientOrderIdAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.EditOrderByClientOrderIdAsync(
                request.OrderId,
                request.Price!.Value,
                request.Quantity!.QuantityInBaseAsset!.Value,
                ct: ct
                ).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.Order.OrderId.ToString()));
        }

        #endregion

        #region Get Futures User Trade History

        async Task<IExchangeCallResult<SharedUserTrade[]>> IGetFuturesUserTradeHistory.GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFuturesUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetFuturesUserTradeHistoryOptions GetFuturesUserTradeHistoryOptions { get; } = new GetFuturesUserTradeHistoryOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFuturesUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var id = _api.GetSymbolIdByName(request.SymbolName(FormatSymbol));
            if (id == null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, ArgumentError.Invalid("Symbol", "Symbol not found, make sure GetFuturesSymbolsAsync has been called"));

            // Get data
            var result = await _api.Trading.GetUserTradesAsync(
                id,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                        () => !result.Data.More ? null : Pagination.NextPageFromTime(pageParams, result.Data.Data.Min(x => x.Timestamp)),
                        result.Data.Data.Length,
                        result.Data.Data.Select(x => x.Timestamp),
                        request.StartTime,
                        request.EndTime ?? DateTime.UtcNow,
                        pageParams);

            return HttpResult.Ok(result, result.Data.Data.Select(x =>
            {
                var symbol = _api.GetSymbolNameById(x.SymbolId);
                return new SharedUserTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, symbol),
                    symbol ?? string.Empty,
                    x.OrderId.ToString(),
                    x.TradeId.ToString(),
                    x.Side == PositionSide.Long ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    new SharedOrderQuantity(x.Quantity),
                    x.Price,
                    x.Timestamp)
                {
                    Fee = x.Fee,
                    FeeAsset = x.FeeAsset,
                    Role = x.Taker ? SharedRole.Taker : SharedRole.Maker,
                };
            }).ToArray(), nextPageRequest);
        }

        #endregion
    }
}
