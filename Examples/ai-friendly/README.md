# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable** - drop into a console project with `dotnet add package Polymarket.Net` and it builds.
- **Self-contained** - single file, no external setup, no shared helpers.
- **Heavily commented** - explains why each line is used.
- **Idiomatic** - follows current Polymarket.Net 3.x patterns and the actual client surface.

## Files

| File | What it shows |
|---|---|
| `01-market-data-quickstart.cs` | Public REST setup, Gamma markets/search, CLOB markets, order book and price reads |
| `02-authentication-and-trading.cs` | L1/L2 credentials, deriving L2 credentials, allowance checks, limit order placement, open orders and cancellation |
| `03-websocket.cs` | Platform, token, user and sports subscriptions with success checks and teardown |
| `04-di-and-orderbook.cs` | Dependency injection, injected clients, local order book factory and per-user client provider |
| `05-error-handling.cs` | `WebCallResult` patterns, retry logic, order acceptance checks and common Polymarket mistakes |

## Running

```bash
dotnet new console -n MyPolymarketApp
cd MyPolymarketApp
dotnet add package Polymarket.Net
# Copy the example .cs file content into Program.cs
# Replace PRIVATE_KEY / L2 placeholders before running authenticated examples
dotnet run
```
