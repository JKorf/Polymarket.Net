---
name: polymarket-net
description: Use Polymarket.Net v5.0.0 when generating C#/.NET code for Polymarket prediction markets or perpetual futures, including market data, orders, account credentials, WebSocket streams, local prediction order books, and authentication.
---

# Polymarket.Net v5.0.0 Skill

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
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS")
        .WithL2("L2_API_KEY", "L2_API_SECRET", "L2_API_PASSPHRASE");
});
```

## Core Pattern: Result Handling

Every REST method returns `HttpResult<T>` or `HttpResult`. WebSocket subscription methods return `WebSocketResult<UpdateSubscription>`. Always check `.Success` before accessing `.Data`.

```csharp
var markets = await restClient.PredictionApi.ExchangeData.GetMarketsAsync();
if (!markets.Success)
{
    Console.WriteLine(markets.Error);
    return;
}

foreach (var market in markets.Data)
    Console.WriteLine(market.MarketId);
```

## Core Pattern: API Surface

```text
restClient.PredictionApi.ExchangeData // CLOB data, Gamma events/markets/tags/search, open interest, token lookup
restClient.PredictionApi.Account      // L2 API keys, allowances, notifications, builder trades
restClient.PredictionApi.Trading      // orders, cancellations, trades, heartbeat, Data API positions
restClient.PerpsApi.ExchangeData      // instruments, tickers, books, candles, funding, fees
restClient.PerpsApi.Account           // proxy credentials, balances, portfolio, leverage, transfers
restClient.PerpsApi.Trading           // place/edit/cancel orders, order history, fills
restClient.PerpsApi.SharedApi         // exchange-independent REST contracts

socketClient.PredictionApi            // platform, token, user, sports, crypto/equity price streams
socketClient.PerpsApi.ExchangeData    // public futures streams
socketClient.PerpsApi.Account         // private account streams and operations
socketClient.PerpsApi.Trading         // private order/fill streams and trading operations
socketClient.PerpsApi.SharedApi       // exchange-independent socket contracts
```

There is no `SpotApi`, `FuturesApi`, or `GeneralApi`. `SharedApi` is exposed only on `PerpsApi`.
In v5.0.0, the former CLOB branch is `PredictionApi`; Gamma methods moved into
`PredictionApi.ExchangeData` and Data positions into `PredictionApi.Trading`.
`GetMarketsAsync` now returns Gamma markets as an array, with no CLOB cursor argument.

## Authentication Levels And Types

Prediction authentication has two credential levels, stored as `PredictionL1` and `PredictionL2`:

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
var l2 = await restClient.PredictionApi.Account.GetOrCreateApiCredentialsAsync();
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

var order = await restClient.PredictionApi.Trading.PlaceOrderAsync(
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

if (!order.Data.Success)
{
    Console.WriteLine(order.Data.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

Use Gamma market `ClobTokenIds` for V1 markets. For `Version == "v2"`, use
`PositionIds` and pass `marketVersion: 2` to order placement. Prediction prices are
between 0 and 1; Perps uses numeric instrument ids and instrument price precision.

## WebSocket Pattern

Use `PolymarketSocketClient`. Store the returned `UpdateSubscription` and unsubscribe on shutdown.

```csharp
using Polymarket.Net.Clients;

var socketClient = new PolymarketSocketClient();

var sub = await socketClient.PredictionApi.SubscribeToTokenUpdatesAsync(
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
        .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASS");
});

var userSub = await socketClient.PredictionApi.SubscribeToUserUpdatesAsync(
    onOrderUpdate: update => Console.WriteLine(update.Data.Id),
    onTradeUpdate: update => Console.WriteLine(update.Data.TradeId));
if (!userSub.Success)
{
    Console.WriteLine(userSub.Error);
    return;
}
await socketClient.UnsubscribeAsync(userSub.Data);
```

## Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Enums;

services.AddPolymarket(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASS");
});

// Inject IPolymarketRestClient, IPolymarketSocketClient (both expose PredictionApi and PerpsApi),
// IPolymarketOrderBookFactory, or IPolymarketUserClientProvider.
```

## Local Order Book

For a maintained local CLOB order book use `PolymarketPredictionSymbolOrderBook` or `IPolymarketOrderBookFactory.CreatePrediction(tokenId)` via DI. Do not hand-roll book synchronization unless the task explicitly requires it.

Perps local books use `PolymarketPerpsSymbolOrderBook(symbolId)` or
`IPolymarketOrderBookFactory.CreatePerps(symbolId)`. Both book classes are in
`Polymarket.Net.SymbolOrderBooks`.

## Common Pitfalls - AVOID

- Do not use raw `HttpClient` calls or manually sign CLOB requests.
- Do not use Binance-style properties such as `SpotApi`, `FuturesApi`, or `GeneralApi`.
- Prediction orders/books use token or position ids. Perps concrete methods use numeric instrument ids discovered with `PerpsApi.ExchangeData.GetSymbolsAsync()`.
- Do not access `.Data` without checking `.Success`.
- Do not call `.Result` or `.Wait()` on async methods.
- Do not instantiate clients per request in production code; reuse clients or use DI.
- For prediction private trading, create/derive L2 credentials first or configure both levels. Perps uses separate proxy credentials.
- Use `PerpsApi.SharedApi` for shared contracts; `PredictionApi` has no `SharedApi`.
- Do not confuse `PolymarketOrderResult.Success` with `HttpResult.Success`; check both if you need to know whether the API call succeeded and whether the order itself was accepted.

## Perps authentication, trading and streams

Public futures data needs no credentials. Discover numeric instrument ids with
`restClient.PerpsApi.ExchangeData.GetSymbolsAsync()`. Use the returned `SymbolId`
for concrete REST/socket calls; check `QuantityDecimals`, `PriceDecimals` and
`MinNotional` before submitting orders. Perps uses `PerpTimeInForce`.

Configure an existing registered Perps proxy:

```csharp
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;

var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials().WithPerps(
        secret: "PERPS_API_SECRET",
        signerKey: null,
        proxyKey: new EthKey(privateKey: "PERPS_PROXY_PRIVATE_KEY"));
});

var balances = await restClient.PerpsApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
Console.WriteLine($"Balances: {balances.Data.Length}");
```

`PolymarketCredentials.Perps` holds a `PolymarketPerpsCredential`. Owner wallet
`signerKey` credentials register proxies with `PerpsApi.Account.CreateProxyAsync`;
retain the returned API secret and proxy key. Proxy private keys sign trading
operations, while secret plus proxy address authenticates private reads and user
streams. Prediction L1/L2 credentials do not authenticate Perps. Both credential
families can be configured together with `.WithL1(...).WithL2(...).WithPerps(...)`.

Perps REST is grouped into `ExchangeData`, `Account` and `Trading`. Public socket
streams use `socketClient.PerpsApi.ExchangeData`; private account streams use
`.Account` and order/fill streams use `.Trading`. REST returns `HttpResult<T>` /
`HttpResult`; subscriptions return `WebSocketResult<UpdateSubscription>`; Perps
socket operations return `QueryResult<T>` / `QueryResult`. Check outer `.Success`
before `.Data`. Single Perps order rejections are surfaced in the outer result;
`PolymarketPerpOrderResult` has `Status`, `Error` and numeric `OrderId`, with no
prediction-style payload `Success` property. Batch REST order placement returns
an outer result with per-order `CallResult<PolymarketPerpOrderResult>` entries;
check every entry before reading its data.

Both `restClient.PerpsApi.SharedApi` and `socketClient.PerpsApi.SharedApi` expose
CryptoExchange.Net shared futures contracts. `PredictionApi` has no `SharedApi`.
After `GetSymbolsAsync()` populates the symbol cache, REST helpers
`GetSymbolIdByName(name)` and `GetSymbolNameById(id)` can resolve ids/names.

Use the compilable examples:

- `Examples/ai-friendly/06-perps-market-data.cs`: instruments, tickers, books, candles, funding.
- `Examples/ai-friendly/07-perps-authentication-and-trading.cs`: credentials, account reads, explicit order inputs, cancellation.
- `Examples/ai-friendly/08-perps-websocket.cs`: public/private subscriptions and cleanup.

Builder settings are API-specific: `PredictionOptions.BuilderCode` and
`PerpsOptions.BuilderCode`; Perps also has `BuilderFeePercentage`. Set the relevant
`BuilderCode` to null to disable it. DI clients expose both API branches. Local
order book factory provides `CreatePrediction(tokenId)` and `CreatePerps(symbolId)`.

## Environments

```csharp
using CryptoExchange.Net.Web3;
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
