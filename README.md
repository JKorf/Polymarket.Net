# ![Polymarket.Net](https://raw.githubusercontent.com/JKorf/Polymarket.Net/main/Polymarket.Net/Icon/icon.png) Polymarket.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Polymarket.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Polymarket.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Polymarket.Net?style=for-the-badge)

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

GitHub Copilot and Cursor instructions are also provided in `.github/copilot-instructions.md` and `.cursor/rules/polymarket-net.mdc`; both point back to the root AI context files.

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
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
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
* Version 3.1.0 - 09 May 2026
    * Added support for Poly1271 signing

* Version 3.0.3 - 01 May 2026
    * Fixed incorrect makerAddress parameter serialization in GetUserTradesAsync

* Version 3.0.2 - 01 May 2026
    * Improved order placement performance
    * Fixed side parameter missing in PlaceMultipleOrdersAsync endpoint
    * Fixed PlaceMultipleOrdersAsync not sending side parameter
    * Fixed CancelOrdersOnMarketAsync incorrect endpoint
    * Fixed restClient.ClobApi.Account.GetBuilderTradesAsync endpoint

* Version 3.0.1 - 29 Apr 2026
    * Removed FeeRateBps from PolymarketOrderBase model fixing deserialization exception in websocket user updates

* Version 3.0.0 - 28 Apr 2026
    * Updated library to use V2 Clob API
    * Updated CryptoExchange.Net to version 11.1.1
    * Added restClient.ClobApi.ExchangeData.GetMarketInfoAsync endpoint
    * Added tokenId parameter to GetUserTradesAsync endpoint
    * Added BuilderCode to REST client options
    * Updated GetUserTradesAsync endpoint from /data/trades to /trades
    * Updated PostOrderHeartbeatAsync endpoint from /v1/heartbeats to /heartbeats
    * Updated documentation references
    * Renamed all ConditionId parameters/properties to MarketId for consistency
    * Renamed all AssetId parameters/properties to TokenId for consistency
    * Removed deprecated takerAddress, nonce and feeRateBps from PlaceOrder endpoints
    * Removed takerAddress parameter from GetUserTradesAsync endpoint
    * Removed BuilderApiKey, BuilderSecret, BuilderPass from REST client options

    * Notes for updating:
        * This release only supports request signing for the V2 Clob API, which will go live after the 28th of April 2026 11:00 UTC. To test this release before the V2 API is live update the CLOB rest address to `https://clob-v2.polymarket.com` in the client options Environment setting
        * A default builder fee of 0.01% / 1bps has been enabled by default to support development, this can be turned of in the client options by setting BuilderCode to null

* Version 2.2.0 - 24 Apr 2026
    * Added QuantityType parameter to PlaceOrder endpoints which allows to specify market buy orders in USD value instead of number of shares
    * Updated book caching logic to only store for 2 seconds instead of indefinitely to fix outdated info preventing correct order placement
    * Fixed PolymarketTrade Role mapping

* Version 2.1.1 - 13 Apr 2026
    * Default timeInForce to ImmediateOrCancel for market orders if parameter not provided
    * Fixed order quantity rounding issues

* Version 2.1.0 - 09 Apr 2026
    * Updated CryptoExchange.Net to version 11.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Added Polymarket Data API support
    * Improved socketClient.ClobApi.SubscribeToTokenUpdatesAsync registration logic
    * Fixed parameters not getting send with various requests
    * Fixed startTimeMin, startTimeMax parameter serialization on GammaApi GetMarketsAsync and GetEventsAsync

* Version 2.0.0 - 24 Mar 2026
    * Updated CryptoExchange.Net to version 11.0.1, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Updated class for supplying API credentials from ApiCredentials to PolymarketCredentials
    * Updated Shared order status parsing to default to Unknown value if not parsable
    * Updated signing logic to unified logic in the CryptoExchange.Net library
    * Added cursor parameter to restClient.ClobApi.Trading.GetOpenOrdersAsync endpoint
    * Updated PolymarketEvent ParentEvent to ParentEventId and changed type to long?
    * Fixed SubscribeToPlatformUpdatesAsync onMarketResolved handling
    * Fixed exception in PolymarketSocketClient.UpdateL2Credentials
    * Fixed parameter serialization restClient.ClobApi.Account.GetBuilderTradesAsync endpoint
    * Fixed parameter serialization restClient.ClobApi.Trading.GetOpenOrdersAsync endpoint

    * Notes for updating:
        * Update ApiCredentials to PolymarketCredentials for authentication, and provided L1 and L2 credentials separately, i.e. `ApiCredentials = new ApiCredentials(signType, l1key, l1fundkey, l2key, l2sec, l2pass)` => `ApiCredentials = new PolymarketCredentials(new PolymarketL1Credential(signType, l1key, l1fundkey), new HMACPassCredential(l2key, l2sec, l2pass))`

* Version 1.5.2 - 10 Mar 2026
    * Updated order quantity/price rounding
    * Updated xml comments to include json fields

* Version 1.5.1 - 06 Mar 2026
    * Fixed bug in market order buy price determination
    * Fixed exception in PlaceOrderAsync when placing a market order when order book is empty
    * Fixed incorrect parameter serialization in restClient.ClobApi.Trading.GetUserTradesAsync endpoint
    * Fixed order expiration serialization in PlaceOrder endpoints
    * Fixed conditionIds not getting sent in restClient.GammaApi.GetMarketsAsync endpoint

* Version 1.5.0 - 06 Mar 2026
    * Updated CryptoExchange.Net to version 10.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Improved method XML comments

* Version 1.4.1 - 26 Feb 2026
    * Updated LastTradePrice on PolymarketBookUpdate model to nullable

* Version 1.4.0 - 24 Feb 2026
    * Updated CryptoExchange.Net to version 10.7.0
    * Added restClient.ClobApi.Trading.PostOrderHeartbeatAsync endpoint
    * Added websocket ping message sending
    * Added additional Http settings to client options
    * Updated Shared REST interfaces pagination logic
    * Updated HttpClient registration, fixing issue of DNS changes not getting processed
    * Fixed UserClientProvider using unconfigured HttpClient

* Version 1.3.1 - 17 Feb 2026
    * Updated CryptoExchange.Net to version 10.6.2, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Fixed not correctly handling book snapshot websocket update

* Version 1.3.0 - 16 Feb 2026
    * Updated CryptoExchange.Net to version 10.6.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Fixed SymbolOrderBook websocket subscription not getting closed if when waiting for initial data times out

* Version 1.2.0 - 10 Feb 2026
    * Updated CryptoExchange.Net to version 10.5.1, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Updated UserClientProvider internal client cache to non-static to prevent cleanup issues
    * Fixed websocket token subscription price change topic mapping

* Version 1.1.0 - 06 Feb 2026
    * Updated CryptoExchange.Net to version 10.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Added OrderStatus.Matched value
    * Fixed disposed clients getting returned from UserClientProvider

* Version 1.0.1 - 27 Jan 2026
    * Fixed signing issue certain token values
    * Fixed rounding issue in quantity calculation

* Version 1.0.0 - 22 Jan 2026
    * Initial release

