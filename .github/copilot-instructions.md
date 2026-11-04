# Copilot Instructions for Polymarket.Net v5.0.0

Use **Polymarket.Net v5.0.0** for Polymarket prediction markets and perpetual futures.
Use `PolymarketRestClient` and `PolymarketSocketClient`; let the library handle HTTP,
signing, rate limits and reconnection. Reuse clients or use dependency injection.

## API structure

|Workflow|REST|WebSocket|
|--|--|--|
|Prediction market data, Gamma events/search/tags, token lookup|`PredictionApi.ExchangeData`|`PredictionApi`|
|Prediction credentials, allowances, notifications|`PredictionApi.Account`|`PredictionApi` user subscriptions|
|Prediction orders, trades, Data API positions|`PredictionApi.Trading`|`PredictionApi` user subscriptions|
|Perps instruments, tickers, books, candles, funding|`PerpsApi.ExchangeData`|`PerpsApi.ExchangeData`|
|Perps proxy credentials, balances, portfolio, leverage|`PerpsApi.Account`|`PerpsApi.Account`|
|Perps order placement/cancellation and fills|`PerpsApi.Trading`|`PerpsApi.Trading`|
|Exchange-independent futures contracts|`PerpsApi.SharedApi`|`PerpsApi.SharedApi`|

The former CLOB client branch is now `PredictionApi`. Gamma and Data methods have
moved as shown above. `PredictionApi` has no `SharedApi`. Do not invent `SpotApi`,
`FuturesApi`, `GeneralApi`, or `SharedClient` members.

## Authentication

Public market data requires no credentials. Prediction authentication uses L1 wallet
signing and L2 API key/secret/passphrase credentials. Keys are `EthKey` objects from
`CryptoExchange.Net.Web3`, not private key strings:

```csharp
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_DEPOSIT_ADDRESS")
        .WithL2("L2_API_KEY", "L2_API_SECRET", "L2_API_PASSPHRASE");
});
```

`PolymarketL1Credential(signType, walletKey, fundingAddress)` also takes `EthKey`.
Credential properties are `PredictionL1`, `PredictionL2` and `Perps`. Supported
prediction sign types are `EOA`, `Email`, `Proxy` and `Poly1271`; Poly1271 requires
its deposit address. Order placement needs both wallet signing and L2 credentials.
With L1 only, call `PredictionApi.Account.GetOrCreateApiCredentialsAsync()`, check
`.Success`, then call `UpdateL2Credentials(result.Data)`. Socket clients also support
`UpdateL2Credentials` when initial prediction credentials have been configured.

Perps uses a registered proxy and the secret returned by
`PerpsApi.Account.CreateProxyAsync`. Owner keys (`signerKey`) register proxies;
proxy private keys sign trading operations; secret plus proxy address authenticates
private reads and user streams. Configure an existing proxy with:

```csharp
using CryptoExchange.Net.Web3;
using Polymarket.Net;

var credentials = new PolymarketCredentials().WithPerps(
    secret: "PERPS_API_SECRET",
    signerKey: null,
    proxyKey: new EthKey(privateKey: "PERPS_PROXY_PRIVATE_KEY"));
```

Pass credentials through client `options.ApiCredentials`. Prediction and Perps can
be combined with `.WithL1(...).WithL2(...).WithPerps(...)`.

## Identifiers and results

- Prediction V1 markets use `ClobTokenIds`; V2 markets use `PositionIds` and `marketVersion: 2` for orders. Prices are decimals between 0 and 1.
- `PredictionApi.ExchangeData.GetMarketsAsync(closed: false, limit: 5)` returns a Gamma market array, not a CLOB cursor page.
- Concrete Perps methods use numeric instrument ids discovered with `PerpsApi.ExchangeData.GetSymbolsAsync()`. Check quantity/price precision and minimum notional. Use `PerpTimeInForce` for Perps orders, `TimeInForce` for prediction orders.
- REST returns `HttpResult<T>` / `HttpResult`; subscriptions return `WebSocketResult<UpdateSubscription>`; Perps socket operations return `QueryResult<T>` / `QueryResult`.
- Always check `.Success` before `.Data`. Prediction orders also require checking `order.Data.Success`. Single Perps order rejections are surfaced in the outer result; payloads contain `Status`, `Error` and numeric `OrderId`.
- Retry only transient failures when appropriate; do not blindly retry order placement.
- Never use `.Result` or `.Wait()`. Store successful subscriptions and unsubscribe on shutdown or partial failure.
- DI: `services.AddPolymarket(options => { ... })`; inject `IPolymarketRestClient`, `IPolymarketSocketClient`, `IPolymarketUserClientProvider` or `IPolymarketOrderBookFactory`.
- Local prediction books use `PolymarketPredictionSymbolOrderBook` / `IPolymarketOrderBookFactory.CreatePrediction(tokenId)`.
- Local Perps books use `PolymarketPerpsSymbolOrderBook(symbolId)` / `IPolymarketOrderBookFactory.CreatePerps(symbolId)`.
- Builder options are scoped under `PredictionOptions` and `PerpsOptions`; set the appropriate `BuilderCode` to null to disable it.

## Reference

Inspect `Polymarket.Net/Interfaces/Clients/PredictionApi/` and
`Polymarket.Net/Interfaces/Clients/PerpsApi/` for exact signatures. Read `AGENTS.md`,
`llms.txt`, `llms-full.txt`, `docs/ai-api-map.md`, and `Examples/ai-friendly/` for
patterns. Examples 01–05 cover Prediction; 06–08 cover Perps.
