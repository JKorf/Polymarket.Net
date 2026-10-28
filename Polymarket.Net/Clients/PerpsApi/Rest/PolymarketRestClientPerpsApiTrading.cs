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
        public async Task<HttpResult<PolymarketPerpOrderResult[]>> PlaceOrderAsync(
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
            var orderParameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            orderParameters.Add("iid", instrumentId);
            orderParameters.Add("buy", side == OrderSide.Buy);
            orderParameters.Add("p", price);
            orderParameters.Add("qty", quantity);
            orderParameters.Add("tif", timeInForce);
            orderParameters.Add("po", postOnly);
            orderParameters.Add("ro", reduceOnly);
            orderParameters.Add("c", clientOrderId);
            var opParameters = PolymarketUtils.GetOperationParameters("createOrders", new[] { orderParameters });
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.ProxyKey);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpEditOrderResult[]>> EditOrderAsync(
            long orderId,
            decimal newPrice,
            decimal newQuantity,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._perpsParameterSerializationSettings);
            parameters.Add("oid", orderId);
            parameters.Add("p", newPrice);
            parameters.Add("qty", newQuantity);
            var opParameters = PolymarketUtils.GetOperationParameters("modifyOrders", new[] { parameters });
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.ProxyKey);
            var request = _definitions.GetOrCreate(new HttpMethod("PATCH"), _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpEditOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion


        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPerpOrderResult[]>> CancelOrderAsync(
            long orderId,
            CancellationToken ct = default)
        {
            var orderParameters = new Parameters(new[] { orderId }, PolymarketPlatform._perpsParameterSerializationSettings);
            var opParameters = PolymarketUtils.GetOperationParameters("cancelOrders", orderParameters);
            _baseClient.AuthenticationProvider?.AuthenticateOpRequest(_baseClient, opParameters, _baseClient.ApiCredentials?.Perps.ProxyKey);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v1/trade/orders", PolymarketPlatform.RateLimiter.PerpsApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPerpOrderResult[]>(request, opParameters, ct).ConfigureAwait(false);
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
