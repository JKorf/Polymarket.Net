# Polymarket.Net v5.0.0 AI API Quick Map

In v5.0.0, CLOB calls use `PredictionApi`; Gamma methods are in
`PredictionApi.ExchangeData`, and Data positions are in `PredictionApi.Trading`.
`GetMarketsAsync` returns a Gamma market array, not a CLOB cursor page.
Prediction V1 markets use `ClobTokenIds`; V2 markets use `PositionIds` and
`marketVersion: 2` when placing orders. Prediction prices are between 0 and 1.
Concrete Perps calls use numeric instrument ids and instrument price/quantity
precision. `EthKey` is in `CryptoExchange.Net.Web3`.

Use this file to route common user intents to the correct Polymarket.Net client member. If a method name or parameter is not listed here, inspect `Polymarket.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new PolymarketRestClient()` |
| WebSocket streams | `new PolymarketSocketClient()` |
| Public market/event/search data | No credentials required |
| L1 wallet authentication | `options.ApiCredentials = new PolymarketCredentials().WithL1(signType, new EthKey(privateKey: privateKey), polymarketFundingAddress)` |
| L2 API authentication | `.WithL2(key, secret, passphrase)` or `new HMACPassCredential(key, secret, passphrase)` |
| Perps proxy authentication | `.WithPerps(secret: secret, signerKey: null, proxyKey: new EthKey(privateKey: proxyPrivateKey))` |
| Combined prediction and Perps credentials | `.WithL1(...).WithL2(...).WithPerps(...)` |
| Credential properties | `PredictionL1`, `PredictionL2`, `Perps` |
| Derive/create L2 credentials | `client.PredictionApi.Account.GetOrCreateApiCredentialsAsync()` |
| Apply returned L2 credentials | `client.UpdateL2Credentials(credentials.Data)` |
| Live environment | `PolymarketEnvironment.Live` |
| Test environment | `PolymarketEnvironment.Testnet` |
| Custom environment | `PolymarketEnvironment.CreateCustom(...)` |
| Dependency injection | `services.AddPolymarket(options => { ... })` |
| Multi-user clients | `IPolymarketUserClientProvider` |
| Local order book factory | `IPolymarketOrderBookFactory.CreatePrediction(tokenId)` |

## Authentication

| User intent | Polymarket.Net member |
|---|---|
| Public CLOB/Gamma/Data API read | `new PolymarketRestClient()` |
| Configure L1 EOA credentials | `new PolymarketCredentials().WithL1(SignType.EOA, new EthKey(privateKey: privateKey), polymarketFundingAddress)` |
| Configure L1 email credentials | `new PolymarketCredentials().WithL1(SignType.Email, new EthKey(privateKey: privateKey), polymarketFundingAddress)` |
| Configure L1 proxy credentials | `new PolymarketCredentials().WithL1(SignType.Proxy, new EthKey(privateKey: privateKey), polymarketFundingAddress)` |
| Configure L1 Poly1271 credentials | `new PolymarketCredentials().WithL1(SignType.Poly1271, new EthKey(privateKey: privateKey), polymarketFundingAddress)` |
| Configure L1 and L2 credentials | `new PolymarketCredentials().WithL1(...).WithL2(key, secret, passphrase)` |
| Create L2 API credentials | `client.PredictionApi.Account.CreateApiCredentialsAsync(nonce)` |
| Derive existing L2 API credentials | `client.PredictionApi.Account.GetApiCredentialsAsync(nonce)` |
| Get or create L2 API credentials | `client.PredictionApi.Account.GetOrCreateApiCredentialsAsync(nonce)` |
| List API keys | `client.PredictionApi.Account.GetApiKeysAsync()` |
| Delete API key | `client.PredictionApi.Account.DeleteApiKeyAsync()` |
| Update REST client with L2 credentials | `restClient.UpdateL2Credentials(polymarketCreds)` |
| Update socket client with L2 credentials | `socketClient.UpdateL2Credentials(polymarketCreds)` |

## Prediction Exchange Data REST

| User intent | Polymarket.Net member |
|---|---|
| Get server time | `client.PredictionApi.ExchangeData.GetServerTimeAsync()` |
| Get geographic restrictions | `client.PredictionApi.ExchangeData.GetGeographicRestrictionsAsync()` |
| Get markets | `client.PredictionApi.ExchangeData.GetMarketsAsync(closed: false, limit: 5)` |
| Get CLOB simplified markets | `client.PredictionApi.ExchangeData.GetSimplifiedMarketsAsync(cursor)` |
| Get sampling markets | `client.PredictionApi.ExchangeData.GetSamplingMarketsAsync(cursor)` |
| Get sampling simplified markets | `client.PredictionApi.ExchangeData.GetSamplingSimplifiedMarketsAsync(cursor)` |
| Get Gamma market by numeric id | `client.PredictionApi.ExchangeData.GetMarketByIdAsync(id)` |
| Get CLOB market info by condition id | `client.PredictionApi.ExchangeData.GetMarketInfoAsync(marketId)` |
| Get buy or sell price for token | `client.PredictionApi.ExchangeData.GetPriceAsync(tokenId, OrderSide.Buy)` |
| Get prices for multiple tokens/sides | `client.PredictionApi.ExchangeData.GetPricesAsync(new Dictionary<string, OrderSide> { ... })` |
| Get midpoint price | `client.PredictionApi.ExchangeData.GetMidpointPriceAsync(tokenId)` |
| Get midpoint prices | `client.PredictionApi.ExchangeData.GetMidpointPricesAsync(tokenIds)` |
| Get price history | `client.PredictionApi.ExchangeData.GetPriceHistoryAsync(market, startTime, endTime, interval, fidelity)` |
| Get bid/ask spread for token | `client.PredictionApi.ExchangeData.GetBidAskSpreadsAsync(tokenId)` |
| Get bid/ask spreads for tokens | `client.PredictionApi.ExchangeData.GetBidAskSpreadsAsync(tokenIds)` |
| Get order book | `client.PredictionApi.ExchangeData.GetOrderBookAsync(tokenId)` |
| Get multiple order books | `client.PredictionApi.ExchangeData.GetOrderBooksAsync(tokenIds)` |
| Get tick size | `client.PredictionApi.ExchangeData.GetTickSizeAsync(tokenId)` |
| Get negative risk | `client.PredictionApi.ExchangeData.GetNegativeRiskAsyncAsync(tokenId)` |
| Get fee rate in bps | `client.PredictionApi.ExchangeData.GetFeeRateBpsAsync(tokenId)` |
| Get last trade price | `client.PredictionApi.ExchangeData.GetLastTradePriceAsync(tokenId)` |
| Get last trade prices | `client.PredictionApi.ExchangeData.GetLastTradePricesAsync(tokenIds)` |
| Look up tokens | `client.PredictionApi.ExchangeData.LookupTokensAsync(tokenIds: tokenIds, marketIds: marketIds)` |
| Get market open interest | `client.PredictionApi.ExchangeData.GetOpenInterestAsync(marketIds)` |
| Get event volumes | `client.PredictionApi.ExchangeData.GetEventVolumeAsync(eventIds)` |

## CLOB Account REST

| User intent | Polymarket.Net member |
|---|---|
| Get closed-only mode | `client.PredictionApi.Account.GetClosedOnlyModeAsync()` |
| Get notifications | `client.PredictionApi.Account.GetNotificationsAsync()` |
| Drop notifications | `client.PredictionApi.Account.DropNotificationsAsync(ids)` |
| Get collateral balance/allowance | `client.PredictionApi.Account.GetBalanceAllowanceAsync(AssetType.Collateral)` |
| Get conditional token balance/allowance | `client.PredictionApi.Account.GetBalanceAllowanceAsync(AssetType.Conditional, tokenId)` |
| Update collateral allowance | `client.PredictionApi.Account.UpdateBalanceAllowanceAsync(AssetType.Collateral)` |
| Update conditional token allowance | `client.PredictionApi.Account.UpdateBalanceAllowanceAsync(AssetType.Conditional, tokenId)` |
| Get builder trades | `client.PredictionApi.Account.GetBuilderTradesAsync(builderCode, ...)` |
| Get rate limit tier | `client.PredictionApi.Account.GetRateLimitTierAsync()` |

## CLOB Trading REST

| User intent | Polymarket.Net member |
|---|---|
| Get open orders | `client.PredictionApi.Trading.GetOpenOrdersAsync(orderId, marketId, tokenId, cursor)` |
| Get order by id | `client.PredictionApi.Trading.GetOrderAsync(orderId)` |
| Get order reward scoring | `client.PredictionApi.Trading.GetOrderRewardScoringAsync(orderId)` |
| Get multiple orders reward scoring | `client.PredictionApi.Trading.GetOrdersRewardScoringAsync(orderIds)` |
| Place limit order | `client.PredictionApi.Trading.PlaceOrderAsync(tokenId, side, OrderType.Limit, quantity, price: price)` |
| Place market order | `client.PredictionApi.Trading.PlaceOrderAsync(tokenId, side, OrderType.Market, quantity, quantityType: quantityType)` |
| Place order with time in force | `client.PredictionApi.Trading.PlaceOrderAsync(..., timeInForce: TimeInForce.GoodTillCanceled)` |
| Place post-only order | `client.PredictionApi.Trading.PlaceOrderAsync(..., postOnly: true)` |
| Place limit order on a V2 market | `client.PredictionApi.Trading.PlaceOrderAsync(tokenId: positionId, side: side, orderType: OrderType.Limit, quantity: quantity, price: price, marketVersion: 2)` |
| Place multiple orders | `client.PredictionApi.Trading.PlaceMultipleOrdersAsync(requests)` |
| Cancel order | `client.PredictionApi.Trading.CancelOrderAsync(orderId)` |
| Cancel multiple orders | `client.PredictionApi.Trading.CancelOrdersAsync(orderIds)` |
| Cancel orders on market/token | `client.PredictionApi.Trading.CancelOrdersOnMarketAsync(marketId, tokenId)` |
| Cancel all open orders | `client.PredictionApi.Trading.CancelAllOrdersAsync()` |
| Get user trades | `client.PredictionApi.Trading.GetUserTradesAsync(tradeId, makerAddress, marketId, tokenId, startTime, endTime, cursor)` |
| Send order heartbeat | `client.PredictionApi.Trading.PostOrderHeartbeatAsync()` |

## Prediction Gamma REST

| User intent | Polymarket.Net member |
|---|---|
| Get sports teams | `client.PredictionApi.ExchangeData.GetSportTeamsAsync(...)` |
| Get sports metadata | `client.PredictionApi.ExchangeData.GetSportsAsync()` |
| Get sport market types | `client.PredictionApi.ExchangeData.GetSportMarketTypesAsync()` |
| Get tags | `client.PredictionApi.ExchangeData.GetTagsAsync(...)` |
| Get tag by id | `client.PredictionApi.ExchangeData.GetTagByIdAsync(id)` |
| Get tag by slug | `client.PredictionApi.ExchangeData.GetTagBySlugAsync(slug)` |
| Get related tags by id | `client.PredictionApi.ExchangeData.GetRelatedTagsByIdAsync(id)` |
| Get related tags by slug | `client.PredictionApi.ExchangeData.GetRelatedTagsBySlugAsync(slug)` |
| Get tags related to tag by id | `client.PredictionApi.ExchangeData.GetTagsRelatedToTagByIdAsync(id)` |
| Get tags related to tag by slug | `client.PredictionApi.ExchangeData.GetTagsRelatedToTagBySlugAsync(slug)` |
| Get events | `client.PredictionApi.ExchangeData.GetEventsAsync(...)` |
| Get keyset-paginated events | `client.PredictionApi.ExchangeData.GetEventsKeysetPaginationAsync(...)` |
| Get event by id | `client.PredictionApi.ExchangeData.GetEventByIdAsync(id)` |
| Get event by slug | `client.PredictionApi.ExchangeData.GetEventBySlugAsync(slug)` |
| Get event tags | `client.PredictionApi.ExchangeData.GetEventTagsAsync(id)` |
| Get Gamma markets | `client.PredictionApi.ExchangeData.GetMarketsAsync(...)` |
| Get keyset-paginated markets | `client.PredictionApi.ExchangeData.GetMarketsKeysetPaginationAsync(...)` |
| Get Gamma market by id | `client.PredictionApi.ExchangeData.GetMarketByIdAsync(id)` |
| Get Gamma market by slug | `client.PredictionApi.ExchangeData.GetMarketBySlugAsync(slug)` |
| Get market tags | `client.PredictionApi.ExchangeData.GetMarketTagsAsync(id)` |
| Get series | `client.PredictionApi.ExchangeData.GetSeriesAsync(...)` |
| Get series by id | `client.PredictionApi.ExchangeData.GetSeriesByIdAsync(id)` |
| Search markets/events/profiles | `client.PredictionApi.ExchangeData.SearchAsync(query, ...)` |

## Prediction Data REST

| User intent | Polymarket.Net member |
|---|---|
| Get current positions for user | `client.PredictionApi.Trading.GetPositionsAsync(user)` |

## WebSocket

| User intent | Polymarket.Net member |
|---|---|
| Subscribe platform new market/resolution updates | `socketClient.PredictionApi.SubscribeToPlatformUpdatesAsync(onNewMarketUpdate, onMarketResolvedUpdate)` |
| Subscribe token price/book/trade/tick/best bid-ask updates | `socketClient.PredictionApi.SubscribeToTokenUpdatesAsync(tokenIds, onPriceChangeUpdate, onBookUpdate, onLastTradePriceUpdate, onTickSizeUpdate, onBestBidAskUpdate)` |
| Subscribe authenticated user order/trade updates | `socketClient.PredictionApi.SubscribeToUserUpdatesAsync(onOrderUpdate, onTradeUpdate)` |
| Subscribe sports updates | `socketClient.PredictionApi.SubscribeToSportsUpdatesAsync(onSportsUpdate)` |
| Subscribe crypto prices | `socketClient.PredictionApi.SubscribeToCryptoPriceUpdatesAsync(symbol, onSnapshot, onUpdate)` |
| Subscribe crypto TWAP | `socketClient.PredictionApi.SubscribeToCryptoTwapPriceUpdatesAsync(symbol, onSnapshot, onUpdate)` |
| Subscribe equity prices | `socketClient.PredictionApi.SubscribeToEquityPriceUpdatesAsync(symbol, onSnapshot, onUpdate)` |
| Subscribe equity TWAP | `socketClient.PredictionApi.SubscribeToEquityTwapPriceUpdatesAsync(symbol, onSnapshot, onUpdate)` |
| Unsubscribe from socket stream | `socketClient.UnsubscribeAsync(subscription.Data)` |

WebSocket subscription methods return `WebSocketResult<UpdateSubscription>`.

## Local Order Book And User Client Provider

| User intent | Polymarket.Net member |
|---|---|
| Create local CLOB order book directly | `new PolymarketPredictionSymbolOrderBook(tokenId)` |
| Create local CLOB order book from DI | `orderBookFactory.CreatePrediction(tokenId)` |
| Create local Perps book | `new PolymarketPerpsSymbolOrderBook(symbolId)` or `orderBookFactory.CreatePerps(symbolId)` |
| Register DI services | `services.AddPolymarket(options => { ... })` |
| Initialize per-user clients | `userClientProvider.InitializeUserClient(userIdentifier, credentials, environment)` |
| Get per-user REST client | `userClientProvider.GetRestClient(userIdentifier, credentials, environment)` |
| Get per-user socket client | `userClientProvider.GetSocketClient(userIdentifier, credentials, environment)` |

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | REST methods return `HttpResult<T>` or `HttpResult`; use `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | Socket subscriptions return `WebSocketResult<UpdateSubscription>`; use `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read result data | Read `result.Data` or `sub.Data` only after `.Success` |
| Prediction order accepted check | Check `order.Data.Success` after `order.Success` |
| Perps single-order result | Check outer `.Success` / `.Error`; payload has `Status`, `Error`, numeric `OrderId` |
| Perps socket operations | `QueryResult<T>` or `QueryResult`; check `.Success` |
| Cancellation | Pass `ct: cancellationToken` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| `PolymarketClient` | `PolymarketRestClient` or `PolymarketSocketClient` |
| `ApiCredentials` directly | `PolymarketCredentials` |
| `SpotApi`, `FuturesApi`, `GeneralApi` | `PredictionApi` or `PerpsApi` |
| Symbol pairs such as `BTCUSDT` for concrete orders | Prediction token/position ids or Perps numeric instrument ids |
| `PredictionApi.SharedApi` | Shared contracts are available through `PerpsApi.SharedApi` on REST and sockets |
| `.Data` without `.Success` check | Check `.Success` first |
| L1-only credentials for order placement | Add or derive L2 credentials |
| `PolymarketOrderResult.Success` only | Check `HttpResult.Success` first, then order result success |
| Manual socket lifecycle without storing subscription | Store `UpdateSubscription` and unsubscribe |

## Perps REST

These concrete methods use numeric instrument ids, not prediction token ids.
Private reads use the registered proxy key and secret; trading signs with the proxy
private key. Owner signer keys are used for proxy registration.

| User intent | Polymarket.Net member |
|---|---|
| Discover instruments | `client.PerpsApi.ExchangeData.GetSymbolsAsync(symbolType: InstrumentType.Perpetual)` |
| Resolve instrument name/id after discovery | `client.PerpsApi.GetSymbolIdByName(name)` / `GetSymbolNameById(id)` |
| Server time / exchange info | `client.PerpsApi.ExchangeData.GetServerTimeAsync()` / `GetExchangeInfoAsync()` |
| Collateral assets | `client.PerpsApi.ExchangeData.GetCollateralAssetsAsync()` |
| Tickers / statistics | `client.PerpsApi.ExchangeData.GetTickersAsync(symbolId)` / `GetStatisticsAsync(symbolId)` |
| Best bid/ask | `client.PerpsApi.ExchangeData.GetBookTickerAsync(symbolId)` |
| Order book | `client.PerpsApi.ExchangeData.GetOrderBookAsync(symbolId, depth: 10)` |
| Candles | `client.PerpsApi.ExchangeData.GetKlinesAsync(symbolId, KlineInterval.OneHour, startTime)` |
| Mark price history | `client.PerpsApi.ExchangeData.GetMarkPriceHistoryAsync(symbolId, interval, startTime)` |
| Funding history | `client.PerpsApi.ExchangeData.GetFundingRateHistoryAsync(symbolId, startTime: startTime)` |
| Recent trades | `client.PerpsApi.ExchangeData.GetRecentTradesAsync(symbolId)` |
| Index | `client.PerpsApi.ExchangeData.GetIndexAsync(asset)` |
| Fee schedule / limit tiers | `client.PerpsApi.ExchangeData.GetFeesAsync()` / `GetLimitTiersAsync()` |
| Public portfolio | `client.PerpsApi.ExchangeData.GetPortfolioAsync(address)` |
| Registration status | `client.PerpsApi.ExchangeData.GetIsRegisteredAsync(address)` |
| Register proxy with explicit owner key | `client.PerpsApi.Account.CreateProxyAsync(signingKey, proxyAddress, expireTime)` |
| Delete proxy | `client.PerpsApi.Account.DeleteProxyAsync(proxyAddress)` |
| Get credentials | `client.PerpsApi.Account.GetCredentialsAsync()` |
| Balances / portfolio | `client.PerpsApi.Account.GetBalancesAsync()` / `GetPortfolioAsync()` |
| Leverage config | `client.PerpsApi.Account.GetLeverageMarginConfigAsync(symbolId)` |
| Set leverage | `client.PerpsApi.Account.SetLeverageAsync(...)` |
| Update isolated margin | `client.PerpsApi.Account.UpdateIsolatedMarginAsync(...)` |
| Equity / PnL / funding history | `client.PerpsApi.Account.GetEquityHistoryAsync(...)` / `GetPnlHistoryAsync(...)` / `GetFundingHistoryAsync(...)` |
| Deposits / withdrawals / internal transfers | `client.PerpsApi.Account.GetDepositsAsync(...)` / `GetWithdrawalsAsync(...)` / `GetInternalTransfersAsync(...)` |
| Withdraw / transfer | `client.PerpsApi.Account.WithdrawAsync(...)` / `TransferInternalAsync(...)` |
| Account stats / limits | `client.PerpsApi.Account.GetAccountStatsAsync()` / `GetAccountLimitsAsync()` |
| Place limit order | `client.PerpsApi.Trading.PlaceOrderAsync(instrumentId, side, quantity, price: price, timeInForce: PerpTimeInForce.GoodTillCanceled)` |
| Place batch | `client.PerpsApi.Trading.PlaceMultipleOrdersAsync(orders)` |
| Edit order | `client.PerpsApi.Trading.EditOrderAsync(orderId, newPrice, newQuantity)` |
| Cancel order | `client.PerpsApi.Trading.CancelOrderAsync(orderId)` |
| Cancel all orders for an instrument | `client.PerpsApi.Trading.CancelAllOrdersAsync(symbolId)` |
| Order history / open orders | `client.PerpsApi.Trading.GetOrdersAsync(symbolId: symbolId)` / `GetOpenOrdersAsync(symbolId)` |
| User fills | `client.PerpsApi.Trading.GetUserTradesAsync(symbolId: symbolId)` |
| Shared REST contracts | `client.PerpsApi.SharedApi` |

`GetSymbolsAsync` returns instrument precision and minimum notional. Historical
Perps endpoints often return `PolymarketPaged<T>` (`Data`, `More`, `Cursor`);
inspect each signature for its time/cursor pagination parameters. Single Perps
order rejections are outer failures. Batch placement contains per-order
`CallResult<PolymarketPerpOrderResult>` entries that must also be checked.

## Perps WebSocket

| User intent | Polymarket.Net member |
|---|---|
| Trades | `socketClient.PerpsApi.ExchangeData.SubscribeToTradeUpdatesAsync(symbolId, onUpdate)` |
| Ticker / all tickers | `socketClient.PerpsApi.ExchangeData.SubscribeToTickerUpdatesAsync(symbolId, onUpdate)` / `SubscribeToAllTickerUpdatesAsync(onUpdate)` |
| Best bid/ask | `socketClient.PerpsApi.ExchangeData.SubscribeToBookTickerUpdatesAsync(symbolId, onUpdate)` |
| Order book | `socketClient.PerpsApi.ExchangeData.SubscribeToOrderBookUpdatesAsync(symbolId, onUpdate)` |
| Candles | `socketClient.PerpsApi.ExchangeData.SubscribeToKlineUpdatesAsync(symbolId, interval, onUpdate)` |
| Statistics | `socketClient.PerpsApi.ExchangeData.SubscribeToStatisticsUpdatesAsync(symbolId, onUpdate)` |
| User orders / fills / TP-SL | `socketClient.PerpsApi.Trading.SubscribeToOrderUpdatesAsync(onUpdate)` / `SubscribeToUserTradeUpdatesAsync(onUpdate)` / `SubscribeToTpSlUpdatesAsync(onUpdate)` |
| Balances / portfolio | `socketClient.PerpsApi.Account.SubscribeToBalanceUpdatesAsync(onUpdate)` / `SubscribeToPortfolioUpdatesAsync(onUpdate)` |
| Funding / deposits / withdrawals | `socketClient.PerpsApi.Account.SubscribeToUserFundingUpdatesAsync(onUpdate)` / `SubscribeToDepositUpdatesAsync(onUpdate)` / `SubscribeToWithdrawalUpdatesAsync(onUpdate)` |
| Place / cancel orders | `socketClient.PerpsApi.Trading.PlaceOrderAsync(...)` / `CancelOrderAsync(orderId)` |
| Leverage / margin operations | `socketClient.PerpsApi.Account.SetLeverageAsync(...)` / `UpdateIsolatedMarginAsync(...)` |
| Shared socket contracts | `socketClient.PerpsApi.SharedApi` |

Subscriptions return `WebSocketResult<UpdateSubscription>`; socket operations return
`QueryResult<T>` / `QueryResult`. Always check `.Success` and clean up successful
subscriptions with `socketClient.UnsubscribeAsync(subscription.Data)`.
