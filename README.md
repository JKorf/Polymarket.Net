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
* REST Endpoints
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
* Websocket streams
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
Authenticate using an email account and providing the exported private key and the funding address. This will require you to request the layer 2 credentials before orders can be placed:
```csharp
var credsEmailLayer1 = new PolymarketCredentials(
    SignType.Email, // Email wallet, when creating a new wallet via the web interface
    "0x00..", // The private key, can be exported from the web interface
    "0x00.."); // The polymarket funding address, can be found in the web interface under `Profile -> Your Polymarket Wallet Address`
```

Authenticate using an email account and providing the exported private key and the funding address, while also providing previously requested layer 2 credentials. Can be used to place orders directly:
```csharp
var credsEmailWithLayer2 = new PolymarketCredentials(
    SignType.Email,// Email wallet, when creating a new wallet via the web interface
    "0x00..", // The private key, can be exported from the web interface
    "KEY",// The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    "PASS", // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    "0x00.."); // The polymarket funding address, can be found in the web interface under `Profile -> Your Polymarket Wallet Address`
```

Authenticate using an external account, for example MetaMask, and providing the private key. This will require you to request the layer 2 credentials before orders can be placed:
```csharp
var credsEoaLayer1 = new PolymarketCredentials(
    SignType.EOA, // Externally Owned Account wallet, when using an existing wallet to connect to polymarket
    "0x00.." // The private key for the wallet
    );
```

Authenticate using an external account, for example MetaMask, and providing the private key, while also providing previously requested layer 2 credentials. Can be used to place orders directly:
```csharp
var credsEoaWithLayer2 = new PolymarketCredentials(
    SignType.EOA, // Externally Owned Account wallet, when using an existing wallet to connect to polymarket
    "0x00..", // The private key for the wallet
    "KEY", // The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    "PASS" // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    );
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
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### REST Central Limit Order Book (CLOB) API
|API|Supported|Location|
|--|--:|--|
|Orderbook|✓|`restClient.ClobApi.ExchangeData`|
|Pricing|✓|`restClient.ClobApi.ExchangeData`|
|Spreads|✓|`restClient.ClobApi.ExchangeData`|
|Historical Timeseries Data|✓|`restClient.ClobApi.ExchangeData`|
|Order Management|✓|`restClient.ClobApi.Trading`|
|Trades|✓|`restClient.ClobApi.Trading`|

### REST Gamma API
|API|Supported|Location|
|--|--:|--|
|Sports|✓|`restClient.GammaApi`|
|Tags|✓|`restClient.GammaApi`|
|Events|✓|`restClient.GammaApi`|
|Markets|✓|`restClient.GammaApi`|
|Series|✓|`restClient.GammaApi`|
|Comments|X||
|Profiles|X||
|Search|✓|`restClient.GammaApi`|

### Websocket API
|API|Supported|Location|
|--|--:|--|
|User Channel|✓|`socketClient.ClobApi`|
|Market Channel|✓|`socketClient.ClobApi`|
|Sports websocket|✓|`socketClient.ClobApi`|

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

