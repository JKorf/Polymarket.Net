# Polymarket.Net usage

Use the section relevant to the task. These snippets are independent console-style
examples with placeholders; they are not instructions to execute live trades.

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Web3;
using CryptoExchange.Net.SharedApis;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
```

## Prediction discovery and order books

```csharp

using var client = new PolymarketRestClient();
var markets = await client.PredictionApi.ExchangeData.GetMarketsAsync(closed: false, limit: 5);
if (!markets.Success)
{
    Console.WriteLine(markets.Error);
    return;
}
var market = markets.Data.FirstOrDefault();
var ids = market?.Version == "v2" ? market.PositionIds : market?.ClobTokenIds;
var tokenId = ids?.FirstOrDefault();
if (tokenId == null)
    return;
var book = await client.PredictionApi.ExchangeData.GetOrderBookAsync(tokenId);
if (!book.Success)
{
    Console.WriteLine(book.Error);
    return;
}
Console.WriteLine($"{market!.Question}: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");
```

Use `SearchAsync(query, ...)` on the same `PredictionApi.ExchangeData` group for
human-readable search. Gamma market `Id` is numeric; `MarketId` is a condition id.

## Derive prediction L2 credentials

```csharp

using var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials().WithL1(
        SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_DEPOSIT_ADDRESS");
});
var l2 = await client.PredictionApi.Account.GetOrCreateApiCredentialsAsync();
if (!l2.Success)
{
    Console.WriteLine(l2.Error);
    return;
}
client.UpdateL2Credentials(l2.Data);
```

For existing L2 credentials use `.WithL1(...).WithL2(key, secret, passphrase)`.
Check collateral/conditional allowance with `PredictionApi.Account.GetBalanceAllowanceAsync`;
choose `AssetType.ConditionalV2` for V2 conditional assets.

## Prediction V2 limit order with acceptance and cancellation checks

```csharp

using var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_DEPOSIT_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASSPHRASE");
});
// Replace with a PositionIds entry from a V2 market. This places a live order.
var order = await client.PredictionApi.Trading.PlaceOrderAsync(
    tokenId: "POSITION_ID", side: OrderSide.Buy, orderType: OrderType.Limit,
    quantity: 10m, price: 0.42m, timeInForce: TimeInForce.GoodTillCanceled,
    marketVersion: 2);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
if (!order.Data.Success)
{
    Console.WriteLine(order.Data.Error);
    return;
}
Console.WriteLine($"Order: {order.Data.OrderId}");
var cancel = await client.PredictionApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine($"Cancellation failed: {cancel.Error}");
else
    foreach (var failure in cancel.Data.NotCanceled)
        Console.WriteLine($"Not canceled {failure.Key}: {failure.Value}");
```

For V1 use a `ClobTokenIds` entry and `marketVersion: 1`. Never substitute the
Gamma market id or condition id for the order's token/position id.

## Prediction positions with cursor pagination

```csharp

using var client = new PolymarketRestClient();
string? cursor = null;
do
{
    var page = await client.PredictionApi.Trading.GetPositionsAsync(
        user: "USER_ADDRESS", status: PositionStatus.Closed, cursor: cursor, limit: 100);
    if (!page.Success)
    {
        Console.WriteLine(page.Error);
        return;
    }
    foreach (var position in page.Data.Data)
        Console.WriteLine($"{position.TokenId}: {position.CurrentSize}");
    cursor = page.Data.Pagination.NextCursor;
} while (!string.IsNullOrEmpty(cursor));
```

## Perps instrument discovery

```csharp

using var client = new PolymarketRestClient();
var symbols = await client.PerpsApi.ExchangeData.GetSymbolsAsync();
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
var instrument = symbols.Data.FirstOrDefault();
if (instrument == null)
    return;
Console.WriteLine($"{instrument.Symbol}: {instrument.SymbolId}, min notional {instrument.MinNotional}");
var book = await client.PerpsApi.ExchangeData.GetOrderBookAsync(instrument.SymbolId, depth: 10);
if (!book.Success)
{
    Console.WriteLine(book.Error);
    return;
}
Console.WriteLine($"Book: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");
```

Use `GetKlinesAsync(symbolId, interval, startTime)` and
`GetFundingRateHistoryAsync(symbolId, startTime: startTime)` for history.

## Perps private reads and limit orders

This example uses an existing registered proxy. Arguments are instrument id,
quantity and price, using invariant decimal notation. Verify instrument precision
and minimum notional before running; supplied order arguments submit a live order.

```csharp

using var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials().WithPerps(
        secret: "PERPS_API_SECRET", signerKey: null,
        proxyKey: new EthKey(privateKey: "PERPS_PROXY_PRIVATE_KEY"));
});
var balances = await client.PerpsApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
Console.WriteLine($"Balances: {balances.Data.Length}");
if (args.Length != 3 || !long.TryParse(args[0], out var instrumentId)
    || !decimal.TryParse(args[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var quantity)
    || !decimal.TryParse(args[2], NumberStyles.Number, CultureInfo.InvariantCulture, out var price)
    || quantity <= 0 || price <= 0)
    return;
var order = await client.PerpsApi.Trading.PlaceOrderAsync(
    instrumentId, OrderSide.Buy, quantity, price: price,
    timeInForce: PerpTimeInForce.GoodTillCanceled, postOnly: true);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
Console.WriteLine($"Order {order.Data.OrderId}: {order.Data.Status}");
var cancel = await client.PerpsApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine($"Cancellation failed: {cancel.Error}");
```

To register a proxy, configure owner `signerKey` and proxy `proxyKey`, then call
`PerpsApi.Account.CreateProxyAsync(expireTime)` and check the result. Retain
`result.Data.Secret` and the proxy key for subsequent sessions.

## Prediction public token stream

```csharp

using var socket = new PolymarketSocketClient();
var sub = await socket.PredictionApi.SubscribeToTokenUpdatesAsync(
    new[] { "TOKEN_ID" }, onBookUpdate: update => Console.WriteLine(update.Data));
if (!sub.Success)
{
    Console.WriteLine(sub.Error);
    return;
}
try { await Task.Delay(TimeSpan.FromSeconds(30)); }
finally { await socket.UnsubscribeAsync(sub.Data); }
```

For authenticated prediction updates use `SubscribeToUserUpdatesAsync` on the
same group with L2 credentials. For public Perps data use
`socket.PerpsApi.ExchangeData.SubscribeToTickerUpdatesAsync(symbolId, onUpdate)`.

## Perps private order stream

```csharp

using var socket = new PolymarketSocketClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials().WithPerps(
        "PERPS_API_SECRET", null, new EthKey(privateKey: "PERPS_PROXY_PRIVATE_KEY"));
});
var sub = await socket.PerpsApi.Trading.SubscribeToOrderUpdatesAsync(
    update => Console.WriteLine(update.Data));
if (!sub.Success)
{
    Console.WriteLine(sub.Error);
    return;
}
try { await Task.Delay(TimeSpan.FromSeconds(30)); }
finally { await socket.UnsubscribeAsync(sub.Data); }
```

## DI, local books and trackers

```csharp

var services = new ServiceCollection();
services.AddLogging();
services.AddPolymarket(); // Set options.ApiCredentials for private workflows.
using var provider = services.BuildServiceProvider();
var rest = provider.GetRequiredService<IPolymarketRestClient>();
var socket = provider.GetRequiredService<IPolymarketSocketClient>();
var shared = provider.GetRequiredService<IPolymarketSharedApiClient>();
var books = provider.GetRequiredService<IPolymarketOrderBookFactory>();
var trackers = provider.GetRequiredService<IPolymarketTrackerFactory>();
var book = books.CreatePrediction("TOKEN_ID");
var start = await book.StartAsync();
if (!start.Success)
{
    Console.WriteLine(start.Error);
    return;
}
try { Console.WriteLine(book.Symbol); }
finally { await book.StopAsync(); }
```

For Perps use `books.CreatePerps(symbolId)`. With private Perps credentials
configured, `trackers.CreateUserPerpsDataTracker()` creates the user futures
tracker. For multiple users, inject `IPolymarketUserClientProvider`, call
`InitializeUserClient(userIdentifier, credentials, environment)`, then
`GetRestClient(userIdentifier)` / `GetSocketClient(userIdentifier)`.

Builder settings are under `options.PredictionOptions` / `options.PerpsOptions`
on the relevant transport. Set that API's `BuilderCode` to null to disable it.
Use `options.Rest` / `options.Socket` when configuring transport-specific settings
through `AddPolymarket`.

## Shared API V2 Perps symbols and ticker

Prediction has no Shared implementation. Discover a Perps symbol rather than guessing an identifier:

```csharp
using var client = new PolymarketRestClient();
IGetFuturesSymbolsRest symbolsClient = client.PerpsApi.SharedApi;
var symbols = await symbolsClient.GetFuturesSymbolsAsync(new GetSymbolsRequest());
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
var symbol = symbols.Data.FirstOrDefault();
if (symbol == null)
    return;
var catalog = symbolsClient.FuturesSymbolCatalog;
IGetTickerRest ticker = client.PerpsApi.SharedApi;
var result = await ticker.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.PerpetualLinear, symbol.BaseAsset, symbol.QuoteAsset)));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

## Dynamic lookup

```csharp
var services = new ServiceCollection();
services.AddPolymarket();
using var provider = services.BuildServiceProvider();
var aggregate = provider.GetRequiredService<IPolymarketSharedApiClient>();
var match = aggregate.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.PerpetualLinear);
if (match is null)
    return;
Console.WriteLine($"{match.Exchange}: {match.Transport}");
```

Use the aggregate's `PerpsRest`/`PerpsSocket` for known workflows. [API routing](api-surfaces.md#shared-api-v2) describes capability options and selection when multiple exchanges are registered. [Operational considerations](safety.md) covers account changes and uncertain submissions.

## Complete programs (checkout-only)

[Examples](../../../../Examples/ai-friendly/README.md) includes Prediction discovery/authentication/orders, Perps discovery/proxy authentication/orders, both socket APIs, DI and Shared API V2. These links require a checkout; the snippets above work with the installed package.
