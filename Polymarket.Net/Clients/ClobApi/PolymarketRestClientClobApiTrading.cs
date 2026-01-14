using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
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

        internal PolymarketRestClientClobApiTrading(ILogger logger, PolymarketRestClientClobApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        public async Task<WebCallResult<PolymarketOrderResult>> PlaceOrderAsync(
            string tokenId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            long? feeRateBps = null,
            string? takerAddress = null,
            long? clientOrderId = null,
            DateTime? expiration = null,
            long? nonce = null,
            CancellationToken ct = default)
        {
            var tokenResult = await PolymarketUtils.GetTokenInfoAsync(tokenId, _baseClient).ConfigureAwait(false);
            if (!tokenResult)
                return new WebCallResult<PolymarketOrderResult>(tokenResult.Error);

            var tickSize = tokenResult.Data.TickQuantity;

#warning todo market order

            var makerTakerQuantities = await GetMakerTakerQuantitiesAsync(tokenId, side, orderType, quantity, price, timeInForce).ConfigureAwait(false);
            if (!makerTakerQuantities)
                return new WebCallResult<PolymarketOrderResult>(makerTakerQuantities.Error);

            var parameters = new ParameterCollection();
            var orderParameters = new ParameterCollection();
            var authProvider = (PolymarketAuthenticationProvider)_baseClient.AuthenticationProvider!;
            orderParameters.Add("salt", (ulong)(clientOrderId ?? ExchangeHelpers.RandomLong(1000000000000, 9999999999999)));
            orderParameters.Add("maker", authProvider.PolymarketAddress);
            orderParameters.Add("signer", authProvider.PublicAddress);
            orderParameters.Add("taker", takerAddress ?? "0x0000000000000000000000000000000000000000");
            orderParameters.Add("tokenId", tokenId);
            orderParameters.AddString("makerAmount", makerTakerQuantities.Data.MakerQuantity);
            orderParameters.AddString("takerAmount", makerTakerQuantities.Data.TakerQuantity);
            orderParameters.AddString("expiration", (ulong)(expiration == null ? 0 : DateTimeConverter.ConvertToMilliseconds(expiration.Value)));
            orderParameters.AddString("nonce", nonce ?? 0);
            orderParameters.AddString("feeRateBps", feeRateBps ?? 0);
            orderParameters.AddEnum("side", side);
            orderParameters.Add("signatureType", 1);
            orderParameters.Add("signature", authProvider.GetOrderSignature(orderParameters, 137, tokenResult.Data.NegativeRisk).ToLowerInvariant());

            parameters.Add("order", orderParameters);
            parameters.Add("owner", authProvider.ApiKey);
            parameters.AddEnum("orderType", timeInForce ?? TimeInForce.GoodTillCanceled);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderResult>(request, parameters, ct).ConfigureAwait(false);

            // If errorMsg is set return error
            return result;
        }

        private async Task<CallResult<(decimal MakerQuantity, decimal TakerQuantity)>> GetMakerTakerQuantitiesAsync(string tokenId, OrderSide side, OrderType orderType, decimal quantity, decimal? price, TimeInForce? timeInForce)
        {
            decimal takerQuantity;
            decimal makerQuantity;
            if (orderType == OrderType.Limit)
            {
                if (price == null)
                    throw new ArgumentNullException(nameof(price), "Price is required for limit orders");

                price = Math.Round(price.Value, 3).Normalize();
                if (side == OrderSide.Buy)
                {
                    takerQuantity = ExchangeHelpers.RoundDown(quantity, 2);
                    makerQuantity = takerQuantity * price.Value;
                }
                else
                {
                    makerQuantity = ExchangeHelpers.RoundDown(quantity, 2);
                    takerQuantity = makerQuantity * price.Value;
                }

                takerQuantity *= 1000000;
                makerQuantity *= 1000000;

                takerQuantity = takerQuantity.Normalize();
                makerQuantity = makerQuantity.Normalize();
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
                        sum += ask.Quantity * ask.Price;
                        if (sum >= quantity)
                        {
                            marketPrice = ask.Price;
                            break;
                        }
                    }

                    if (timeInForce == TimeInForce.FillOrKill && marketPrice == null)
                        return new WebCallResult<(decimal, decimal)>(new ServerError(new ErrorInfo(ErrorType.RejectedOrderConfiguration, "FOK order couldn't fill")));

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

                    price = marketPrice ?? bookInfo.Data.Bids[0].Price;
                }

                price = Math.Round(price!.Value, 3).Normalize();
                if (side == OrderSide.Buy)
                {
                    makerQuantity = ExchangeHelpers.RoundDown(quantity, 2);
                    takerQuantity = makerQuantity / price.Value;
                }
                else
                {
                    makerQuantity = ExchangeHelpers.RoundDown(quantity, 2);
                    takerQuantity = makerQuantity * price.Value;
                }

                takerQuantity *= 1000000;
                makerQuantity *= 1000000;

                takerQuantity = takerQuantity.Normalize();
                makerQuantity = makerQuantity.Normalize();
            }

            return new CallResult<(decimal, decimal)>((makerQuantity, takerQuantity));
        }

        public async Task<WebCallResult<PolymarketOrder>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/order/" + orderId, PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketPage<PolymarketOrder>>> GetOpenOrdersAsync(string? orderId = null, string? conditionId = null, string? assetId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/orders", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketOrder>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketOrderScoring>> GetOrderRewardScoringAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("order_id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/order-scoring", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderScoring>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }


        public async Task<WebCallResult<Dictionary<string, bool>>> GetOrdersRewardScoringAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/orders-scoring", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, bool>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("orderID", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/order", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/orders", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelOrdersOnMarketAsync(string? market = null, string? tokenId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("market", market);
            parameters.AddOptional("asset_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/orders", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        public async Task<WebCallResult<PolymarketCancelResult>> CancelAllOrdersAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/cancel-all", PolymarketExchange.RateLimiter.Polymarket, 1, true);
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
            parameters.AddOptional("id", cursor);
            parameters.AddOptional("taker", cursor);
            parameters.AddOptional("maker", cursor);
            parameters.AddOptional("market", cursor);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/data/trades", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketPage<PolymarketTrade>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

    }
}
