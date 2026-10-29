using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PerpsApi;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PerpsApi.Rest
{
    internal class PolymarketRestClientPerpsApiTrading : IPolymarketRestClientPerpsApiTrading
    {

        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPerpsApi _baseClient;

        public PolymarketRestClientPerpsApiTrading(PolymarketRestClientPerpsApi client)
        {
            _baseClient = client;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult>> PlaceOrderAsync(
            long instrumentId,
            OrderSide side,
            decimal quantity,
            decimal? price = null,
            PerpTimeInForce? timeInForce = null,
            bool? postOnly = null,
            bool? reduceOnly = null,
            string? clientOrderId = null,
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
                    ClientOrderId = clientOrderId
                }
            }, ct).ConfigureAwait(false);

            if (!result.Success && result.Error.ErrorType != ErrorType.AllOrdersFailed)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (!orderResult.Success)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, orderResult.Error);

            return HttpResult.Ok(result, orderResult.Data);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<PolymarketPerpOrderResult>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<PolymarketPerpsOrderRequest> orders,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail<CallResult<PolymarketPerpOrderResult>[]>(_baseClient.Exchange, error!);

            await PolymarketUtils.CheckPerpBuilderAsync(_baseClient, ct).ConfigureAwait(false);

            string? builder = null;
            decimal fee = 0;
            if (_baseClient.ClientOptions.PerpsOptions.BuilderFeePercentage > 0
                && _baseClient.ClientOptions.PerpsOptions.BuilderCode != null
                && PolymarketUtils._builderFeeStatus.TryGetValue(_baseClient.ApiCredentials!.Perps!.ProxyKey.PublicAddress, out var status) && status.Success)
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
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult[]>(request, opParameters, ct, skipBuilderCheck: true).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CallResult<PolymarketPerpOrderResult>[]>(result);

            var ordersResult = new List<CallResult<PolymarketPerpOrderResult>>();
            foreach (var item in result.Data)
            {
                if (item.Status == "err")
                    ordersResult.Add(CallResult.Fail<PolymarketPerpOrderResult>(new ServerError(item.Error!, _baseClient.GetErrorInfo(item.Error!, item.Error!))));
                else
                    ordersResult.Add(CallResult.Ok(item));
            }

            if (ordersResult.All(x => !x.Success))
                return HttpResult.Fail(result, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return HttpResult.Ok(result, ordersResult.ToArray());
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpEditOrderResult>> EditOrderAsync(
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
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return HttpResult.Ok(result, orderResult);
        }

        #endregion

        #region Edit Orders

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpEditOrderResult[]>> EditOrdersAsync(
            IEnumerable<PolymarketPerpsEditRequest> orders,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail<PolymarketPerpEditOrderResult[]>(_baseClient.Exchange, error!);

            var allOrders = new List<Parameters>();
            foreach (var order in orders)
            {
                var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                orderParameters.Add("oid", order.OrderId);
                orderParameters.Add("p", order.Price);
                orderParameters.Add("qty", order.Quantity);
                allOrders.Add(orderParameters);
            }

            var opParameters = PolymarketUtils.GetOperationParameters("modifyOrders", allOrders.ToArray());
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(new HttpMethod("PATCH"), _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpEditOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Order By Client Order Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpEditOrderResult>> EditOrderByClientOrderIdAsync(
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
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return HttpResult.Fail<PolymarketPerpEditOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return HttpResult.Ok(result, orderResult);
        }

        #endregion

        #region Edit Orders By Client Order Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpEditOrderResult[]>> EditOrdersByClientOrderIdAsync(
            IEnumerable<PolymarketPerpsEditCidRequest> orders,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail<PolymarketPerpEditOrderResult[]>(_baseClient.Exchange, error!);

            var allOrders = new List<Parameters>();
            foreach (var order in orders)
            {
                var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
                orderParameters.Add("oid", order.ClientOrderId);
                orderParameters.Add("p", order.Price);
                orderParameters.Add("qty", order.Quantity);
                allOrders.Add(orderParameters);
            }

            var opParameters = PolymarketUtils.GetOperationParameters("modifyOrdersCOID", allOrders.ToArray());
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(new HttpMethod("PATCH"), _baseClient.BaseAddress, "/v1/trade/orders-coid", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpEditOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult>> CancelOrderAsync(
            long orderId,
            CancellationToken ct = default)
        {
            var result = await CancelOrdersAsync([orderId], ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return HttpResult.Ok(result, orderResult);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult[]>> CancelOrdersAsync(
            IEnumerable<long> orderIds,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail<PolymarketPerpOrderResult[]>(_baseClient.Exchange, error!);

            var orderParameters = new Parameters(orderIds.ToArray(), PolymarketPlatform._perpsParameterSerializationSettings);
            var opParameters = PolymarketUtils.GetOperationParameters("cancelOrders", orderParameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order By Client Order Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult>> CancelOrderByClientOrderIdAsync(
            string clientOrderId,
            CancellationToken ct = default)
        {
            var result = await CancelOrdersByClientOrderIdAsync([clientOrderId], ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result);

            var orderResult = result.Data?.FirstOrDefault();
            if (orderResult == null)
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(ErrorType.Unknown, "No order result returned"));

            if (orderResult.Status == "err")
                return HttpResult.Fail<PolymarketPerpOrderResult>(result, new ServerError(orderResult.Error!, _baseClient.GetErrorInfo(orderResult.Error!, orderResult.Error)));

            return HttpResult.Ok(result, orderResult);
        }

        #endregion

        #region Cancel Orders By Client Order Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult[]>> CancelOrdersByClientOrderIdAsync(
            IEnumerable<string> clientOrderIds,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail<PolymarketPerpOrderResult[]>(_baseClient.Exchange, error!);

            var orderParameters = new Parameters(clientOrderIds.ToArray(), PolymarketPlatform._perpsParameterSerializationSettings);
            var opParameters = PolymarketUtils.GetOperationParameters("cancelOrdersCOID", orderParameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult> CancelAllOrdersAsync(
            long symbolId,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(false, true, out var error))
                return HttpResult.Fail(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("iid", symbolId);
            var opParameters = PolymarketUtils.GetOperationParameters("cancelAll", parameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials!.Perps!.ProxyKey!);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/trade/orders/all", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult>(request, opParameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.Status == "err")
                return HttpResult.Fail(result, new ServerError(result.Data.Error!, _baseClient.GetErrorInfo(result.Data.Error!, result.Data.Error)));

            return result;
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrder[]>> GetOrdersAsync(
            long? orderId = null,
            string? clientOrderId = null,
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPerpOrder[]>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("order_id", orderId);
            parameters.Add("client_order_id", clientOrderId);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            var weight = orderId == null ? 1 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrder[]>(request, parameters, ct, weight).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrder[]>> GetOpenOrdersAsync(long? symbolId = null, CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPerpOrder[]>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/open-orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPaged<PolymarketPerpUserTrade>>> GetUserTradesAsync(
            long? symbolId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            SortDirection? sort = null,
            string? cursor = null,
            CancellationToken ct = default)
        {
            if (!_baseClient.ValidateProxyCredentialsAvailable(true, false, out var error))
                return HttpResult.Fail<PolymarketPaged<PolymarketPerpUserTrade>>(_baseClient.Exchange, error!);

            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("instrument_id", symbolId);
            parameters.Add("start_timestamp", startTime);
            parameters.Add("end_timestamp", endTime);
            parameters.Add("sort", sort);
            parameters.Add("cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/account/fills", PolymarketPlatform.RateLimiter.PerpsApi, 10, true);
            var result = await _baseClient.SendAsync<PolymarketPaged<PolymarketPerpUserTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
