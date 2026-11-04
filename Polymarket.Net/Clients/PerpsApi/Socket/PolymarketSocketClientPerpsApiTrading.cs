using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Objects.Sockets.Subscriptions;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi
{
    internal class PolymarketSocketClientPerpsApiTrading : IPolymarketSocketClientPerpsApiTrading
    {
        private readonly ILogger _logger;
        private readonly PolymarketSocketClientPerpsApi _baseClient;

        public PolymarketSocketClientPerpsApiTrading(ILogger logger, PolymarketSocketClientPerpsApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region TpSl

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTpSlUpdatesAsync(
            Action<DataEvent<PolymarketPerpsTpSlUpdate>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpsTpSlUpdate>(
                _logger,
                _baseClient,
                "tpsl",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Orders

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<PolymarketPerpOrder>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpOrder>(
                _logger,
                _baseClient,
                "orders",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region User Trades

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
            Action<DataEvent<PolymarketPerpUserTradeUpdate[]>> onUpdate,
            CancellationToken ct = default)
        {
            var subscription = new PolymarketPerpsSubscription<PolymarketPerpUserTradeUpdate[]>(
                _logger,
                _baseClient,
                "fills",
                null,
                onUpdate,
                authenticated: true);
            return await _baseClient.SubscribeAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpOrderResult>> PlaceOrderAsync(
            long instrumentId,
            OrderSide side,
            decimal quantity,
            decimal? price = null,
            PerpTimeInForce? timeInForce = null,
            bool? postOnly = null,
            bool? reduceOnly = null,
            string? clientOrderId = null,
            OrderType? triggerOrderType = null,
            decimal? triggerPrice = null,
            TpSlType? tpslType = null,
            TpSlGrouping? grouping = null,
            CancellationToken ct = default)
        {
            var result = await PlaceMultipleOrdersAsync(new[]
                {
                    new PolymarketPerpsOrderRequest
                    {
                        SymbolId = instrumentId,
                        Side = side,
                        Price = price,
                        Quantity = quantity,
                        TimeInForce = timeInForce,
                        PostOnly = postOnly,
                        ReduceOnly = reduceOnly,
                        ClientOrderId = clientOrderId,
                        TriggerOrderType = triggerOrderType,
                        TriggerType = tpslType,
                        TriggerPrice = triggerPrice,
                    },
                },
                grouping: grouping,
                ct).ConfigureAwait(false);

            if (!result.Success && result.Error.ErrorType != ErrorType.AllOrdersFailed)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (!orderResult.Success)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, orderResult.Error);

            return QueryResult.Ok(result, orderResult.Data);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<QueryResult<CallResult<PolymarketPerpOrderResult>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<PolymarketPerpsOrderRequest> orders,
            TpSlGrouping? grouping = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail<CallResult<PolymarketPerpOrderResult>[]>(_baseClient.Exchange, error!);

            await PolymarketUtils.CheckPerpBuilderAsync(_baseClient, ct).ConfigureAwait(false);

            string? builder = null;
            decimal fee = 0;
            if (_baseClient.ClientOptions.PerpsOptions.BuilderFeePercentage > 0
                && _baseClient.ClientOptions.PerpsOptions.BuilderCode != null
                && PolymarketUtils._builderFeeStatus.TryGetValue(_baseClient.ApiCredentials!.Perps!.ProxyKey!.PublicAddress, out var status) && status.Success)
            {
                builder = _baseClient.ClientOptions.PerpsOptions.BuilderCode;
                fee = _baseClient.ClientOptions.PerpsOptions.BuilderFeePercentage.Value / 100m;
            }

            var allOrders = new List<Parameters>();
            foreach (var order in orders)
            {
                var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                orderParameters.Add("iid", order.SymbolId);
                orderParameters.Add("buy", order.Side == OrderSide.Buy);
                orderParameters.Add("p", order.Price);
                orderParameters.Add("qty", order.Quantity);
                orderParameters.Add("tif", order.TimeInForce);
                orderParameters.Add("po", order.PostOnly);
                orderParameters.Add("ro", order.ReduceOnly);
                orderParameters.Add("c", order.ClientOrderId);
                if (order.TriggerPrice != null)
                {
                    var tpSlParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                    tpSlParameters.Add("market", order.TriggerOrderType == OrderType.Market);
                    tpSlParameters.Add("trp", order.TriggerPrice);
                    tpSlParameters.Add("tpsl", order.TriggerType);
                    orderParameters.AddRaw("tr", tpSlParameters);
                }
                if (builder != null)
                {
                    var builderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                    builderParameters.Add("address", builder);
                    builderParameters.Add("fee_rate", fee);
                    orderParameters.AddRaw("builder", builderParameters);
                }
                allOrders.Add(orderParameters);
            }

            var opParameters = PolymarketUtils.GetOperationParameters("createOrders", allOrders.ToArray());
            ((Parameters)opParameters["op"]).Add("grp", grouping);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);

            var request = new PolymarketPerpsAuthSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Request = "post",
                Op = (Parameters)opParameters["op"],
                Salt = (long)opParameters["salt"],
                Timestamp = (long)opParameters["ts"],
                Signature = (string)opParameters["sig"]
            };

            var result = await _baseClient.QueryAsync(_baseClient.BaseAddress.AppendPath("/v1/ws"), new PolymarketPerpsQuery<PolymarketPerpOrderResult[]>(_baseClient, request, false)).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<CallResult<PolymarketPerpOrderResult>[]>(result);

            var ordersResult = new List<CallResult<PolymarketPerpOrderResult>>();
            foreach (var item in result.Data)
            {
                if (item.Status == "err")
                    ordersResult.Add(CallResult.Fail<PolymarketPerpOrderResult>(new ServerError(item.Error!, _baseClient.GetErrorInfo(item.Error!, item.Error!))));
                else
                    ordersResult.Add(CallResult.Ok(item));
            }

            if (ordersResult.All(x => !x.Success))
                return QueryResult.Fail(result, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return QueryResult.Ok(result, ordersResult.ToArray());
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpEditOrderResult>> EditOrderAsync(
            long orderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default)
        {
            var result = await EditOrdersAsync(new[]
            {
                new PolymarketPerpsEditRequest
                {
                    OrderId = orderId,
                    Price = newPrice,
                    Quantity = newQuantity,
                }
            }, ct).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return QueryResult.Ok(result, orderResult);
        }

        #endregion

        #region Edit Orders

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpEditOrderResult[]>> EditOrdersAsync(
            IEnumerable<PolymarketPerpsEditRequest> orders,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail<PolymarketPerpEditOrderResult[]>(_baseClient.Exchange, error!);

            var allOrders = new List<Parameters>();
            foreach (var order in orders)
            {
                var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                orderParameters.Add("oid", order.OrderId);
                orderParameters.Add("p", order.Price);
                orderParameters.Add("qty", order.Quantity);
                allOrders.Add(orderParameters);
            }

            var result = await _baseClient.Query<PolymarketPerpEditOrderResult[]>("modifyOrders", allOrders.ToArray()).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Order By Client Order Id

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpEditOrderResult>> EditOrderByClientOrderIdAsync(
            string clientOrderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default)
        {
            var result = await EditOrdersByClientOrderIdAsync(new[]
            {
                new PolymarketPerpsEditCidRequest
                {
                    ClientOrderId = clientOrderId,
                    Price = newPrice,
                    Quantity = newQuantity,
                }
            }, ct).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return QueryResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return QueryResult.Ok(result, orderResult);
        }

        #endregion

        #region Edit Orders By Client Order Id

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpEditOrderResult[]>> EditOrdersByClientOrderIdAsync(
            IEnumerable<PolymarketPerpsEditCidRequest> orders,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail<PolymarketPerpEditOrderResult[]>(_baseClient.Exchange, error!);

            var allOrders = new List<Parameters>();
            foreach (var order in orders)
            {
                var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                orderParameters.Add("oid", order.ClientOrderId);
                orderParameters.Add("p", order.Price);
                orderParameters.Add("qty", order.Quantity);
                allOrders.Add(orderParameters);
            }

            var result = await _baseClient.Query<PolymarketPerpEditOrderResult[]>("modifyOrdersCOID", allOrders.ToArray()).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpOrderResult>> CancelOrderAsync(
            long orderId,
            CancellationToken ct = default)
        {
            var result = await CancelOrdersAsync([orderId], ct).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return QueryResult.Ok(result, orderResult);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpOrderResult[]>> CancelOrdersAsync(
            IEnumerable<long> orderIds,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail<PolymarketPerpOrderResult[]>(_baseClient.Exchange, error!);

            var orderParameters = new Parameters(orderIds.ToArray(), PolymarketPlatform._perpsParameterSerializationSettings);
            var result = await _baseClient.Query<PolymarketPerpOrderResult[]>("cancelOrders", orderParameters).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order By Client Order Id

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpOrderResult>> CancelOrderByClientOrderIdAsync(
            string clientOrderId,
            CancellationToken ct = default)
        {
            var result = await CancelOrdersByClientOrderIdAsync([clientOrderId], ct).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return QueryResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return QueryResult.Ok(result, orderResult);
        }

        #endregion

        #region Cancel Orders By Client Order Id

        /// <inheritdoc />
        public async Task<QueryResult<PolymarketPerpOrderResult[]>> CancelOrdersByClientOrderIdAsync(
            IEnumerable<string> clientOrderIds,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail<PolymarketPerpOrderResult[]>(_baseClient.Exchange, error!);

            var orderParameters = new Parameters(clientOrderIds.ToArray(), PolymarketPlatform._perpsParameterSerializationSettings);
            var result = await _baseClient.Query<PolymarketPerpOrderResult[]>("cancelOrdersCOID", orderParameters).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<QueryResult> CancelAllOrdersAsync(
            long symbolId,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return QueryResult.Fail(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("iid", symbolId);
            var result = await _baseClient.Query<PolymarketPerpOrderResult>("cancelAll", parameters).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.Status == "err")
                return QueryResult.Fail(result, new ServerError(result.Data.Error!, _baseClient.GetErrorInfo(result.Data.Error!, result.Data.Error)));

            return result;
        }

        #endregion
    }
}
