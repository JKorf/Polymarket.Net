using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

// Use an existing registered Perps proxy and the secret returned by CreateProxyAsync.
// The proxy private key signs trading operations; the secret authenticates private reads.
// Replace these placeholders before running. Keep secrets outside source control.
var credentials = new PolymarketCredentials().WithPerps(
    secret: "PERPS_API_SECRET",
    signerKey: null,
    proxyKey: new EthKey(privateKey: "PERPS_PROXY_PRIVATE_KEY"));

using var client = new PolymarketRestClient(options => options.ApiCredentials = credentials);

var balances = await client.PerpsApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine($"Could not load balances: {balances.Error}");
    return;
}

Console.WriteLine($"Loaded {balances.Data.Length} collateral balances.");

var portfolio = await client.PerpsApi.Account.GetPortfolioAsync();
if (!portfolio.Success)
{
    Console.WriteLine($"Could not load portfolio: {portfolio.Error}");
    return;
}

Console.WriteLine($"Portfolio: {portfolio.Data}");

// Supply a SymbolId from PerpsApi.ExchangeData.GetSymbolsAsync(), a quantity and a price.
// Check the instrument's QuantityDecimals, PriceDecimals and MinNotional first.
// Requiring these inputs avoids submitting a guessed order when copying the example.
if (args.Length != 3
    || !long.TryParse(args[0], out var instrumentId)
    || !decimal.TryParse(args[1], System.Globalization.NumberStyles.Number,
        System.Globalization.CultureInfo.InvariantCulture, out var quantity)
    || !decimal.TryParse(args[2], System.Globalization.NumberStyles.Number,
        System.Globalization.CultureInfo.InvariantCulture, out var price)
    || quantity <= 0 || price <= 0)
{
    Console.WriteLine("To place and cancel a live limit buy: dotnet run -- <instrumentId> <quantity> <price>");
    return;
}

// Perps uses numeric instrument ids and PerpTimeInForce.
// A price specifies a limit order; postOnly prevents immediate taker execution.
var order = await client.PerpsApi.Trading.PlaceOrderAsync(
    instrumentId: instrumentId,
    side: OrderSide.Buy,
    quantity: quantity,
    price: price,
    timeInForce: PerpTimeInForce.GoodTillCanceled,
    postOnly: true);
if (!order.Success)
{
    // Single-order API rejections are surfaced through HttpResult.Error.
    Console.WriteLine($"Order failed: {order.Error}");
    return;
}

Console.WriteLine($"Order {order.Data.OrderId}: {order.Data.Status}");

// Cancel only the order created by this example. It may already have filled.
var cancel = await client.PerpsApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (!cancel.Success)
{
    Console.WriteLine($"Cancel failed: {cancel.Error}");
    return;
}

Console.WriteLine($"Cancellation: {cancel.Data.Status}");

var openOrders = await client.PerpsApi.Trading.GetOpenOrdersAsync(symbolId: instrumentId);
if (!openOrders.Success)
{
    Console.WriteLine($"Could not load open orders: {openOrders.Error}");
    return;
}

Console.WriteLine($"Open orders: {openOrders.Data.Length}");
