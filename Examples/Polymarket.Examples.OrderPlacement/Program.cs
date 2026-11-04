using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

const string tokenId = "4153292802911610701832309484716814274802943278345248636922528170020319407796";

// Replace with valid credentials or order placement will always fail
var privateKey = "KEY";
var polymarketFundingAddress = "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS";
var l2ApiKey = "L2KEY";
var l2ApiSecret = "L2SECRET";
var l2ApiPassphrase = "L2PASSPHRASE";

Console.WriteLine("Polymarket.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.EOA, EthKey.FromPrivateKey(privateKey), polymarketFundingAddress)
        .WithL2(l2ApiKey, l2ApiSecret, l2ApiPassphrase);
});

await PlaceLimitOrderAsync(client);

static async Task PlaceLimitOrderAsync(PolymarketRestClient client)
{
    Console.WriteLine($"Placing CLOB limit buy order for token {tokenId}...");

    var lastTradePrice = await client.PredictionApi.ExchangeData.GetLastTradePriceAsync(tokenId);
    if (!lastTradePrice.Success)
    {
        Console.WriteLine($"Failed to get last trade price: {lastTradePrice.Error}");
        return;
    }

    var safePrice = Math.Max(0.01m, Math.Round(lastTradePrice.Data.LastTradePrice * 0.95m, 2));
    var order = await client.PredictionApi.Trading.PlaceOrderAsync(
        tokenId: tokenId,
        side: OrderSide.Buy,
        orderType: OrderType.Limit,
        quantity: 5m,
        price: safePrice,
        timeInForce: TimeInForce.GoodTillCanceled);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place order: {order.Error}");
        return;
    }

    if (!order.Data.Success)
    {
        Console.WriteLine($"Order rejected: {order.Data.Error}");
        return;
    }

    Console.WriteLine($"Placed order {order.Data.OrderId}, status: {order.Data.Status}");

    var orderStatus = await client.PredictionApi.Trading.GetOrderAsync(order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query order: {orderStatus.Error}");

    var cancel = await client.PredictionApi.Trading.CancelOrderAsync(order.Data.OrderId);
    if (!cancel.Success)
    {
        Console.WriteLine($"Failed to cancel order: {cancel.Error}");
        return;
    }

    Console.WriteLine(cancel.Data.Canceled.Contains(order.Data.OrderId)
        ? $"Cancelled order {order.Data.OrderId}"
        : $"Cancel request completed, but order was not cancelled: {string.Join(", ", cancel.Data.NotCanceled.Select(x => $"{x.Key}: {x.Value}"))}");
}
