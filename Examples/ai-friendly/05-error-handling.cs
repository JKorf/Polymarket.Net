using CryptoExchange.Net.Objects;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;

var client = new PolymarketRestClient();
var tokenId = "TOKEN_ID";

var book = await WithRetry(() => client.ClobApi.ExchangeData.GetOrderBookAsync(tokenId));
if (!book.Success)
{
    Console.WriteLine($"Order book failed: {book.Error}");
    return;
}

Console.WriteLine($"Loaded order book with {book.Data.Bids.Length} bids.");

// Normal API failures are returned in WebCallResult.Error, not thrown.
var price = await client.ClobApi.ExchangeData.GetPriceAsync(tokenId, OrderSide.Buy);
if (!price.Success)
{
    Console.WriteLine($"Price request failed: {price.Error}");
    return;
}

Console.WriteLine("Price request succeeded.");

// Trading responses have two layers:
// 1. WebCallResult.Success: request/transport/API call succeeded.
// 2. PolymarketOrderResult.Success: Polymarket accepted the order.
static bool IsAcceptedOrder(WebCallResult<PolymarketOrderResult> order)
{
    if (!order.Success)
    {
        Console.WriteLine($"Order call failed: {order.Error}");
        return false;
    }

    if (!order.Data.Success)
    {
        Console.WriteLine($"Order rejected: {order.Data.Error}");
        return false;
    }

    return true;
}

static async Task<WebCallResult<T>> WithRetry<T>(Func<Task<WebCallResult<T>>> call, int maxAttempts = 3)
{
    WebCallResult<T> last = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success)
            return last;

        // Only retry transient errors. Authentication, invalid token ids,
        // allowance issues and order validation failures should be fixed by code/user input.
        if (last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt)));
    }

    return last;
}

// Keep this helper reachable for the compiler while avoiding a live order request.
Func<WebCallResult<PolymarketOrderResult>, bool> orderAccepted = IsAcceptedOrder;
Console.WriteLine(orderAccepted.Method.Name);
