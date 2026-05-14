using CryptoExchange.Net.Authentication;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

// Polymarket authentication has two levels:
// L1 signs wallet-level authentication requests.
// L2 is the API key/secret/passphrase used for private order and user endpoints.
var credentials = new PolymarketCredentials(
    new PolymarketL1Credential(
        SignType.Poly1271,
        "PRIVATE_KEY",
        "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS"),
    new HMACPassCredential(
        "L2_API_KEY",
        "L2_API_SECRET",
        "L2_API_PASSPHRASE"));

var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = credentials;
});

// If you only have L1 credentials, derive or create L2 credentials before trading:
// var l2 = await client.ClobApi.Account.GetOrCreateApiCredentialsAsync();
// if (!l2.Success) { Console.WriteLine(l2.Error); return; }
// client.UpdateL2Credentials(l2.Data);

var tokenId = "TOKEN_ID";

// Check allowance before trading. For conditional tokens pass AssetType.Conditional and a token id.
var allowance = await client.ClobApi.Account.GetBalanceAllowanceAsync(AssetType.Collateral);
if (!allowance.Success)
{
    Console.WriteLine($"Could not load allowance: {allowance.Error}");
    return;
}

var order = await client.ClobApi.Trading.PlaceOrderAsync(
    tokenId: tokenId,
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 10m,
    price: 0.42m,
    timeInForce: TimeInForce.GoodTillCanceled);

if (!order.Success)
{
    Console.WriteLine($"Order request failed: {order.Error}");
    return;
}

// A successful WebCallResult means the request completed. The order payload also
// has an acceptance flag and error message from Polymarket.
if (!order.Data.Success)
{
    Console.WriteLine($"Order rejected: {order.Data.Error}");
    return;
}

Console.WriteLine($"Placed order: {order.Data.OrderId}");

var openOrders = await client.ClobApi.Trading.GetOpenOrdersAsync(tokenId: tokenId);
if (!openOrders.Success)
{
    Console.WriteLine($"Could not load open orders: {openOrders.Error}");
    return;
}

Console.WriteLine($"Open orders page contains {openOrders.Data.Data.Length} orders.");

var cancel = await client.ClobApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (!cancel.Success)
{
    Console.WriteLine($"Cancel failed: {cancel.Error}");
    return;
}

Console.WriteLine("Cancel request completed.");
