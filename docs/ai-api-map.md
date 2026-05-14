# Polymarket.Net AI API Quick Map

Use this file to route common user intents to the correct Polymarket.Net client member. If a method name or parameter is not listed here, inspect `Polymarket.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new PolymarketRestClient()` |
| WebSocket streams | `new PolymarketSocketClient()` |
| Public market/event/search data | No credentials required |
| L1 wallet authentication | `options.ApiCredentials = new PolymarketCredentials().WithL1(signType, privateKey, polymarketFundingAddress)` |
| L2 API authentication | `.WithL2(key, secret, passphrase)` or `new HMACPassCredential(key, secret, passphrase)` |
| Derive/create L2 credentials | `client.ClobApi.Account.GetOrCreateApiCredentialsAsync()` |
| Apply returned L2 credentials | `client.UpdateL2Credentials(credentials.Data)` |
| Live environment | `PolymarketEnvironment.Live` |
| Test environment | `PolymarketEnvironment.Testnet` |
| Custom environment | `PolymarketEnvironment.CreateCustom(...)` |
| Dependency injection | `services.AddPolymarket(options => { ... })` |
| Multi-user clients | `IPolymarketUserClientProvider` |
| Local order book factory | `IPolymarketOrderBookFactory.CreateClob(tokenId)` |

## Authentication

| User intent | Polymarket.Net member |
|---|---|
| Public CLOB/Gamma/Data API read | `new PolymarketRestClient()` |
| Configure L1 EOA credentials | `new PolymarketCredentials().WithL1(SignType.EOA, privateKey, polymarketFundingAddress)` |
| Configure L1 email credentials | `new PolymarketCredentials().WithL1(SignType.Email, privateKey, polymarketFundingAddress)` |
| Configure L1 proxy credentials | `new PolymarketCredentials().WithL1(SignType.Proxy, privateKey, polymarketFundingAddress)` |
| Configure L1 Poly1271 credentials | `new PolymarketCredentials().WithL1(SignType.Poly1271, privateKey, polymarketFundingAddress)` |
| Configure L1 and L2 credentials | `new PolymarketCredentials().WithL1(...).WithL2(key, secret, passphrase)` |
| Create L2 API credentials | `client.ClobApi.Account.CreateApiCredentialsAsync(nonce)` |
| Derive existing L2 API credentials | `client.ClobApi.Account.GetApiCredentialsAsync(nonce)` |
| Get or create L2 API credentials | `client.ClobApi.Account.GetOrCreateApiCredentialsAsync(nonce)` |
| List API keys | `client.ClobApi.Account.GetApiKeysAsync()` |
| Delete API key | `client.ClobApi.Account.DeleteApiKeyAsync()` |
| Update REST client with L2 credentials | `restClient.UpdateL2Credentials(polymarketCreds)` |
| Update socket client with L2 credentials | `socketClient.UpdateL2Credentials(polymarketCreds)` |

## CLOB Exchange Data REST

| User intent | Polymarket.Net member |
|---|---|
| Get server time | `client.ClobApi.ExchangeData.GetServerTimeAsync()` |
| Get geographic restrictions | `client.ClobApi.ExchangeData.GetGeographicRestrictionsAsync()` |
| Get CLOB markets | `client.ClobApi.ExchangeData.GetMarketsAsync(cursor)` |
| Get CLOB simplified markets | `client.ClobApi.ExchangeData.GetSimplifiedMarketsAsync(cursor)` |
| Get sampling markets | `client.ClobApi.ExchangeData.GetSamplingMarketsAsync(cursor)` |
| Get sampling simplified markets | `client.ClobApi.ExchangeData.GetSamplingSimplifiedMarketsAsync(cursor)` |
| Get CLOB market by id | `client.ClobApi.ExchangeData.GetMarketAsync(id)` |
| Get CLOB market info by condition id | `client.ClobApi.ExchangeData.GetMarketInfoAsync(marketId)` |
| Get buy or sell price for token | `client.ClobApi.ExchangeData.GetPriceAsync(tokenId, OrderSide.Buy)` |
| Get prices for multiple tokens/sides | `client.ClobApi.ExchangeData.GetPricesAsync(new Dictionary<string, OrderSide> { ... })` |
| Get midpoint price | `client.ClobApi.ExchangeData.GetMidpointPriceAsync(tokenId)` |
| Get midpoint prices | `client.ClobApi.ExchangeData.GetMidpointPricesAsync(tokenIds)` |
| Get price history | `client.ClobApi.ExchangeData.GetPriceHistoryAsync(market, startTime, endTime, interval, fidelity)` |
| Get bid/ask spread for token | `client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(tokenId)` |
| Get bid/ask spreads for tokens | `client.ClobApi.ExchangeData.GetBidAskSpreadsAsync(tokenIds)` |
| Get order book | `client.ClobApi.ExchangeData.GetOrderBookAsync(tokenId)` |
| Get multiple order books | `client.ClobApi.ExchangeData.GetOrderBooksAsync(tokenIds)` |
| Get tick size | `client.ClobApi.ExchangeData.GetTickSizeAsync(tokenId)` |
| Get negative risk | `client.ClobApi.ExchangeData.GetNegativeRiskAsyncAsync(tokenId)` |
| Get fee rate in bps | `client.ClobApi.ExchangeData.GetFeeRateBpsAsync(tokenId)` |
| Get last trade price | `client.ClobApi.ExchangeData.GetLastTradePriceAsync(tokenId)` |
| Get last trade prices | `client.ClobApi.ExchangeData.GetLastTradePricesAsync(tokenIds)` |

## CLOB Account REST

| User intent | Polymarket.Net member |
|---|---|
| Get closed-only mode | `client.ClobApi.Account.GetClosedOnlyModeAsync()` |
| Get notifications | `client.ClobApi.Account.GetNotificationsAsync()` |
| Drop notifications | `client.ClobApi.Account.DropNotificationsAsync(ids)` |
| Get collateral balance/allowance | `client.ClobApi.Account.GetBalanceAllowanceAsync(AssetType.Collateral)` |
| Get conditional token balance/allowance | `client.ClobApi.Account.GetBalanceAllowanceAsync(AssetType.Conditional, tokenId)` |
| Update collateral allowance | `client.ClobApi.Account.UpdateBalanceAllowanceAsync(AssetType.Collateral)` |
| Update conditional token allowance | `client.ClobApi.Account.UpdateBalanceAllowanceAsync(AssetType.Conditional, tokenId)` |
| Get builder trades | `client.ClobApi.Account.GetBuilderTradesAsync(builderCode, ...)` |

## CLOB Trading REST

| User intent | Polymarket.Net member |
|---|---|
| Get open orders | `client.ClobApi.Trading.GetOpenOrdersAsync(orderId, marketId, tokenId, cursor)` |
| Get order by id | `client.ClobApi.Trading.GetOrderAsync(orderId)` |
| Get order reward scoring | `client.ClobApi.Trading.GetOrderRewardScoringAsync(orderId)` |
| Get multiple orders reward scoring | `client.ClobApi.Trading.GetOrdersRewardScoringAsync(orderIds)` |
| Place limit order | `client.ClobApi.Trading.PlaceOrderAsync(tokenId, side, OrderType.Limit, quantity, price: price)` |
| Place market order | `client.ClobApi.Trading.PlaceOrderAsync(tokenId, side, OrderType.Market, quantity, quantityType: quantityType)` |
| Place order with time in force | `client.ClobApi.Trading.PlaceOrderAsync(..., timeInForce: TimeInForce.GoodTillCanceled)` |
| Place post-only order | `client.ClobApi.Trading.PlaceOrderAsync(..., postOnly: true)` |
| Place multiple orders | `client.ClobApi.Trading.PlaceMultipleOrdersAsync(requests)` |
| Cancel order | `client.ClobApi.Trading.CancelOrderAsync(orderId)` |
| Cancel multiple orders | `client.ClobApi.Trading.CancelOrdersAsync(orderIds)` |
| Cancel orders on market/token | `client.ClobApi.Trading.CancelOrdersOnMarketAsync(marketId, tokenId)` |
| Cancel all open orders | `client.ClobApi.Trading.CancelAllOrdersAsync()` |
| Get user trades | `client.ClobApi.Trading.GetUserTradesAsync(tradeId, makerAddress, marketId, tokenId, startTime, endTime, cursor)` |
| Send order heartbeat | `client.ClobApi.Trading.PostOrderHeartbeatAsync()` |

## Gamma REST

| User intent | Polymarket.Net member |
|---|---|
| Get sports teams | `client.GammaApi.GetSportTeamsAsync(...)` |
| Get sports metadata | `client.GammaApi.GetSportsAsync()` |
| Get sport market types | `client.GammaApi.GetSportMarketTypesAsync()` |
| Get tags | `client.GammaApi.GetTagsAsync(...)` |
| Get tag by id | `client.GammaApi.GetTagByIdAsync(id)` |
| Get tag by slug | `client.GammaApi.GetTagBySlugAsync(slug)` |
| Get related tags by id | `client.GammaApi.GetRelatedTagsByIdAsync(id)` |
| Get related tags by slug | `client.GammaApi.GetRelatedTagsBySlugAsync(slug)` |
| Get tags related to tag by id | `client.GammaApi.GetTagsRelatedToTagByIdAsync(id)` |
| Get tags related to tag by slug | `client.GammaApi.GetTagsRelatedToTagBySlugAsync(slug)` |
| Get events | `client.GammaApi.GetEventsAsync(...)` |
| Get event by id | `client.GammaApi.GetEventByIdAsync(id)` |
| Get event by slug | `client.GammaApi.GetEventBySlugAsync(slug)` |
| Get event tags | `client.GammaApi.GetEventTagsAsync(id)` |
| Get Gamma markets | `client.GammaApi.GetMarketsAsync(...)` |
| Get Gamma market by id | `client.GammaApi.GetMarketByIdAsync(id)` |
| Get Gamma market by slug | `client.GammaApi.GetMarketBySlugAsync(slug)` |
| Get market tags | `client.GammaApi.GetMarketTagsAsync(id)` |
| Get series | `client.GammaApi.GetSeriesAsync(...)` |
| Get series by id | `client.GammaApi.GetSeriesByIdAsync(id)` |
| Search markets/events/profiles | `client.GammaApi.SearchAsync(query, ...)` |

## Data REST

| User intent | Polymarket.Net member |
|---|---|
| Get current positions for user | `client.DataApi.GetPositionsAsync(user)` |

## WebSocket

| User intent | Polymarket.Net member |
|---|---|
| Subscribe platform new market/resolution updates | `socketClient.ClobApi.SubscribeToPlatformUpdatesAsync(onNewMarketUpdate, onMarketResolvedUpdate)` |
| Subscribe token price/book/trade/tick/best bid-ask updates | `socketClient.ClobApi.SubscribeToTokenUpdatesAsync(tokenIds, onPriceChangeUpdate, onBookUpdate, onLastTradePriceUpdate, onTickSizeUpdate, onBestBidAskUpdate)` |
| Subscribe authenticated user order/trade updates | `socketClient.ClobApi.SubscribeToUserUpdatesAsync(onOrderUpdate, onTradeUpdate)` |
| Subscribe sports updates | `socketClient.ClobApi.SubscribeToSportsUpdatesAsync(onSportsUpdate)` |
| Unsubscribe from socket stream | `socketClient.UnsubscribeAsync(subscription.Data)` |

## Local Order Book And User Client Provider

| User intent | Polymarket.Net member |
|---|---|
| Create local CLOB order book directly | `new PolymarketClobSymbolOrderBook(tokenId)` |
| Create local CLOB order book from DI | `orderBookFactory.CreateClob(tokenId)` |
| Register DI services | `services.AddPolymarket(options => { ... })` |
| Initialize per-user clients | `userClientProvider.InitializeUserClient(userIdentifier, credentials, environment)` |
| Get per-user REST client | `userClientProvider.GetRestClient(userIdentifier, credentials, environment)` |
| Get per-user socket client | `userClientProvider.GetSocketClient(userIdentifier, credentials, environment)` |

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Order accepted check | Check `order.Data.Success` after `order.Success` |
| Cancellation | Pass `ct: cancellationToken` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| `PolymarketClient` | `PolymarketRestClient` or `PolymarketSocketClient` |
| `ApiCredentials` directly | `PolymarketCredentials` |
| `SpotApi`, `FuturesApi`, `GeneralApi` | `ClobApi`, `GammaApi`, `DataApi` |
| Symbol pairs such as `BTCUSDT` for orders | Polymarket token ids |
| `.SharedClient` | Not exposed by current Polymarket.Net interfaces |
| `.Data` without `.Success` check | Check `.Success` first |
| L1-only credentials for order placement | Add or derive L2 credentials |
| `PolymarketOrderResult.Success` only | Check `WebCallResult.Success` first, then order result success |
| Manual socket lifecycle without storing subscription | Store `UpdateSubscription` and unsubscribe |
