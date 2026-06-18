using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
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

        public async Task<HttpResult<PolymarketOrderResult>> PlaceOrderAsync(
            string tokenId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            bool? postOnly = null,
            long? clientOrderId = null,
            DateTime? expiration = null,
            QuantityType? quantityType = null,
            decimal? tickQuantity = null,
            bool? negativeRisk = null,
            CancellationToken ct = default)
        {
            PolymarketOrderBook? tokenInfo = null;
            if (orderType != OrderType.Limit || tickQuantity is null || negativeRisk is null)
            {
                var tokenResult = await PolymarketUtils.GetTokenInfoAsync(tokenId, _baseClient).ConfigureAwait(false);
                if (!tokenResult.Success)
                    return HttpResult.Fail<PolymarketOrderResult>(PolymarketPlatform.Metadata.Id, tokenResult.Error);

                tokenInfo = tokenResult.Data;
                tickQuantity ??= tokenInfo.TickQuantity;
                negativeRisk ??= tokenInfo.NegativeRisk;
            }

            var makerTakerQuantities = GetMakerTakerQuantities(tokenId, side, orderType, quantity, price, timeInForce, tickQuantity.Value, quantityType ?? QuantityType.Shares, tokenInfo);
            if (!makerTakerQuantities.Success)
                return HttpResult.Fail<PolymarketOrderResult>(_baseClient.Exchange, makerTakerQuantities.Error);

            var builderCode = _baseClient.ClientOptions.BuilderCode ?? "0x0000000000000000000000000000000000000000000000000000000000000000";
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var orderParameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var credentials = _baseClient.AuthenticationProvider!.ApiCredentials;

            var maker = credentials.L1.PolymarketFundingAddress ?? credentials.L1.GetPublicAddress();
            var signer = credentials.L1.GetPublicAddress();
            if (credentials.L1.SignType == SignType.Poly1271)
            {
                signer = credentials.L1.PolymarketFundingAddress;
            }

            orderParameters.AddRaw("salt", (ulong)(clientOrderId ?? ExchangeHelpers.RandomLong(1000000000000, 9999999999999)));
            orderParameters.Add("maker", maker);
            orderParameters.Add("signer", signer!);
            orderParameters.Add("tokenId", tokenId);
            orderParameters.Add("makerAmount", makerTakerQuantities.Data.MakerQuantity);
            orderParameters.Add("takerAmount", makerTakerQuantities.Data.TakerQuantity);
            orderParameters.Add("side", side);
            orderParameters.Add("expiration", (expiration == null ? 0 : DateTimeConverter.ConvertToSeconds(expiration.Value)).ToString());
            orderParameters.Add("signatureType", (int)credentials.L1.SignType);
            orderParameters.Add("timestamp", DateTime.UtcNow);
            orderParameters.Add("metadata", "0x0000000000000000000000000000000000000000000000000000000000000000");
            orderParameters.Add("builder", builderCode!);
            orderParameters.Add("signature",
                _baseClient.AuthenticationProvider.GetOrderSignature(
                    orderParameters,
                    _baseClient.ClientOptions.Environment.ChainId,
                    negativeRisk.Value).ToLowerInvariant());

            parameters.Add("order", orderParameters);
            parameters.Add("owner", credentials.L2!.Key!);
            parameters.Add("orderType", timeInForce ?? (orderType == OrderType.Limit ? TimeInForce.GoodTillCanceled : TimeInForce.ImmediateOrCancel));
            parameters.Add("postOnly", postOnly);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/order", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(3500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderResult>(request, parameters, ct).ConfigureAwait(false);

            if (!result.Success)
                return result;

            if (!string.IsNullOrEmpty(result.Data.Error))
                return HttpResult.Fail<PolymarketOrderResult>(result, new ServerError(_baseClient.GetErrorInfo(result.Data.Error!, result.Data.Error)));

            return result;
        }

        public async Task<HttpResult<CallResult<PolymarketOrderResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<PolymarketOrderRequest> requests, CancellationToken ct = default)
        {
            var tokensToRequest = requests
                .Where(x => x.OrderType != OrderType.Limit || x.TickQuantity is null || x.NegativeRisk is null)
                .Select(x => x.TokenId)
                .Distinct().ToList();

            var orderBookInfos = new PolymarketOrderBook[0];
            if (tokensToRequest.Count > 0)
            {
                var tokenResult = await PolymarketUtils.GetTokenInfosAsync(tokensToRequest, _baseClient).ConfigureAwait(false);
                if (!tokenResult.Success)
                    return HttpResult.Fail<CallResult<PolymarketOrderResult>[]>(_baseClient.Exchange, tokenResult.Error);

                orderBookInfos = tokenResult.Data;
            }

            var builderCode = _baseClient.ClientOptions.BuilderCode ?? "0x0000000000000000000000000000000000000000000000000000000000000000";

            var credentials = _baseClient.AuthenticationProvider!.ApiCredentials;

            var maker = credentials.L1.PolymarketFundingAddress ?? credentials.L1.GetPublicAddress();
            var signer = credentials.L1.GetPublicAddress();
            if (credentials.L1.SignType == SignType.Poly1271)
            {
                signer = credentials.L1.PolymarketFundingAddress;
            }
            var parameterList = new List<Parameters>();
            foreach (var request in requests)
            {
                PolymarketOrderBook? tokenInfo = null;
                decimal? tickQuantity = request.TickQuantity;
                bool? negativeRisk = request.NegativeRisk;
                if (request.OrderType != OrderType.Limit || request.TickQuantity is null || request.NegativeRisk is null)
                {
                    tokenInfo = orderBookInfos.SingleOrDefault(x => x.TokenId == request.TokenId);
                    if (tokenInfo == null)
                    {
                        return HttpResult.Fail<CallResult<PolymarketOrderResult>[]>(_baseClient.Exchange,
                            new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, $"Token {request.TokenId} not found")));
                    }

                    tickQuantity ??= tokenInfo.TickQuantity;
                    negativeRisk ??= tokenInfo.NegativeRisk;
                }

                var makerTakerQuantities = GetMakerTakerQuantities(request.TokenId, request.Side, request.OrderType, request.Quantity, request.Price, request.TimeInForce, tickQuantity!.Value, request.QuantityType ?? QuantityType.Shares, tokenInfo);
                if (!makerTakerQuantities.Success)
                    return HttpResult.Fail<CallResult<PolymarketOrderResult>[]>(_baseClient.Exchange, makerTakerQuantities.Error);

                var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
                var orderParameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
                orderParameters.AddRaw("salt", (ulong)(request.ClientOrderId ?? ExchangeHelpers.RandomLong(1000000000000, 9999999999999)));
                orderParameters.Add("maker", maker);
                orderParameters.Add("signer", signer!);
                orderParameters.Add("tokenId", request.TokenId);
                orderParameters.Add("makerAmount", makerTakerQuantities.Data.MakerQuantity);
                orderParameters.Add("takerAmount", makerTakerQuantities.Data.TakerQuantity);
                orderParameters.Add("expiration", (request.Expiration == null ? 0 : DateTimeConverter.ConvertToSeconds(request.Expiration.Value)).ToString());
                orderParameters.Add("timestamp", DateTime.UtcNow);
                orderParameters.Add("metadata", "0x0000000000000000000000000000000000000000000000000000000000000000");
                orderParameters.Add("builder", builderCode!);
                orderParameters.Add("side", request.Side);
                orderParameters.Add("signatureType", (int)credentials.L1.SignType);
                orderParameters.Add("signature",
                    _baseClient.AuthenticationProvider.GetOrderSignature(
                        orderParameters,
                        _baseClient.ClientOptions.Environment.ChainId,
                        negativeRisk!.Value).ToLowerInvariant());

                parameters.Add("order", orderParameters);
                parameters.Add("owner", credentials.L2!.Key!);
                parameters.Add("orderType", request.TimeInForce ?? TimeInForce.GoodTillCanceled);
                parameters.Add("postOnly", request.PostOnly);
                parameterList.Add(parameters);
            }

            var requestParams = new Parameters(parameterList.ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var requestDef = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrderResult[]>(requestDef, requestParams, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CallResult<PolymarketOrderResult>[]>(result);

            var ordersResult = new List<CallResult<PolymarketOrderResult>>();
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Error))
                    ordersResult.Add(CallResult.Fail<PolymarketOrderResult>(new ServerError(_baseClient.GetErrorInfo(item.Error!, item.Error))));
                else
                    ordersResult.Add(CallResult.Ok(item));
            }

            if (ordersResult.All(x => !x.Success))
                return HttpResult.Fail<CallResult<PolymarketOrderResult>[]>(result, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return HttpResult.Ok(result, ordersResult.ToArray());
        }

        private CallResult<(decimal MakerQuantity, decimal TakerQuantity)> GetMakerTakerQuantities(
            string tokenId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price,
            TimeInForce? timeInForce, 
            decimal tickSize,
            QuantityType quantityType,
            PolymarketOrderBook? bookInfo)
        {
            if (quantityType == QuantityType.Value && !(orderType == OrderType.Market && side == OrderSide.Buy))
                throw new ArgumentException("QuantityType.Value can only be set for buy market orders", nameof(quantityType));

            if (bookInfo == null && orderType != OrderType.Limit)
                throw new ArgumentException("Order book info is required for non-limit orders to calculate maker/taker quantities", nameof(bookInfo));

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
                if (side == OrderSide.Buy)
                {
                    decimal? marketPrice = null;
                    var sum = 0m;
                    for (var i = bookInfo!.Asks.Length - 1; i >= 0; i--)
                    {
                        var ask = bookInfo.Asks[i];
                        sum += ask.Quantity;
                        if (sum >= quantity)
                        {
                            marketPrice = ask.Price;
                            break;
                        }
                    }

                    if (timeInForce == TimeInForce.FillOrKill && marketPrice == null)
                        return CallResult.Fail<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "FOK order couldn't fill")));

                    if (marketPrice == null && bookInfo.Asks.Length == 0)
                        return CallResult.Fail<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "Market order couldn't be filled due to empty order book")));

                    price = marketPrice ?? bookInfo.Asks[0].Price;
                }
                else
                {
                    decimal? marketPrice = null;
                    var sum = 0m;
                    for (var i = bookInfo!.Bids.Length - 1; i >= 0; i--)
                    {
                        var bid = bookInfo.Bids[i];
                        sum += bid.Quantity;
                        if (sum >= quantity)
                        {
                            marketPrice = bid.Price;
                            break;
                        }
                    }

                    if (timeInForce == TimeInForce.FillOrKill && marketPrice == null)
                        return CallResult.Fail<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "FOK order couldn't fill")));

                    if (marketPrice == null && bookInfo.Bids.Length == 0)
                        return CallResult.Fail<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "Market order couldn't be filled due to empty order book")));

                    price = marketPrice ?? bookInfo.Bids[0].Price;
                }
            }

            price = Math.Round(price!.Value, rounding.Price).Normalize();
            if (side == OrderSide.Buy)
            {
                if (orderType == OrderType.Market)
                {
                    if (quantityType == QuantityType.Shares)
                    {
                        // Quantity in number of shares
                        takerQuantity = quantity;
                        if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                        {
                            takerQuantity = RoundUp(takerQuantity, rounding.Amount + 4);
                            if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                                takerQuantity = RoundDown(takerQuantity, rounding.Amount);
                        }

                        makerQuantity = takerQuantity * price.Value;
                        makerQuantity = Round(makerQuantity, rounding.Size);
                    }
                    else
                    {
                        // Quantity in USD value
                        makerQuantity = RoundDown(quantity, rounding.Size);

                        takerQuantity = makerQuantity / price.Value;
                        if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                        {
                            takerQuantity = RoundUp(takerQuantity, rounding.Amount + 4);
                            if (GetDecimalPlaces(takerQuantity) > rounding.Amount)
                                takerQuantity = RoundDown(takerQuantity, rounding.Amount);
                        }
                    }
                }
                else
                {
                    takerQuantity = RoundDown(quantity, rounding.Size);
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

            return CallResult.Ok((makerQuantity, takerQuantity));
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

        public async Task<HttpResult<PolymarketOrder>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/data/order/" + orderId, PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(900, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(
            string? orderId = null,
            string? marketId = null,
            string? tokenId = null,
            string? cursor = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("id", orderId);
            parameters.Add("market", marketId);
            parameters.Add("asset_id", tokenId);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/data/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketOrder>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketOrderScoring>> GetOrderRewardScoringAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("order_id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/order-scoring", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderScoring>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }


        public async Task<HttpResult<Dictionary<string, bool>>> GetOrdersRewardScoringAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(orderIds.ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/orders-scoring", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, bool>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("orderID", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/order", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(3000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCancelResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(orderIds.ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? market = null, string? tokenId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("market", market);
            parameters.Add("asset_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/cancel-market-orders", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketCancelResult>> CancelAllOrdersAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/cancel-all", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(250, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult<PolymarketPage<PolymarketTrade>>> GetUserTradesAsync(
            string? tradeId = null,
            string? makerAddress = null,
            string? marketId = null,
            string? tokenId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? cursor = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("id", tradeId);
            parameters.Add("maker_address", makerAddress);
            parameters.Add("market", marketId);
            parameters.Add("asset_id", tokenId);
            parameters.Add("after", startTime);
            parameters.Add("before", endTime);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/trades", PolymarketPlatform.RateLimiter.ClobApi, 1, true,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<HttpResult> PostOrderHeartbeatAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/heartbeats", PolymarketPlatform.RateLimiter.ClobApi, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderHeartbeat>(request, null, ct).ConfigureAwait(false);
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
