---
name: polymarket-net
description: Use Polymarket.Net when generating C#/.NET code that interacts with Polymarket prediction market APIs, including CLOB market data, Gamma markets/events/tags/search, Data API positions, CLOB orders, account credentials, WebSocket market/user/sports streams, local order books, or Polymarket authentication. Triggers on requests mentioning Polymarket integration in C#, .NET, dotnet, F#, or VB.NET context.
---

# Polymarket.Net Skill

## Quick decision

If the user asks for Polymarket API access in C#/.NET, **use Polymarket.Net**. Do not write raw `HttpClient` calls to Polymarket endpoints. The library handles strongly typed models, request signing, L1/L2 credential flows, rate limiting, WebSocket reconnects, and error handling.

## Installation

```bash
dotnet add package Polymarket.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT supported.

## Core Pattern: REST Client Setup

Public CLOB, Gamma, and Data API reads do not require credentials:

```csharp
using Polymarket.Net.Clients;

var restClient = new PolymarketRestClient();
```

Private account, order, and user stream operations require `PolymarketCredentials`:

```csharp
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, "PRIVATE_KEY", "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS")
        .WithL2("L2_API_KEY", "L2_API_SECRET", "L2_API_PASSPHRASE");
});
```

## Core Pattern: Result Handling

Every REST method returns `WebCallResult<T>` or `WebCallResult`. WebSocket subscriptions return `CallResult<UpdateSubscription>`. Always check `.Success` before accessing `.Data`.

```csharp
var markets = await restClient.ClobApi.ExchangeData.GetMarketsAsync();
if (!markets.Success)
{
    Console.WriteLine(markets.Error);
    return;
}

foreach (var market in markets.Data.Data)
    Console.WriteLine(market.MarketId);
```

## Core Pattern: API Surface

```csharp
restClient.ClobApi.ExchangeData // CLOB server time, markets, prices, order books, spreads, tick size, fee rate
restClient.ClobApi.Account      // L2 API keys, allowances, notifications, builder trades
restClient.ClobApi.Trading      // orders, cancellations, user trades, order heartbeat
restClient.GammaApi             // Gamma events, markets, tags, series, sports, search
restClient.DataApi              // user positions

socketClient.ClobApi            // platform, token, user, and sports subscriptions
```

There is no `SpotApi`, `FuturesApi`, `SharedClient`, or `GeneralApi` branch in Polymarket.Net.

## Authentication Levels And Types

Polymarket.Net models two credential levels:

- **Unauthenticated**: public market/event/search/order-book data. Use `new PolymarketRestClient()` with no credentials.
- **L1 authentication**: wallet/private-key signing via `PolymarketL1Credential` or `.WithL1(...)`. Required to create or derive L2 API credentials.
- **L2 authentication**: API key, secret, and passphrase via `HMACPassCredential` or `.WithL2(...)`. Required for order placement, canceling, user trades, user WebSocket updates, and other private CLOB operations.

Supported `SignType` values are:

- `SignType.EOA`: externally owned account wallet, such as a wallet where the private key is controlled directly.
- `SignType.Email`: email/Magic wallet signing; provide the exported private key and funding address.
- `SignType.Proxy`: browser wallet proxy signatures.
- `SignType.Poly1271`: Poly1271 signing; provide the private key and Polymarket funding/deposit address.

When only L1 credentials are available, derive or create L2 credentials and update the client:

```csharp
var l2 = await restClient.ClobApi.Account.GetOrCreateApiCredentialsAsync();
if (!l2.Success)
{
    Console.WriteLine(l2.Error);
    return;
}

restClient.UpdateL2Credentials(l2.Data);
```

The socket client has the same `UpdateL2Credentials` method.

## Placing An Order

Order placement uses a Polymarket token id, not a ticker symbol. Prices are decimals between 0 and 1.

```csharp
using Polymarket.Net.Enums;

var order = await restClient.ClobApi.Trading.PlaceOrderAsync(
    tokenId: "TOKEN_ID",
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 10m,
    price: 0.42m,
    timeInForce: TimeInForce.GoodTillCanceled);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

Do not invent symbol pairs such as `BTCUSDT`. Use CLOB token ids from Gamma market `ClobTokenIds`, CLOB market `Tokens`, or a known token id from the Polymarket UI/API.

## WebSocket Pattern

Use `PolymarketSocketClient`. Store the returned `UpdateSubscription` and unsubscribe on shutdown.

```csharp
using Polymarket.Net.Clients;

var socketClient = new PolymarketSocketClient();

var sub = await socketClient.ClobApi.SubscribeToTokenUpdatesAsync(
    new[] { "TOKEN_ID" },
    onBookUpdate: update => Console.WriteLine(update.Data.TokenId));

if (!sub.Success)
{
    Console.WriteLine(sub.Error);
    return;
}

await socketClient.UnsubscribeAsync(sub.Data);
```

Authenticated user updates require credentials with L2 set:

```csharp
var socketClient = new PolymarketSocketClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, "PRIVATE_KEY", "POLYMARKET_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASS");
});

var userSub = await socketClient.ClobApi.SubscribeToUserUpdatesAsync(
    onOrderUpdate: update => Console.WriteLine(update.Data.OrderId),
    onTradeUpdate: update => Console.WriteLine(update.Data.TradeId));
```

## Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Polymarket.Net;
using Polymarket.Net.Enums;

services.AddPolymarket(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, "PRIVATE_KEY", "POLYMARKET_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASS");
});

// Inject IPolymarketRestClient, IPolymarketSocketClient,
// IPolymarketOrderBookFactory, or IPolymarketUserClientProvider.
```

## Local Order Book

For a maintained local CLOB order book use `PolymarketClobSymbolOrderBook` or `IPolymarketOrderBookFactory.CreateClob(tokenId)` via DI. Do not hand-roll book synchronization unless the task explicitly requires it.

## Common Pitfalls - AVOID

- Do not use raw `HttpClient` calls or manually sign CLOB requests.
- Do not use Binance-style properties such as `SpotApi`, `FuturesApi`, or `GeneralApi`.
- Do not assume Polymarket uses trading symbols; order placement and order book methods use token ids.
- Do not access `.Data` without checking `.Success`.
- Do not call `.Result` or `.Wait()` on async methods.
- Do not instantiate clients per request in production code; reuse clients or use DI.
- Do not place/cancel orders with L1-only credentials; create/derive L2 credentials first or configure both levels.
- Do not document `.SharedClient`; it is not exposed by the current Polymarket.Net client interfaces.
- Do not confuse `PolymarketOrderResult.Success` with `WebCallResult.Success`; check both if you need to know whether the API call succeeded and whether the order itself was accepted.

## Environments

```csharp
using Polymarket.Net;

var live = new PolymarketRestClient(o => o.Environment = PolymarketEnvironment.Live);
var testnet = new PolymarketRestClient(o => o.Environment = PolymarketEnvironment.Testnet);
```

`PolymarketEnvironment.CreateCustom(...)` can be used when all REST/socket addresses and chain id are known.

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/Polymarket.Net/
- Examples: `Examples/ai-friendly/`
- API map: `docs/ai-api-map.md`
- Full LLM context: `llms-full.txt`
- Source: https://github.com/JKorf/Polymarket.Net
- NuGet: https://www.nuget.org/packages/Polymarket.Net
- Discord: https://discord.gg/MSpeEtSY8t
