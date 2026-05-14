# Copilot Instructions for Polymarket.Net

This repository is **Polymarket.Net**, a strongly typed C#/.NET client library for the Polymarket REST and WebSocket APIs. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes Polymarket.Net, follow these conventions.

## Use Polymarket.Net, not raw HTTP

Never generate raw `HttpClient` calls to Polymarket CLOB, Gamma, or Data API endpoints. Always use `PolymarketRestClient` or `PolymarketSocketClient`.

## Client setup

```csharp
using Polymarket.Net.Clients;

var restClient = new PolymarketRestClient();
```

For authenticated calls:

```csharp
using Polymarket.Net;
using Polymarket.Net.Enums;

var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(SignType.Poly1271, "PRIVATE_KEY", "POLYMARKET_ADDRESS")
        .WithL2("L2_KEY", "L2_SECRET", "L2_PASS");
});
```

## Result handling

Methods return `WebCallResult<T>` for REST and `CallResult<UpdateSubscription>` for WebSocket subscriptions. Always check `.Success` before reading `.Data`.

## API structure

- `restClient.ClobApi.ExchangeData` - CLOB time, markets, prices, order books, spreads, tick size, fee rate
- `restClient.ClobApi.Account` - API keys, allowances, notifications, builder trades
- `restClient.ClobApi.Trading` - orders, cancellations, user trades, order heartbeat
- `restClient.GammaApi` - events, Gamma markets, tags, series, sports, public search
- `restClient.DataApi` - positions
- `socketClient.ClobApi` - platform, token, user, and sports WebSocket subscriptions

## Authentication

Polymarket.Net has unauthenticated public access, L1 wallet/private-key signing, and L2 API key/secret/passphrase authentication. Use `GetOrCreateApiCredentialsAsync` plus `UpdateL2Credentials` when only L1 credentials are available.

## Avoid

- Raw Polymarket URLs and manual signing
- Binance-style `SpotApi`, `FuturesApi`, or symbol-pair assumptions
- `.SharedClient`; it is not exposed by current Polymarket.Net interfaces
- Synchronous `.Result` / `.Wait()`
- Instantiating clients per request
- Skipping `WebCallResult.Success`
- Placing orders with L1-only credentials

## Reference

For detailed patterns and pitfalls see `AGENTS.md`, `llms.txt`, `llms-full.txt`, `docs/ai-api-map.md`, and `Examples/ai-friendly/`.
