# Polymarket.Net examples

Each file is an independent console program. Install `Polymarket.Net` in a console project and copy one program to `Program.cs`. Replace credential and identifier placeholders for the intended workflow.

| Program | Workflow |
|---|---|
| [01-market-data-quickstart.cs](01-market-data-quickstart.cs) | Prediction discovery and books |
| [02-authentication-and-trading.cs](02-authentication-and-trading.cs) | Prediction wallet/L2 authentication and live orders |
| [03-websocket.cs](03-websocket.cs) | Prediction subscriptions and cleanup |
| [04-di-and-orderbook.cs](04-di-and-orderbook.cs) | DI, books and Perps Shared API V2 |
| [05-error-handling.cs](05-error-handling.cs) | Results and bounded public-read retry |
| [06-perps-market-data.cs](06-perps-market-data.cs) | Perps instrument discovery and history |
| [07-perps-authentication-and-trading.cs](07-perps-authentication-and-trading.cs) | Perps proxy authentication and live trading |
| [08-perps-websocket.cs](08-perps-websocket.cs) | Perps subscriptions and cleanup |

Prediction and Perps use different credentials and identifiers. Review each program before execution: wallet/API-key/proxy changes and trading calls affect account state. Read [operational considerations](../../.claude/skills/polymarket-net/references/safety.md) and the [usage reference](../../.claude/skills/polymarket-net/references/usage.md) first.

`AiExampleCompileTests` compiles all eight programs without executing exchange operations.
