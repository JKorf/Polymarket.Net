# ![Polymarket.Net](https://raw.githubusercontent.com/JKorf/Polymarket.Net/main/Polymarket.Net/Icon/icon.png) Polymarket.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Polymarket.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Polymarket.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Polymarket.Net?style=for-the-badge)
![Since](https://img.shields.io/badge/since-2025-brightgreen?style=for-the-badge)

Polymarket.Net is a client library for accessing the [Polymarket REST and WebSocket APIs](https://docs.polymarket.com/developers/CLOB/introduction), covering prediction markets through `PredictionApi` and perpetual futures through `PerpsApi`.

## Features
* Prediction market and perpetual futures REST and WebSocket APIs
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* High performance
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange clients based on the CryptoExchange.Net base library
* Native AOT support

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as the latest dotnet versions to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/Polymarket.net.svg?style=for-the-badge)](https://www.nuget.org/packages/Polymarket.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/Polymarket.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/Polymarket.Net)

	dotnet add package Polymarket.Net
	
### GitHub packages
Polymarket.Net is available on [GitHub packages](https://github.com/JKorf/Polymarket.Net/pkgs/nuget/Polymarket.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/Polymarket.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/Polymarket.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/Polymarket.Net/releases).

## How to use

The REST and WebSocket clients expose two API branches:

|API|REST members|WebSocket members|
|--|--|--|
|Prediction markets|`PredictionApi.ExchangeData`, `.Account`, `.Trading`|`PredictionApi`|
|Perpetual futures|`PerpsApi.ExchangeData`, `.Account`, `.Trading`|`PerpsApi.ExchangeData`, `.Account`, `.Trading`|

When migrating from the previous structure, replace `ClobApi` with `PredictionApi`.
Gamma market/event/search methods now live under `PredictionApi.ExchangeData`;
Data API position methods live under `PredictionApi.Trading`.

### Prediction market data

```csharp
using Polymarket.Net.Clients;

using var restClient = new PolymarketRestClient();
var markets = await restClient.PredictionApi.ExchangeData.GetMarketsAsync(closed: false, limit: 5);
if (!markets.Success)
{
    Console.WriteLine(markets.Error);
    return;
}

var firstMarket = markets.Data.FirstOrDefault();
// V1 markets use CLOB token ids; V2 markets use position ids.
var tokenIds = firstMarket?.Version == "v2" ? firstMarket.PositionIds : firstMarket?.ClobTokenIds;
if (tokenIds == null || tokenIds.Length == 0)
    return;

var books = await restClient.PredictionApi.ExchangeData.GetOrderBooksAsync(tokenIds);
if (!books.Success)
{
    Console.WriteLine(books.Error);
    return;
}

Console.WriteLine($"Loaded {books.Data.Length} order books.");
```

### Prediction authentication and orders

Prediction authentication has two levels. L1 uses a wallet key to create or derive
L2 credentials. L2 uses an API key, secret and passphrase for private CLOB endpoints.
Order placement also needs the L1 wallet key to sign orders.

Construct wallet keys with `new EthKey(privateKey: "PRIVATE_KEY")`. Choose the
`SignType` that matches the account:

|Sign type|Account|Address supplied to `WithL1`|
|--|--|--|
|`SignType.Poly1271`|Poly1271 wallet|Polymarket deposit address|
|`SignType.Email`|Email/Magic wallet|Polymarket funding address|
|`SignType.Proxy`|Browser wallet proxy|Polymarket funding address|
|`SignType.EOA`|Externally owned account|Polymarket funding address when required by the account|

```csharp
using var restClient = new PolymarketRestClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials().WithL1(
        SignType.Poly1271,
        new EthKey(privateKey: "PRIVATE_KEY"),
        "POLYMARKET_DEPOSIT_ADDRESS");
});

var credentials = await restClient.PredictionApi.Account.GetOrCreateApiCredentialsAsync();
if (!credentials.Success)
{
    Console.WriteLine(credentials.Error);
    return;
}
restClient.UpdateL2Credentials(credentials.Data);

// Buy 50 shares at $0.10 each ($5 before fees).
// Use ClobTokenIds for V1 markets, or PositionIds with marketVersion: 2 for V2.
var order = await restClient.PredictionApi.Trading.PlaceOrderAsync(
    tokenId: "TOKEN_ID",
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 50m,
    price: 0.1m,
    marketVersion: 1);
```

If L2 credentials are already available, configure both levels directly:

```csharp
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Enums;

var credentials = new PolymarketCredentials()
    .WithL1(SignType.Poly1271, new EthKey(privateKey: "PRIVATE_KEY"), "POLYMARKET_DEPOSIT_ADDRESS")
    .WithL2("L2_API_KEY", "L2_API_SECRET", "L2_API_PASSPHRASE");
```

### Prediction WebSocket subscriptions

```csharp
var socketClient = new PolymarketSocketClient();
var subscription = await socketClient.PredictionApi.SubscribeToTokenUpdatesAsync(
    new[] { "TOKEN_ID" },
    onBookUpdate: update => Console.WriteLine($"Book update: {update.Data}"),
    onBestBidAskUpdate: update => Console.WriteLine($"Best bid/ask: {update.Data}"));
	
Console.ReadLine();
```

For user order/trade subscriptions, configure prediction L2 credentials on the socket client. `UpdateL2Credentials` is available on both clients.

### Perpetual futures market data

Perps calls use symbol ids discovered through `GetSymbolsAsync()`. Conversion is also available via `restClient.PerpsApi.GetSymbolIdByName` and `GetSymbolNameById` after having called `GetSymbolsAsync`.

```csharp
var restClient = new PolymarketRestClient();
var symbols = await restClient.PerpsApi.ExchangeData.GetSymbolsAsync();
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}

var instrument = symbols.Data.FirstOrDefault();
if (instrument == null)
    return;

Console.WriteLine($"{instrument.Symbol}: {instrument.SymbolId}");
var book = await restClient.PerpsApi.ExchangeData.GetOrderBookAsync(instrument.SymbolId, depth: 10);
if (!book.Success)
{
    Console.WriteLine(book.Error);
    return;
}

Console.WriteLine($"Book: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");
```

### Perps authentication and orders

Perps uses a registered proxy key and its API secret. The proxy private key signs
trading operations; the secret authenticates private reads and user streams. Register
a proxy through `PerpsApi.Account.CreateProxyAsync`, using the owner's `signerKey`,
and retain the returned secret. For an existing proxy, configure its key and secret.

Initial proxy creation:  
```csharp
var client = new PolymarketRestClient(x =>
{
    x.ApiCredentials = new PolymarketCredentials()
        .WithPerps(null, EthKey.FromPrivateKey("<signing private key>"), EthKey.FromPrivateKey("<proxy private key>"));
});

// Call CreateProxy to register the proxy address used in the credentials and get a secret.
// The secret will be automatically applied if creation is successful,
// for later use the secret should be stored and reused.
var result = await client.PerpsApi.Account.CreateProxyAsync(
    DateTime.UtcNow.AddDays(30)); // Expiration time, after which the proxy can not be used anymore

if (!result.Success)
{
    Console.WriteLine($"Failed to create proxy: {result.Error}");
    return;
}

// Store the secret for later use
var secret = result.Data.Secret;

_ = await client.PerpsApi.Account.GetBalancesAsync();
```

With already registered proxy:
```csharp
var previousSecret = "<previously obtained secret>";
client = new PolymarketRestClient(x =>
{
    x.ApiCredentials = new PolymarketCredentials()
        .WithPerps(previousSecret, EthKey.FromPrivateKey("<signing private key>"), EthKey.FromPrivateKey("<proxy private key>"));
});

_ = await client.PerpsApi.Account.GetBalancesAsync();
```

### Perps WebSocket subscriptions

```csharp
using Polymarket.Net.Clients;

var symbolId = 7; // ETH-USD symbol
var socketClient = new PolymarketSocketClient();
var subscription = await socketClient.PerpsApi.ExchangeData.SubscribeToTickerUpdatesAsync(
    symbolId, update => Console.WriteLine($"Ticker: {update.Data}"));
```

Public streams include tickers, trades, order books, candles and statistics. Configure
Perps credentials on the socket client for private subscriptions under
`PerpsApi.Trading` (orders and fills) and `PerpsApi.Account` (balances and portfolio).

### Dependency injection

```csharp
services.AddPolymarket();

var restClient = provider.GetRequiredService<IPolymarketRestClient>();
var socketClient = provider.GetRequiredService<IPolymarketSocketClient>();
```

Set `options.ApiCredentials` through `services.AddPolymarket(options => { ... })` for
private access. Both injected clients expose `PredictionApi` and `PerpsApi`.

For client setup, dependency injection and response processing, see the
[documentation](https://cryptoexchange.jkorf.dev/client-libs/getting-started) or the
[examples](Examples), including the [AI-friendly examples](Examples/ai-friendly).

## AI / LLM documentation

Polymarket.Net includes AI-oriented documentation and examples for code generation tools:

|File|Purpose|
|--|--|
|[`AGENTS.md`](AGENTS.md)|Assistant skill with core Polymarket.Net patterns, pitfalls, and examples|
|[`CLAUDE.md`](CLAUDE.md)|Claude-oriented guide with authentication rules and source-of-truth files|
|[`llms.txt`](llms.txt)|Short LLM index with links to docs, examples, and critical usage rules|
|[`llms-full.txt`](llms-full.txt)|Detailed LLM context with endpoint routing, authentication levels, code patterns, and anti-hallucination checks|
|[`docs/ai-api-map.md`](docs/ai-api-map.md)|Table-style intent-to-method map for CLOB, Gamma, Data, WebSocket, authentication, DI, and local order book workflows|
|[`Examples/ai-friendly`](Examples/ai-friendly)|Compilable single-file Prediction and Perps examples for market data, authentication/trading, WebSocket, DI/order book, and error handling workflows|

See [cryptoexchange-skills-hub](https://github.com/JKorf/cryptoexchange-skills-hub) for installable skills.

**NOTE**  
Polymarket.Net uses the Builder Code mechanism for Polymarket, which means that an additional 1bps / 0.01% fee is charged on top of orders placed with the library to fund development. This is entirely optional: set `PredictionOptions.BuilderCode` or `PerpsOptions.BuilderCode` to `null` in the client options to disable it for that API.

## Updating from V4 to V5
* Client structure:
  * ClobApi has been renamed to PredictionApi on REST and WebSocket clients.
  * GammaApi methods have moved to PredictionApi.ExchangeData.
  * DataApi position queries have moved to PredictionApi.Trading.
  * Update explicit API client types and namespace imports from ClobApi to PredictionApi.
* Credentials:
  * WithL1 and PolymarketL1Credential now accept an EthKey instead of a private key string. Import CryptoExchange.Net.Web3 and pass new EthKey(privateKey: privateKey).
  * Credential properties L1 and L2 have been renamed to PredictionL1 and PredictionL2.
  * PolymarketL1Credential.PrivateKey has been replaced by WalletKey.
  * Create or derive L2 credentials through PredictionApi.Account.GetOrCreateApiCredentialsAsync() and apply them with UpdateL2Credentials.
* Prediction positions:
  * GetPositionsAsync has moved to PredictionApi.Trading and now returns PolymarketDataPage<PolymarketPosition>.
  * Replace offset-based pagination with cursor, using Pagination.NextCursor.
  * Replace GetClosedPositionsAsync with GetPositionsAsync(status: PositionStatus.Closed, ...).
* Prediction market versions:
  * For V1 markets, continue using ClobTokenIds.
  * For markets with Version == "v2", use PositionIds and pass marketVersion: 2 when placing orders.
* Client options:
  * ClobOptions, GammaOptions and DataOptions have been consolidated into PredictionOptions.
  * The REST-level BuilderCode setting has moved to PredictionOptions.BuilderCode.
* Local order books:
  * PolymarketClobSymbolOrderBook has been renamed to PolymarketPredictionSymbolOrderBook.
  * IPolymarketOrderBookFactory.CreateClob(tokenId) has been renamed to CreatePrediction(tokenId).

## CryptoExchange.Net
Polymarket.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|Bitstamp|[JKorf/Bitstamp.Net](https://github.com/JKorf/Bitstamp.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitstamp.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitstamp.Net)|
|Bitunix|[JKorf/Bitunix.Net](https://github.com/JKorf/Bitunix.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitunix.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitunix.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|LBank|[JKorf/LBank.Net](https://github.com/JKorf/LBank.Net)|[![Nuget version](https://img.shields.io/nuget/v/LBank.net.svg?style=flat-square)](https://www.nuget.org/packages/LBank.Net)|
|Lighter|[JKorf/Lighter.Net](https://github.com/JKorf/Lighter.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Lighter.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Lighter.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Pionex|[JKorf/Pionex.Net](https://github.com/JKorf/Pionex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Pionex.net.svg?style=flat-square)](https://www.nuget.org/packages/Pionex.Net)|
|Tapbit|[JKorf/Tapbit.Net](https://github.com/JKorf/Tapbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Tapbit.net.svg?style=flat-square)](https://www.nuget.org/packages/Tapbit.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|Weex|[JKorf/Weex.Net](https://github.com/JKorf/Weex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Weex.net.svg?style=flat-square)](https://www.nuget.org/packages/Weex.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Prediction REST API
|API|Supported|Location|
|--|--:|--|
|Events|✓|`restClient.PredictionApi.ExchangeData`|
|Markets|✓|`restClient.PredictionApi.ExchangeData`|
|Orderbook & Pricing|✓|`restClient.PredictionApi.ExchangeData`|
|Orders|✓|`restClient.PredictionApi.Trading`|
|Trades|✓|`restClient.PredictionApi.Trading`|
|CLOB Market Info|✓|`restClient.PredictionApi.ExchangeData`|
|Rebates|X||
|Rewards|X||
|Positions|✓|`restClient.PredictionApi.Trading`|
|Leaderboard|X||
|Builders|X||
|Search|✓|`restClient.PredictionApi.ExchangeData`|
|Tags|✓|`restClient.PredictionApi.ExchangeData`|
|Series|✓|`restClient.PredictionApi.ExchangeData`|
|Comments|X||
|Sports|✓|`restClient.PredictionApi.ExchangeData`|
|Bridge|X||
|Relayer|X||

### Prediction WebSocket API
|API|Supported|Location|
|--|--:|--|
|Market Channel|✓|`socketClient.PredictionApi`|
|User Channel|✓|`socketClient.PredictionApi`|
|Sport Channel|✓|`socketClient.PredictionApi`|
|Crypto / Equity Prices and TWAP|✓|`socketClient.PredictionApi`|

### Perps REST API

|API|Supported|Location|
|--|--:|--|
|Instruments, Tickers, Order Books and Trades|✓|`restClient.PerpsApi.ExchangeData`|
|Candles, Mark Prices and Funding History|✓|`restClient.PerpsApi.ExchangeData`|
|Exchange Info, Fees and Limit Tiers|✓|`restClient.PerpsApi.ExchangeData`|
|Proxy Registration and Credentials|✓|`restClient.PerpsApi.Account`|
|Balances, Portfolio and Account History|✓|`restClient.PerpsApi.Account`|
|Leverage and Isolated Margin|✓|`restClient.PerpsApi.Account`|
|Withdrawals and Internal Transfers|✓|`restClient.PerpsApi.Account`|
|Place, Edit and Cancel Orders|✓|`restClient.PerpsApi.Trading`|
|Open Orders, Order History and User Trades|✓|`restClient.PerpsApi.Trading`|

### Perps WebSocket API

|API|Supported|Location|
|--|--:|--|
|Tickers, Trades, Books, Candles and Statistics|✓|`socketClient.PerpsApi.ExchangeData`|
|Orders, Fills and TP/SL Updates|✓|`socketClient.PerpsApi.Trading`|
|Place and Cancel Orders|✓|`socketClient.PerpsApi.Trading`|
|Balances, Portfolio, Funding, Deposits and Withdrawals|✓|`socketClient.PerpsApi.Account`|
|Leverage and Isolated Margin|✓|`socketClient.PerpsApi.Account`|

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate in a different currency or network send me a message.
   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 5.0.0 - 04 Nov 2026
    * Perps API
      * Added Perps REST and WebSocket API support
      * Accessible via the PerpsApi on clients
      * Added PerpsOptions to options objects
      * Added Shared API implementation
        * Added (I)PolymarketSharedApiClient for Shared API access
      * Added Perps to PolymarketCredentials
      * Added PolymarketPerpsSymbolOrderBook order book implementation
      * Added PolymarketUserPerpsDataTracker user data tracker implementation
    * Prediction API
      * Moved prediction API logic
        * Renamed ClobApi to PredictionApi on REST and WebSocket clients
        * Moved Gamma endpoints to PredictionApi.ExchangeData
        * Moved Data position endpoints to PredictionApi.Trading
      * Moved builder options under PredictionOptions
      * Added support for V2 markets
        * Added Version and PositionIds to Gamma market models. V2 orders use PositionIds and marketVersion: 2.
        * Added marketVersion parameter to restClient.PredictionApi.Trading.PlaceOrderAsync endpoint
        * Added MarketVersion property to PolymarketOrderRequest
      * Added socketClient.PredictionApi.SubscribeToCryptoPriceUpdatesAsync
      * Added socketClient.PredictionApi.SubscribeToCryptoTwapPriceUpdatesAsync
      * Added socketClient.PredictionApi.SubscribeToEquityPriceUpdatesAsync
      * Added socketClient.PredictionApi.SubscribeToEquityTwapPriceUpdatesAsync
      * Added restClient.PredictionApi.Account.GetRateLimitTierAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.GetMarketsKeysetPaginationAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.GetMarketByTokenAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.GetResolutionStateAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.GetOpenInterestAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.GetEventVolumeAsync endpoint
      * Added restClient.PredictionApi.ExchangeData.LookupTokensAsync endpoint
      * Updated some response models
      * Updated Data API GetPositionsAsync endpoint to Data API v2, now under restClient.PredictionApi.Trading
      * Renamed L1/L2 on PolymarketCredentials to PredictionL1/PredictionL2
      * Updated PolymarketL1Credential and WithL1 to accept EthKey instead of a private key string
      * Renamed PolymarketClobSymbolOrderBook to PolymarketPredictionSymbolOrderBook

* Version 4.6.0 - 30 Sep 2026
    * Updated CryptoExchange.Net to V13.1.0

* Version 4.5.0 - 24 Sep 2026
    * Rate limiting
      * Added rate limit admission callback to client options to allow rate limit admission ruling on request definition
      * Added `WithRateLimitAdmissionAsync` to client to allow rate limit admission ruling on a specific request
      * Update rate limit safety margin logic
      * Fixed some rate limit calculation issues
    * Request coalescing
      * Sending identical public GET requests on the same client will only send a single request to the server and use the same response
      * Coalescing is enabled by default and can be disabled with the `RequestCoalescingEnabled` client option

* Version 4.4.0 - 21 Aug 2026
    * Updated to CryptoExchange.Net v12.5.0

* Version 4.3.0 - 29 Jul 2026
    * Updated CryptoExchange.Net to version 12.4.0
    * Added DataApi.GetClosedPositionsAsync endpoint
    * Updated DataApi.GetPositionsAsync endpoint parameters
    * Fixed deserialization issue PolymarketEvent when GameId is set

* Version 4.2.1 - 23 Jul 2026
    * Added GameId to PolymarketEvent model
    * Added Unmatched, Delayed values to OrderStatus Enum

* Version 4.2.0 - 21 Jul 2026
    * Updated CryptoExchange.Net to v12.2.0 

* Version 4.1.2 - 13 Jul 2026
    * Added restClient.ClobApi.ExchangeData.GetPriceHistoriesAsync endpoint

* Version 4.1.1 - 11 Jul 2026
    * Updated CryptoExchange.Net to version 12.1.1 to fix deserialization issue for certain timestamps
    * Fixed GetOrderBookAsync deserialization if there is no trade price

* Version 4.1.0 - 09 Jul 2026
    * Updated CryptoExchange.Net to v12.1.0
    * Added 0.005 and 0.0025 tick size support
    * Added LastPrice to PolymarketOrderBook model
    * Fixed Gamma API timestamp parameter serialization
    * Fixed PolymarketNegRisk deserialization

* Version 4.0.0 - 29 Jun 2026
    * Result types:
      * (Web)CallResult types are replaced by HttpResult and WebSocketResult with the same logic
      * WebSocketResult now return additional info for websocket operations
      * Updated result types to record type
      * Removed implicit result type conversion to bool, `if (result)` no longer works, instead use `if (result.Success)`
      * Fixed result object nullability hinting, for example Data might be null if Success isn't checked for true
    * Clients:
      * Added ToString overrides on base API types
      * Added Exchange property on BaseApiClient
      * Added ApiCredentials property on Api clients
      * Updated ILogger source from client name to topic specific client name
      * Removed logging from client creation
      * Fixed issue in SocketApiClient.GetSocketConnection causing requests to always wait the full max 10 seconds when there was a reconnecting socket
    * Added bypass of order book request for place order if possible
    * Added SupportedEnvironments property to PlatformInfo
    * Added setter to PolymarketPlatform.RateLimiter to allow custom rate limit settings
    * Various small performance improvements
    * Fixed websocket connection attempts counting towards rate limit even when server could not be reached
