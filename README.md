# ![Polymarket.Net](https://raw.githubusercontent.com/JKorf/Polymarket.Net/main/Polymarket.Net/Icon/icon.png) Polymarket.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Polymarket.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Polymarket.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Polymarket.Net?style=for-the-badge)
![Since](https://img.shields.io/badge/since-2025-brightgreen?style=for-the-badge)

Polymarket.Net is a client library for accessing the [Polymarket REST and Websocket API](https://docs.polymarket.com/developers/CLOB/introduction). 

## Features
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
*Basic request:* 
```csharp
// Get the order book info for the outcomes of the first market via rest request
var markets = await polymarketRestClient.GammaApi.GetMarketsAsync(closed: false);
if (!markets.Success)
{
	Console.WriteLine("Failed: " + markets.Error);
	return;
}

var firstMarket = markets.Data[0];
var bookInfo = await polymarketRestClient.ClobApi.ExchangeData.GetOrderBooksAsync(firstMarket.ClobTokenIds!);
```
	
*Place order:*
```csharp
var restClient = new PolymarketRestClient(opts => {
	opts.ApiCredentials = new PolymarketCredentials(new PolymarketL1Credential(
        SignType.Poly1271,
        "PRIVATEKEY",
        "DEPOSITADDRESS"
        ));
});

// Update the client with layer 2 credentials
var credentials = await polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync();
polymarketRestClient.UpdateL2Credentials(credentials.Data);

// Place Limit order to buy 50 shares at 0.1 ($10)
var tokenIdTest = "67565972075898091709163371829761231762318232475740950317083391526954889294846";
var result = await polymarketRestClient.ClobApi.Trading.PlaceOrderAsync(
    tokenIdTest, 
	OrderSide.Buy,
	OrderType.Limit, 
	50, 
	price: 0.1m);
```

*WebSocket subscription:*
```csharp
// Subscribe to updates for a specific token/asset via the websocket API
var socketClient = new PolymarketSocketClient();
var tokenId = "11862165566757345985240476164489718219056735011698825377388402888080786399275";
var subscriptionResult = await polymarketSocketClient.ClobApi.SubscribeToTokenUpdatesAsync([tokenId2],
	priceUpdate =>
	{
		// Handle price change update
	},
	bookUpdate =>
	{
		// Handle order book update
	},
	lastTradePriceUpdate =>
	{
		// Handle last trade price update
	},
	tickSizeUpdate =>
	{
		// Handle tick size update
	},
	bestBidAskUpdate =>
	{
		// Handle best bid/ask change update
	});
```

### Authentication
Authenticate using Poly1271 signing and a deposit address, providing the private key and the deposit address. This should be used for Polymarket accounts created after 04 May 2026. This will require you to request the layer 2 credentials before orders can be placed:
```csharp
var credsPoly1271Layer1 = new PolymarketCredentials(
	new PolymarketL1Credential(
		SignType.Poly1271, // Poly1271 signing, for accounts created after 4 May 2026
		"0x00..", // The private key for the wallet
		"0x00..")); // The polymarket deposit address, can be found in the web interface under `Profile -> Copy Address`
```

Authenticate using Poly1271 signing and a deposit address, providing the private key and the deposit address, while also providing previously requested layer 2 credentials. Can be used to place orders directly:
```csharp
var credsPoly1271WithLayer2 = new PolymarketCredentials(
    new PolymarketL1Credential(
		SignType.Poly1271, // Poly1271 signing, for accounts created after 4 May 2026
		"0x00..", // The private key for the wallet
		"0x00..")); // The polymarket deposit address, can be found in the web interface under `Profile -> Copy Address`
    new HMACPassCredential(
        "KEY",// The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "PASS" // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    ));
```

Authenticate using an email account and providing the exported private key and the funding address. This will require you to request the layer 2 credentials before orders can be placed:
```csharp
var credsEmailLayer1 = new PolymarketCredentials(
	new PolymarketL1Credential(
		SignType.Email, // Email wallet, when creating a new wallet via the web interface
		"0x00..", // The private key, can be exported from the web interface
		"0x00..")); // The polymarket funding address, can be found in the web interface under `Profile -> Your Polymarket Wallet Address`
```

Authenticate using an email account and providing the exported private key and the funding address, while also providing previously requested layer 2 credentials. Can be used to place orders directly:
```csharp
var credsEmailWithLayer2 = new PolymarketCredentials(
    new PolymarketL1Credential(
        SignType.Email,// Email wallet, when creating a new wallet via the web interface
        "0x00..",// The private key, can be exported from the web interface
        "0x00.."), // The polymarket funding address, can be found in the web interface under `Profile -> Your Polymarket Wallet Address`
    new HMACPassCredential(
        "KEY",// The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "PASS" // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    ));
```

Authenticate using an external account, for example MetaMask, and providing the private key. This will require you to request the layer 2 credentials before orders can be placed:
```csharp
var credsEoaLayer1 = new PolymarketCredentials(
    new PolymarketL1Credential(
        SignType.EOA, // Externally Owned Account wallet, when using an existing wallet to connect to polymarket
        "0x00..")); // The private key for the wallet
```

Authenticate using an external account, for example MetaMask, and providing the private key, while also providing previously requested layer 2 credentials. Can be used to place orders directly:
```csharp
var credsEoaWithLayer2 = new PolymarketCredentials(
    new PolymarketL1Credential(
        SignType.EOA, // Externally Owned Account wallet, when using an existing wallet to connect to polymarket
        "0x00.." // The private key for the wallet
    ),
    new HMACPassCredential(
        "KEY", // The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
        "PASS" // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    ));
```

Retrieve and set layer 2 credentials need for placing orders (required when L2 credentials not provided in the credentials):
```csharp
var credentialResult = await polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync();
if (credentialResult.Success)
    polymarketRestClient.UpdateL2Credentials(credentialResult.Data);
```

Set the previously created credentials:
```csharp
// Via constructor
var client = new PolymarketRestClient(options =>
{
    options.ApiCredentials = credentials;
});

// Via dependency injection
services.AddPolymarket(options =>
{
    options.ApiCredentials = credentials
});
```

For information on the clients, dependency injection, response processing and more see the [documentation](https://cryptoexchange.jkorf.dev/client-libs/getting-started), or have a look at the examples [here](https://github.com/JKorf/Polymarket.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## AI / LLM documentation

Polymarket.Net includes AI-oriented documentation and examples for code generation tools:

|File|Purpose|
|--|--|
|[`AGENTS.md`](AGENTS.md)|Assistant skill with core Polymarket.Net patterns, pitfalls, and examples|
|[`CLAUDE.md`](CLAUDE.md)|Claude-oriented guide with authentication rules and source-of-truth files|
|[`llms.txt`](llms.txt)|Short LLM index with links to docs, examples, and critical usage rules|
|[`llms-full.txt`](llms-full.txt)|Detailed LLM context with endpoint routing, authentication levels, code patterns, and anti-hallucination checks|
|[`docs/ai-api-map.md`](docs/ai-api-map.md)|Table-style intent-to-method map for CLOB, Gamma, Data, WebSocket, authentication, DI, and local order book workflows|
|[`Examples/ai-friendly`](Examples/ai-friendly)|Compilable single-file examples for common REST, authentication/trading, WebSocket, DI/order book, and error handling workflows|

See [cryptoexchange-skills-hub](https://github.com/JKorf/cryptoexchange-skills-hub) for installable skills.

**NOTE**  
Polymarket.Net uses the Builder Code mechanism for Polymarket, which means that an additional 1bps / 0.01% fee is charged on top of orders placed with the library to fund development. This is entirely optional and can be disabled in the client options by setting `BuilderCode` to `null` in the REST client options.

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
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|Bitstamp|[JKorf/Bitstamp.Net](https://github.com/JKorf/Bitstamp.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitstamp.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitstamp.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Lighter|[JKorf/Lighter.Net](https://github.com/JKorf/Lighter.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Lighter.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Lighter.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Pionex|[JKorf/Pionex.Net](https://github.com/JKorf/Pionex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Pionex.net.svg?style=flat-square)](https://www.nuget.org/packages/Pionex.Net)|
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

### REST API
|API|Supported|Location|
|--|--:|--|
|Events|✓|`restClient.ClobApi.ExchangeData`|
|Markets|✓|`restClient.ClobApi.ExchangeData`|
|Orderbook & Pricing|✓|`restClient.ClobApi.ExchangeData`|
|Orders|✓|`restClient.ClobApi.Trading` / `restClient.ClobApi.Account`|
|Trades|✓|`restClient.ClobApi.Trading` / `restClient.ClobApi.Account`|
|CLOB Markets|✓|`restClient.ClobApi.ExchangeData`|
|Rebates|X||
|Rewards|X||
|Profile|partial|`restClient.DataApi`|
|Leaderboard|X||
|Builders|X||
|Search|✓|`restClient.GammaApi`|
|Tags|✓|`restClient.GammaApi`|
|Series|✓|`restClient.GammaApi`|
|Comments|X||
|Sports|✓|`restClient.GammaApi`|
|Bridge|X||
|Relayer|X||

### WebSocketocket API
|API|Supported|Location|
|--|--:|--|
|Market Channel|✓|`socketClient.ClobApi`|
|User Channel|✓|`socketClient.ClobApi`|
|Sport Channel|✓|`socketClient.ClobApi`|

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
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
