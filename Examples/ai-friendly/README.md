# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable** - drop into a console project with `dotnet add package Polymarket.Net` and it builds.
- **Self-contained** - single file, no external setup, no shared helpers.
- **Heavily commented** - explains why each line is used.
- **Idiomatic** - follows the current `PredictionApi` / `PerpsApi` client structure and uses `EthKey` for wallet credentials.

## Files

| File | What it shows |
|---|---|
| `01-market-data-quickstart.cs` | Public REST setup, Gamma markets/search and CLOB order book and price reads through `PredictionApi.ExchangeData` |
| `02-authentication-and-trading.cs` | L1/L2 credentials, deriving L2 credentials, allowance checks, limit order placement, open orders and cancellation |
| `03-websocket.cs` | Platform, token, user and sports subscriptions with success checks and teardown |
| `04-di-and-orderbook.cs` | Dependency injection, injected clients, local order book factory and per-user client provider |
| `05-error-handling.cs` | `HttpResult` patterns, retry logic, order acceptance checks and common Polymarket mistakes |
| `06-perps-market-data.cs` | Perpetual instrument discovery, tickers, order books, candles and funding history |
| `07-perps-authentication-and-trading.cs` | Perps proxy credentials, balances, portfolio, limit order placement, cancellation and open orders |
| `08-perps-websocket.cs` | Public Perps ticker/trade streams, optional authenticated order updates and subscription cleanup |

## Running

Examples 01–05 cover prediction markets. REST calls are grouped under
`PredictionApi.ExchangeData`, `PredictionApi.Account`, and `PredictionApi.Trading`;
prediction market subscriptions use `socketClient.PredictionApi`. Perpetual futures
use the separate `PerpsApi` branch. Local prediction order books still use
`IPolymarketOrderBookFactory.CreateClob(tokenId)`.

Examples 06–08 cover perpetual futures through `PerpsApi.ExchangeData`,
`PerpsApi.Account`, and `PerpsApi.Trading`. Discover numeric instrument ids with
`GetSymbolsAsync()`. Private Perps calls use `.WithPerps(...)` with a registered
proxy key and its API secret. Example 07 submits and cancels a live limit buy only
when instrument id, quantity and price arguments are supplied. Example 08 accepts
an instrument id and enables private order updates when both
`PERPS_PROXY_PRIVATE_KEY` and `PERPS_API_SECRET` environment variables are set.

The documentation test in `Polymarket.Net.UnitTests/Documentation/AiExampleCompileTests.cs`
compiles every example against the current library without executing API calls.

```bash
dotnet new console -n MyPolymarketApp
cd MyPolymarketApp
dotnet add package Polymarket.Net
# Copy the example .cs file content into Program.cs
# Replace PRIVATE_KEY / L2 placeholders before running authenticated examples
dotnet run
```
