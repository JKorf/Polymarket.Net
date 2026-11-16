---
name: polymarket-net
description: Generate and troubleshoot C#/.NET code consuming Polymarket.Net for prediction market discovery, outcome-token orders, Perps data/trading, wallet and proxy authentication, WebSocket streams, local books and Shared API V2.
---

# Polymarket.Net

Install the consumer package:

```sh
dotnet add package Polymarket.Net
```

`PolymarketRestClient` exposes `PredictionApi` and `PerpsApi`, each with `ExchangeData`, `Account` and `Trading`. `PolymarketSocketClient.PredictionApi` exposes streams directly; `PerpsApi` groups market/account/trading streams and socket transactions. Reuse and dispose clients, await calls, and retain successful subscriptions for unsubscribe cleanup.

Prediction credentials distinguish L1 `EthKey` wallet signing from L2 API key/secret/passphrase. Orders require both; user streams use L2. Use `PolymarketCredentials().WithL1(...).WithL2(...)`, or obtain L2 credentials through `GetOrCreateApiCredentialsAsync` and apply `UpdateL2Credentials`. Select signature type and funding/deposit address for the actual wallet.

Perps has a separate owner/proxy/secret model through `.WithPerps(...)`: owner keys register proxies, registered proxy private keys sign trading, and proxy address plus secret authenticate private reads/streams. Prediction credentials do not authenticate Perps.

Prediction V1 uses `ClobTokenIds`; V2 uses `PositionIds` and `marketVersion: 2`. Token/position IDs, condition IDs and Gamma numeric IDs are distinct. Prediction prices are between 0 and 1. Concrete Perps methods use numeric instrument IDs and base-asset quantities; discover instrument precision and minimum notional first.

Check `Success` before `Data`: REST returns `HttpResult`, subscriptions `WebSocketResult<UpdateSubscription>`, Perps socket transactions `QueryResult`, and Shared helpers may return `ExchangeCallResult`, with generic variants where applicable. Prediction placement also requires `Data.Success`; Perps single-order rejections become outer failures. Check each batch item.

Shared API V2 is Perps-only through `.PerpsApi.SharedApi`; `IPolymarketSharedApiClient` groups `PerpsRest` and `PerpsSocket`. Prediction token trading has no Shared implementation.

## Read when needed

- [API surfaces](references/api-surfaces.md): routing, identifiers, pagination, proxy lifecycle, Shared capabilities, catalogs and quantities.
- [Usage](references/usage.md): authentication, account reads, orders, subscriptions, Shared API V2 and DI.
- [Operational considerations](references/safety.md): signing roles, execution, allowances, proxy changes and uncertain submissions.

For work on the library itself, use [repository instructions](../../../AGENTS.md) (checkout-only).
