using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.Requests;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.ClobApi;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.ClobApi
{
    /// <inheritdoc />
    internal class PolymarketRestClientClobApiTrading : IPolymarketRestClientClobApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PolymarketRestClientClobApi _baseClient;
        private readonly ILogger _logger;

        private record RoundingConfig
        {
            public int Price { get; set; }
            public int Size { get; set; }
            public int Amount { get; set; }
        }

        private static Dictionary<decimal, RoundingConfig> _roundingConfig = new()
        {
            { 0.1m, new RoundingConfig { Price = 1, Size = 2, Amount = 3 } },
            { 0.01m, new RoundingConfig { Price = 2, Size = 2, Amount = 4 } },
            { 0.001m, new RoundingConfig { Price = 3, Size = 2, Amount = 5 } },
            { 0.0001m, new RoundingConfig { Price = 4, Size = 2, Amount = 6 } },
        };

        internal PolymarketRestClientClobApiTrading(ILogger logger, PolymarketRestClientClobApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }





		/// <summary>
        /// Places a new limit order to the Polymarket platform asynchronously using the specified order request parameters.
        /// </summary>
        /// <remarks>The method validates the provided order request and retrieves necessary token and
        /// quantity information before submitting the order. If validation fails or required data cannot be retrieved,
        /// the result will contain error information. The caller should check the returned WebCallResult for success or
        /// failure and handle errors appropriately.</remarks>
        /// <param name="request">An object containing the details of the order to be placed, including token information, side, quantity (number of shares),
        /// price, and time in force. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a WebCallResult with the result
        /// of the order placement, including order details if successful or error information if the operation fails.</returns>
		public async Task<WebCallResult<PolymarketOrderResult>> PlaceOrderAsync(
			PolymarketOrderRequest request,
			CancellationToken ct = default)
		{
			var tokenResult = await PolymarketUtils.GetTokenInfoAsync(request.TokenId, _baseClient).ConfigureAwait(false);
			if (!tokenResult)
				return new WebCallResult<PolymarketOrderResult>(tokenResult.Error);

			var makerTakerQuantities = await GetMakerTakerQuantitiesAsync(request.TokenId, request.Side, OrderType.Limit, request.Quantity, request.Price, request.TimeInForce, tokenResult.Data.TickQuantity).ConfigureAwait(false);
			if (!makerTakerQuantities)
				return new WebCallResult<PolymarketOrderResult>(makerTakerQuantities.Error);

			var parameters = BuildOrderParameters(
		        request.TokenId, request.Side, makerTakerQuantities.Data.MakerQuantity, makerTakerQuantities.Data.TakerQuantity,
		        tokenResult.Data.NegativeRisk, request.TakerAddress, request.ClientOrderId, request.Expiration, request.Nonce, request.FeeRateBps,
		        request.TimeInForce ?? TimeInForce.GoodTillCanceled,
		        null);

			return await SendOrderAsync(parameters, ct).ConfigureAwait(false);
		}


		/// <summary>
		/// Places a new market order to the Polymarket platform asynchronously using the specified order request parameters.
		/// </summary>
		/// <remarks>The method validates the provided token information and calculates maker and taker
		/// quantities before submitting the order. If validation fails, the result will contain error details. The
		/// operation is performed asynchronously and can be cancelled using the provided cancellation token.</remarks>
		/// <param name="request">The market order request containing details such as token ID, side, amount (BUY = USDC spend amount, SELL = asset/shares amount), and other order parameters.
		/// Cannot be null.</param>
		/// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a WebCallResult with the result
		/// of the placed market order, or error information if the operation fails.</returns>
		public async Task<WebCallResult<PolymarketOrderResult>> PlaceMarketOrderAsync(
			PolymarketMarketOrderRequest request,
			CancellationToken ct = default)
		{
			var tokenResult = await PolymarketUtils.GetTokenInfoAsync(request.TokenId, _baseClient).ConfigureAwait(false);
			if (!tokenResult)
				return new WebCallResult<PolymarketOrderResult>(tokenResult.Error);

			var makerTakerQuantities = await GetMakerTakerQuantitiesAsync(request.TokenId, request.Side, OrderType.Market, request.Amount, null, request.TimeInForce, tokenResult.Data.TickQuantity).ConfigureAwait(false);
			if (!makerTakerQuantities)
				return new WebCallResult<PolymarketOrderResult>(makerTakerQuantities.Error);

			var parameters = BuildOrderParameters(
				request.TokenId, request.Side, makerTakerQuantities.Data.MakerQuantity, makerTakerQuantities.Data.TakerQuantity,
				tokenResult.Data.NegativeRisk, request.TakerAddress, request.ClientOrderId, request.Expiration, request.Nonce, request.FeeRateBps,
				request.TimeInForce ?? TimeInForce.ImmediateOrCancel,
				null);

			return await SendOrderAsync(parameters, ct).ConfigureAwait(false);
		}        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<WebCallResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<PolymarketOrderRequest> requests, CancellationToken ct = default)
        {
            var parameterList = new List<ParameterCollection>();
            foreach (var request in requests)
            {
                var tokenResult = await PolymarketUtils.GetTokenInfoAsync(request.TokenId, _baseClient).ConfigureAwait(false);
                if (!tokenResult)
                    return new WebCallResult<CallResult<PolymarketOrderResult>[]>(tokenResult.Error);

                var makerTakerQuantities = await GetMakerTakerQuantitiesAsync(request.TokenId, request.Side, OrderType.Limit, request.Quantity, request.Price, request.TimeInForce, tokenResult.Data.TickQuantity).ConfigureAwait(false);
                if (!makerTakerQuantities)
                    return new WebCallResult<CallResult<PolymarketOrderResult>[]>(makerTakerQuantities.Error);

                var parameters = BuildOrderParameters(
                    request.TokenId, request.Side, makerTakerQuantities.Data.MakerQuantity, makerTakerQuantities.Data.TakerQuantity,
                    tokenResult.Data.NegativeRisk, request.TakerAddress, request.ClientOrderId, request.Expiration, request.Nonce, request.FeeRateBps,
                    request.TimeInForce ?? TimeInForce.GoodTillCanceled,
                    request.PostOnly);

                parameterList.Add(parameters);
            }

            var requestParams = new ParameterCollection();
            requestParams.SetBody(parameterList.ToArray());
            var requestDef = _definitions.GetOrCreate(HttpMethod.Post, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderResult[]>(requestDef, requestParams, ct).ConfigureAwait(false);
            if (!result)
                return result.As<CallResult<PolymarketOrderResult>[]>(default);

            var ordersResult = new List<CallResult<PolymarketOrderResult>>();
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Error))
                    ordersResult.Add(new CallResult<PolymarketOrderResult>(item, null, new ServerError(_baseClient.GetErrorInfo(item.Error!, item.Error))));
                else
                    ordersResult.Add(new CallResult<PolymarketOrderResult>(item));
            }

            if (ordersResult.All(x => !x.Success))
                return result.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return result.As(ordersResult.ToArray());
        }

        public async Task<WebCallResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleMarketOrdersAsync(IEnumerable<PolymarketMarketOrderRequest> requests, CancellationToken ct = default)
        {
            var parameterList = new List<ParameterCollection>();
            foreach (var request in requests)
            {
                var tokenResult = await PolymarketUtils.GetTokenInfoAsync(request.TokenId, _baseClient).ConfigureAwait(false);
                if (!tokenResult)
                    return new WebCallResult<CallResult<PolymarketOrderResult>[]>(tokenResult.Error);

                var makerTakerQuantities = await GetMakerTakerQuantitiesAsync(request.TokenId, request.Side, OrderType.Market, request.Amount, null, request.TimeInForce, tokenResult.Data.TickQuantity).ConfigureAwait(false);
                if (!makerTakerQuantities)
                    return new WebCallResult<CallResult<PolymarketOrderResult>[]>(makerTakerQuantities.Error);

                var parameters = BuildOrderParameters(
                    request.TokenId, request.Side, makerTakerQuantities.Data.MakerQuantity, makerTakerQuantities.Data.TakerQuantity,
                    tokenResult.Data.NegativeRisk, request.TakerAddress, request.ClientOrderId, request.Expiration, request.Nonce, request.FeeRateBps,
                    request.TimeInForce ?? TimeInForce.ImmediateOrCancel,
                    null);

                parameterList.Add(parameters);
            }

            var requestParams = new ParameterCollection();
            requestParams.SetBody(parameterList.ToArray());
            var requestDef = _definitions.GetOrCreate(HttpMethod.Post, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderResult[]>(requestDef, requestParams, ct).ConfigureAwait(false);
            if (!result)
                return result.As<CallResult<PolymarketOrderResult>[]>(default);

            var ordersResult = new List<CallResult<PolymarketOrderResult>>();
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Error))
                    ordersResult.Add(new CallResult<PolymarketOrderResult>(item, null, new ServerError(_baseClient.GetErrorInfo(item.Error!, item.Error))));
                else
                    ordersResult.Add(new CallResult<PolymarketOrderResult>(item));
            }

            if (ordersResult.All(x => !x.Success))
                return result.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return result.As(ordersResult.ToArray());
        }

        private ParameterCollection BuildOrderParameters(
            string tokenId,
            OrderSide side,
            decimal makerQuantity,
            decimal takerQuantity,
            bool negativeRisk,
            string? takerAddress,
            long? clientOrderId,
            DateTime? expiration,
            long? nonce,
            long? feeRateBps,
            TimeInForce timeInForce,
            bool? postOnly)
        {
            var parameters = new ParameterCollection();
            var orderParameters = new ParameterCollection();
            var credentials = _baseClient.AuthenticationProvider!.ApiCredentials;
            orderParameters.Add("salt", (ulong)(clientOrderId ?? ExchangeHelpers.RandomLong(1000000000000, 9999999999999)));
            orderParameters.Add("maker", credentials.L1.PolymarketFundingAddress ?? credentials.L1.GetPublicAddress());
            orderParameters.Add("signer", credentials.L1.GetPublicAddress());
            orderParameters.Add("taker", takerAddress ?? "0x0000000000000000000000000000000000000000");
            orderParameters.Add("tokenId", tokenId);
            orderParameters.AddString("makerAmount", makerQuantity);
            orderParameters.AddString("takerAmount", takerQuantity);
            orderParameters.AddString("expiration", (ulong)(expiration == null ? 0 : DateTimeConverter.ConvertToSeconds(expiration.Value)));
            orderParameters.AddString("nonce", nonce ?? 0);
            orderParameters.AddString("feeRateBps", feeRateBps ?? 0);
            orderParameters.AddEnum("side", side);
            orderParameters.Add("signatureType", (int)credentials.L1.SignType);
            orderParameters.Add("signature",
                _baseClient.AuthenticationProvider.GetOrderSignature(
                    orderParameters,
                    _baseClient.ClientOptions.Environment.ChainId,
                    negativeRisk).ToLowerInvariant());

            parameters.Add("order", orderParameters);
            parameters.Add("owner", credentials.L2!.Key!);
            parameters.AddEnum("orderType", timeInForce);
            parameters.AddOptional("postOnly", postOnly);
            return parameters;
        }

        private async Task<WebCallResult<PolymarketOrderResult>> SendOrderAsync(ParameterCollection parameters, CancellationToken ct)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(3500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderResult>(request, parameters, ct).ConfigureAwait(false);

            if (!result)
                return result;

            if (!string.IsNullOrEmpty(result.Data.Error))
                return result.AsError<PolymarketOrderResult>(new ServerError(_baseClient.GetErrorInfo(result.Data.Error!, result.Data.Error)));

            return result;
        }

        private async Task<CallResult<(decimal MakerQuantity, decimal TakerQuantity)>> GetMakerTakerQuantitiesAsync(string tokenId, OrderSide side, OrderType orderType, decimal quantity, decimal? price, TimeInForce? timeInForce, decimal tickSize)
        {
            var rounding = _roundingConfig.TryGetValue(tickSize, out var config) ? config : throw new ArgumentException($"Tick size {tickSize} not mapped to rounding config");

            decimal takerQuantity;
            decimal makerQuantity;
            if (orderType == OrderType.Limit)
            {
                if (price == null)
                    throw new ArgumentNullException(nameof(price), "Price is required for limit orders");
            }
            else
            {
                var bookInfo = await _baseClient.ExchangeData.GetOrderBookAsync(tokenId).ConfigureAwait(false);
                if (!bookInfo)
                    return bookInfo.As<(decimal, decimal)>(default);

                if (side == OrderSide.Buy)
                {
                    decimal? marketPrice = null;
                    var sum = 0m;
                    for (var i = bookInfo.Data.Asks.Length - 1; i >= 0; i--)
                    {
                        var ask = bookInfo.Data.Asks[i];
                        sum += ask.Quantity;
                        if (sum >= quantity)
                        {
                            marketPrice = ask.Price;
                            break;
                        }
                    }

                    if (timeInForce == TimeInForce.FillOrKill && marketPrice == null)
                        return new WebCallResult<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "FOK order couldn't fill")));

                    if (marketPrice == null && bookInfo.Data.Asks.Length == 0)
                        return new WebCallResult<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "Market order couldn't be filled due to empty order book")));

                    price = marketPrice ?? bookInfo.Data.Asks[0].Price;
                }
                else
                {
                    decimal? marketPrice = null;
                    var sum = 0m;
                    for (var i = bookInfo.Data.Bids.Length - 1; i >= 0; i--)
                    {
                        var bid = bookInfo.Data.Bids[i];
                        sum += bid.Quantity;
                        if (sum >= quantity)
                        {
                            marketPrice = bid.Price;
                            break;
                        }
                    }

                    if (timeInForce == TimeInForce.FillOrKill && marketPrice == null)
                        return new WebCallResult<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "FOK order couldn't fill")));

                    if (marketPrice == null && bookInfo.Data.Bids.Length == 0)
                        return new WebCallResult<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "Market order couldn't be filled due to empty order book")));

                    price = marketPrice ?? bookInfo.Data.Bids[0].Price;
                }
            }

            price = Math.Round(price!.Value, rounding.Price).Normalize();
			if (side == OrderSide.Buy)
			{
				if (orderType == OrderType.Market)
				{
					// For market buy orders, quantity represents the USDC spend amount.
					// makerQuantity = USDC amount (what the buyer gives)
					makerQuantity = RoundDown(quantity, rounding.Size);

					// takerQuantity = asset/token amount (what the buyer receives)
					takerQuantity = makerQuantity / price.Value;
					if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
					{
						takerQuantity = RoundUp(takerQuantity, rounding.Amount + 4);
						if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
							takerQuantity = RoundDown(takerQuantity, rounding.Amount);
					}
				}
				else
				{
					// For limit buy orders, quantity represents the asset/shares amount.
					// takerQuantity = asset amount (what the buyer receives)
					takerQuantity = RoundDown(quantity, rounding.Size);

					// makerQuantity = USDC spend (what the buyer gives)
					makerQuantity = takerQuantity * price.Value;

					if (GetDecimalPlaces(makerQuantity) > rounding.Amount)
					{
						makerQuantity = RoundUp(makerQuantity, rounding.Amount + 4);
						if (GetDecimalPlaces(makerQuantity) > rounding.Amount)
							makerQuantity = RoundDown(makerQuantity, rounding.Amount);
					}
				}
			}
			else
			{
				// For sell orders, quantity is always the asset/shares amount regardless of order type.
				// makerQuantity = asset amount (what the seller gives)
				// takerQuantity = USDC amount (what the seller receives)
				makerQuantity = RoundDown(quantity, rounding.Size);
                takerQuantity = makerQuantity * price.Value;

                if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                {
                    takerQuantity = RoundUp(takerQuantity, rounding.Amount + 4);
                    if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                        takerQuantity = RoundDown(takerQuantity, rounding.Amount);
                }
            }

            takerQuantity *= 1000000;
            makerQuantity *= 1000000;

            takerQuantity = takerQuantity.Normalize();
            makerQuantity = makerQuantity.Normalize();

            return new CallResult<(decimal, decimal)>((makerQuantity, takerQuantity));
        }

        private static decimal RoundUp(decimal value, int digits)
        {
            var factor = (decimal)Math.Pow(10, digits);
            return Math.Ceiling(value * factor) / factor;
        }

        private static decimal Round(decimal value, int digits)
        {
            var factor = (decimal)Math.Pow(10, digits);
            return Math.Round(value * factor) / factor;
        }

        private static decimal RoundDown(decimal value, int digits)
        {
            var factor = (decimal)Math.Pow(10, digits);
            return Math.Floor(value * factor) / factor;
        }

        public async Task<WebCallResult<PolymarketOrder>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/order/" + orderId, PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(900, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(
            string? orderId = null,
            string? conditionId = null,
            string? assetId = null,
            string? cursor = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", orderId);
            parameters.AddOptional("market", conditionId);
            parameters.AddOptional("asset_id", assetId);
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketOrder>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketOrderScoring>> GetOrderRewardScoringAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("order_id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/order-scoring", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderScoring>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }


        public async Task<WebCallResult<Dictionary<string, bool>>> GetOrdersRewardScoringAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/orders-scoring", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, bool>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("orderID", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/order", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(3000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? market = null, string? tokenId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("market", market);
            parameters.AddOptional("asset_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelAllOrdersAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/cancel-all", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(250, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketPage<PolymarketTrade>>> GetUserTradesAsync(
            string? tradeId = null,
            string? takerAddress = null,
            string? makerAddress = null,
            string? conditionId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", tradeId);
            parameters.AddOptional("taker", takerAddress);
            parameters.AddOptional("maker", makerAddress);
            parameters.AddOptional("market", conditionId);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/trades", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketOrderHeartbeat>> PostOrderHeartbeatAsync(
            string? heartbeatId,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("heartbeat_id", heartbeatId ?? "");
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/heartbeats", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderHeartbeat>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        private static int GetDecimalPlaces(decimal value)
        {
            var s = value.ToString("G29", CultureInfo.InvariantCulture);
            var idx = s.IndexOf('.');
            if (idx < 0) 
                return 0;

            return s.Length - idx - 1;
        }
    }
}
