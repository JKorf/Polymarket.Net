using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
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
using System.Linq;
using System.Net.Http;
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
            TimeInForce timeInForce,
            decimal quantity,
            decimal price,
            decimal feeRateBps,
            string? makerAddress = null,
            string? signingAddress = null,
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

            var parameters = new ParameterCollection();
            var orderParameters = new ParameterCollection();

            price = price.Normalize();
            decimal takerQuantity;
            decimal makerQuantity;
            if (side == OrderSide.Buy)
            {
                makerQuantity = quantity * price;
                takerQuantity = quantity;
            }
            else
            {
                takerQuantity = quantity * price;
                makerQuantity = quantity;
            }

            takerQuantity *= 1000000;
            makerQuantity *= 1000000;

            takerQuantity = takerQuantity.Normalize();
            makerQuantity = makerQuantity.Normalize();

            // price: 0.1, size: 50
            //takerQuantity = 50000000;
            //makerQuantity = 50000;

            var authProvider = (PolymarketAuthenticationProvider)_baseClient.AuthenticationProvider!;
            orderParameters.Add("salt", (ulong)(clientOrderId ?? ExchangeHelpers.RandomLong(1000000000000, 9999999999999)));
            orderParameters.Add("maker", makerAddress ?? authProvider.PublicAddress);
            orderParameters.Add("signer", signingAddress ?? authProvider.PublicAddress);
            orderParameters.Add("taker", takerAddress ?? "0x0000000000000000000000000000000000000000");
            orderParameters.Add("tokenId", tokenId);
            orderParameters.AddString("makerAmount", makerQuantity);
            orderParameters.AddString("takerAmount", takerQuantity);
            orderParameters.AddString("expiration", (ulong)(expiration == null ? 0 : DateTimeConverter.ConvertToMilliseconds(expiration.Value)));
            orderParameters.Add("nonce", (nonce ?? 0).ToString());
            orderParameters.AddString("feeRateBps", feeRateBps);
            orderParameters.AddEnum("side", side);
            orderParameters.Add("signatureType", 1);
            orderParameters.Add("signature", authProvider.GetOrderSignature(orderParameters, 137, tokenResult.Data.NegativeRisk).ToLowerInvariant());

            parameters.Add("order", orderParameters);
            parameters.Add("owner", authProvider.ApiKey);
            parameters.AddEnum("orderType", timeInForce);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order", PolymarketExchange.RateLimiter.Polymarket, 1, true);
            var result = await _baseClient.SendAsync<PolymarketOrderResult>(request, parameters, ct).ConfigureAwait(false);

            // If errorMsg is set return error
            return result;
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
