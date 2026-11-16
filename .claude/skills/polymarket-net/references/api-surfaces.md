# Polymarket.Net API surfaces

Use this reference for endpoint selection. Examples omit optional parameters;
inspect the interface for exact filters, callbacks, pagination and cancellation.

## Prediction REST

`client.PredictionApi.ExchangeData` combines CLOB data and Gamma metadata:

- Discovery: `GetMarketsAsync(closed: false, limit: 5)`, `GetMarketsKeysetPaginationAsync(...)`, `GetEventsAsync(...)`, `GetEventsKeysetPaginationAsync(...)`, `SearchAsync(query, ...)`.
- Market details: `GetMarketByIdAsync(longId)`, `GetMarketBySlugAsync(slug)`, `GetMarketByTokenAsync(tokenId)`, `GetMarketInfoAsync(conditionId)`.
- Event details: `GetEventByIdAsync(id)`, `GetEventBySlugAsync(slug)`, `GetEventTagsAsync(id)`.
- CLOB pages: `GetSimplifiedMarketsAsync(cursor)`, `GetSamplingMarketsAsync(cursor)`, `GetSamplingSimplifiedMarketsAsync(cursor)`. These are distinct from Gamma market arrays.
- Prices: `GetPriceAsync(tokenId, side)`, `GetPricesAsync(tokens)`, `GetMidpointPriceAsync(tokenId)`, `GetMidpointPricesAsync(tokenIds)`, `GetPriceHistoryAsync(...)`, `GetPriceHistoriesAsync(...)`.
- Books/spreads: `GetOrderBookAsync(tokenId)`, `GetOrderBooksAsync(tokenIds)`, `GetBidAskSpreadsAsync(tokenId)` or the token-id collection overload.
- Token metadata: `GetTickSizeAsync(tokenId)`, `GetNegativeRiskAsyncAsync(tokenId)` (current spelling), `GetFeeRateBpsAsync(tokenId)`, `LookupTokensAsync(tokenIds: tokenIds, marketIds: marketIds)`.
- Trades: `GetLastTradePriceAsync(tokenId)`, `GetLastTradePricesAsync(tokenIds)`.
- Interest/resolution: `GetOpenInterestAsync(marketIds)`, `GetEventVolumeAsync(eventIds)`, `GetResolutionStateAsync(...)`.
- Tags/series/sports: `GetTagsAsync(...)`, `GetTagByIdAsync(id)`, `GetTagBySlugAsync(slug)`, related-tag methods, `GetMarketTagsAsync(id)`, `GetSeriesAsync(...)`, `GetSeriesByIdAsync(id)`, `GetSportsAsync()`, `GetSportTeamsAsync(...)`, `GetSportMarketTypesAsync()`.
- System: `GetServerTimeAsync()`, `GetGeographicRestrictionsAsync()`.

`GetMarketsAsync` returns `PolymarketGammaMarket[]`. Gamma market `Id` is `long`;
`MarketId` is the condition id. Keyset market results expose `Markets` and
`NextPageCursor`. V1 markets expose `ClobTokenIds`; V2 exposes `PositionIds`.

`client.PredictionApi.Account`:

- `CreateApiCredentialsAsync(nonce)`, `GetApiCredentialsAsync(nonce)`, `GetOrCreateApiCredentialsAsync(nonce)`.
- `GetApiKeysAsync()`, `DeleteApiKeyAsync()`.
- `GetBalanceAllowanceAsync(assetType, tokenId)`, `UpdateBalanceAllowanceAsync(assetType, tokenId)`.
- Use `AssetType.Collateral`, `AssetType.Conditional` or `AssetType.ConditionalV2` as appropriate.
- `GetClosedOnlyModeAsync()`, `GetNotificationsAsync()`, `DropNotificationsAsync(ids)`.
- `GetBuilderTradesAsync(...)`, `GetRateLimitTierAsync()`.

`client.PredictionApi.Trading`:

- `PlaceOrderAsync(tokenId, side, orderType, quantity, price: price, marketVersion: 1)`; use `marketVersion: 2` with V2 position ids.
- `PlaceMultipleOrdersAsync(requests)`; each `PolymarketOrderRequest` has `MarketVersion`.
- `GetOpenOrdersAsync(...)`, `GetOrderAsync(orderId)`, `GetUserTradesAsync(...)`.
- `GetOrderRewardScoringAsync(orderId)`, `GetOrdersRewardScoringAsync(orderIds)`.
- `CancelOrderAsync(orderId)`, `CancelOrdersAsync(orderIds)`, `CancelOrdersOnMarketAsync(...)`, `CancelAllOrdersAsync()`.
- `PostOrderHeartbeatAsync()`.
- `GetPositionsAsync(user: address, status: PositionStatus.Closed, cursor: cursor, ...)` for Data API v2 positions, including closed positions.

Position results are `PolymarketDataPage<PolymarketPosition>`. Check outer success,
read positions from `.Data.Data`, and paginate with `.Data.Pagination.NextCursor`.
Model changes include `Asset` â†’ `TokenId`, `Size` â†’ `CurrentSize`, `CurPrice` â†’
`CurrentPrice`, nullable `DateTime` for `EndDate`, and revised cost/PnL fields.

## Prediction streams

Use `socket.PredictionApi` directly:

- `SubscribeToTokenUpdatesAsync(tokenIds, onPriceChangeUpdate: ..., onBookUpdate: ..., onLastTradePriceUpdate: ..., onTickSizeUpdate: ..., onBestBidAskUpdate: ...)`.
- `SubscribeToPlatformUpdatesAsync(onNewMarketUpdate: ..., onMarketResolvedUpdate: ...)`.
- `SubscribeToSportsUpdatesAsync(onSportsUpdate)`.
- `SubscribeToUserUpdatesAsync(onOrderUpdate: ..., onTradeUpdate: ...)` with prediction L2 credentials.
- `SubscribeToCryptoPriceUpdatesAsync(symbol, onSnapshot, onUpdate)`.
- `SubscribeToCryptoTwapPriceUpdatesAsync(symbol, onSnapshot, onUpdate)`.
- `SubscribeToEquityPriceUpdatesAsync(symbol, onSnapshot, onUpdate)`.
- `SubscribeToEquityTwapPriceUpdatesAsync(symbol, onSnapshot, onUpdate)`.

Prediction order updates use `update.Data.Id`. Snapshot callbacks for price/TWAP
streams differ from incremental callbacks; inspect the interface before assigning
handler types. Store each successful subscription and unsubscribe on shutdown.

## Perps REST

`client.PerpsApi.ExchangeData`:

- Instruments/system: `GetSymbolsAsync(...)`, `GetServerTimeAsync()`, `GetExchangeInfoAsync()`, `GetCollateralAssetsAsync()`.
- Prices/books: `GetTickersAsync(symbolId)`, `GetStatisticsAsync(symbolId)`, `GetBookTickerAsync(symbolId)`, `GetOrderBookAsync(symbolId, depth: 10)`; supported depths are 10, 100, 500 and 1000.
- History: `GetKlinesAsync(symbolId, interval, startTime)`, `GetMarkPriceHistoryAsync(...)`, `GetRecentTradesAsync(symbolId)`, `GetFundingRateHistoryAsync(symbolId)`.
- Other public data: `GetIndexAsync(asset)`, `GetFeesAsync()`, `GetLimitTiersAsync()`, `GetPortfolioAsync(address)`, `GetIsRegisteredAsync(address)`, `GetExchangeStatisticsAsync(startTime, endTime)`.

Concrete calls use numeric instrument ids. After `GetSymbolsAsync()` populates the
cache, `client.PerpsApi.GetSymbolIdByName(name)` and `GetSymbolNameById(id)` resolve
names/ids. History results often use `PolymarketPaged<T>` (`Data`, `More`, `Cursor`),
but not every endpoint accepts a cursor. Use each method's actual time/cursor filters.

`client.PerpsApi.Account`:

- Proxy lifecycle: `CreateProxyAsync(expireTime)` using configured owner/proxy keys, `CreateProxyAsync(signingKey, proxyAddress, expireTime)` with an explicit owner key, `DeleteProxyAsync(proxyAddress)`, `GetCredentialsAsync()`.
- Balances/portfolio: `GetBalancesAsync()`, `GetPortfolioAsync()`, `GetEquityHistoryAsync(...)`, `GetPnlHistoryAsync(...)`, `GetFundingHistoryAsync(...)`.
- Risk: `GetLeverageMarginConfigAsync(symbolId)`, `SetLeverageAsync(symbolId, leverage, cross)`, `UpdateIsolatedMarginAsync(...)`, `GetAccountStatsAsync()`, `GetAccountLimitsAsync()`.
- Transfers/history: `WithdrawAsync(...)`, `TransferInternalAsync(...)`, `GetDepositsAsync(...)`, `GetWithdrawalsAsync(...)`, `GetInternalTransfersAsync(...)`.
- Builders: `ApproveBuilderAsync(...)`, `GetBuilderApprovalsAsync(builder)`.

`client.PerpsApi.Trading`:

- `PlaceOrderAsync(instrumentId, side, quantity, price: price, timeInForce: PerpTimeInForce.GoodTillCanceled)`.
- `PlaceMultipleOrdersAsync(orders)`; request type `PolymarketPerpsOrderRequest`.
- `EditOrderAsync(orderId, newPrice, newQuantity)`, `EditOrdersAsync(orders)`, and client-order-id edit variants.
- `CancelOrderAsync(orderId)`, `CancelOrdersAsync(orderIds)`, client-order-id cancellation variants, `CancelAllOrdersAsync(symbolId)`.
- `GetOrdersAsync(...)`, `GetOpenOrdersAsync(symbolId)`, `GetUserTradesAsync(...)`.

Perps order ids are numeric. Single-order rejections become outer failures. Batch
placement returns per-order `CallResult<PolymarketPerpOrderResult>` entries; check
each entry. The payload has `Status`, `Error`, `OrderId`, not prediction `Success`.

## Perps streams and operations

`socket.PerpsApi.ExchangeData` public streams include:

- `SubscribeToTradeUpdatesAsync`, `SubscribeToTickerUpdatesAsync`, `SubscribeToAllTickerUpdatesAsync`.
- `SubscribeToBookTickerUpdatesAsync`, `SubscribeToOrderBookUpdatesAsync`, `SubscribeToKlineUpdatesAsync`.
- `SubscribeToStatisticsUpdatesAsync`, `SubscribeToAllStatisticsUpdatesAsync`.

`socket.PerpsApi.Trading` provides private `SubscribeToOrderUpdatesAsync`,
`SubscribeToUserTradeUpdatesAsync` and `SubscribeToTpSlUpdatesAsync`, plus order
placement, cancellation and editing operations. `socket.PerpsApi.Account` provides
balance, portfolio, funding, deposit and withdrawal streams, plus leverage/margin
operations. Use registered proxy credentials for private access.

REST returns `HttpResult<T>` / `HttpResult`; subscriptions return
`WebSocketResult<UpdateSubscription>`; Perps socket operations return
`QueryResult<T>` / `QueryResult`. Always inspect success before data.

## Additional native routes

| Intent | Method |
|---|---|
| Get Related Tags By Id | `client.PredictionApi.ExchangeData.GetRelatedTagsByIdAsync(...)` |
| Get Related Tags By Slug | `client.PredictionApi.ExchangeData.GetRelatedTagsBySlugAsync(...)` |
| Get Tags Related To Tag By Id | `client.PredictionApi.ExchangeData.GetTagsRelatedToTagByIdAsync(...)` |
| Get Tags Related To Tag By Slug | `client.PredictionApi.ExchangeData.GetTagsRelatedToTagBySlugAsync(...)` |
| Subscribe To Balance Updates | `socketClient.PerpsApi.Account.SubscribeToBalanceUpdatesAsync(...)` |
| Subscribe To User Funding Updates | `socketClient.PerpsApi.Account.SubscribeToUserFundingUpdatesAsync(...)` |

## Shared API V2

Prediction has no Shared API implementation. `IPolymarketSharedApiClient` exposes these Perps properties; native Perps REST/socket surfaces expose `.SharedApi`.

- `PerpsRest` (`IPolymarketRestClientPerpsSharedApi`): `IGetBalancesRest`, `IGetDepositHistoryRest`, `IGetFeesRest`, `IGetFundingRateHistoryRest`, `IGetFundingInfoRest`, `IGetUserFundingHistoryRest`, `IPlaceFuturesOrderRest`, `IGetFuturesOrderRest`, `IGetFuturesOrderByClientOrderIdRest`, `IGetOpenFuturesOrdersRest`, `IGetClosedFuturesOrdersRest`, `ICancelFuturesOrderRest`, `ICancelFuturesOrderByClientOrderIdRest`, `ICancelAllFuturesSymbolOrdersRest`, `IEditFuturesOrderRest`, `IEditFuturesOrderByClientOrderIdRest`, `IGetFuturesUserTradeHistoryRest`, `IPlaceMultipleFuturesOrdersRest`, `IGetPositionsRest`, `IGetFuturesSymbolsRest`, `IGetAllTickersRest`, `IGetTickerRest`, `IGetIndexPriceRest`, `IGetAllIndexPricesRest`, `IGetMarkPriceRest`, `IGetAllMarkPricesRest`, `IGetOpenInterestRest`, `IGetKlinesRest`, `IGetLeverageRest`, `ISetLeverageRest`, `IGetLeverageTiersRest`, `IGetOrderBookRest`, `IGetBookTickerRest`, `IGetRecentTradesRest`, `IGetWithdrawalHistoryRest`.
- `PerpsSocket` (`IPolymarketSocketClientPerpsSharedApi`): `ISubscribeTradesSocket`, `ISubscribeBookTickerSocket`, `ISubscribeOrderBookSocket`, `ISubscribeKlinesSocket`, `ISubscribeTickerSocket`, `ISubscribeAllTickersSocket`, `ISubscribeBalancesSocket`, `ISubscribeUserTradesSocket`, `ISubscribeFuturesOrdersSocket`, `ISubscribePositionsSocket`, `IPlaceFuturesOrderSocket`, `ICancelFuturesOrderSocket`, `ICancelFuturesOrderByClientOrderIdSocket`, `ICancelAllFuturesSymbolOrdersSocket`, `IEditFuturesOrderSocket`, `IEditFuturesOrderByClientOrderIdSocket`, `IPlaceMultipleFuturesOrdersSocket`.

For known workflows use aggregate properties or inject narrow capabilities. For dynamic selection call `aggregate.GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.PerpetualLinear)` and check for null before using `Capability` and `Options`. Descriptors identify operations, not guarantees of support. `GetCapabilities` enumerates matches and `Discover` summarizes metadata. With several exchanges registered, inject `IEnumerable<TCapability>` and select by exchange, mode and options.

## Catalogs, quantities and factories

Call `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest())`, check success, then inspect `FuturesSymbolCatalog`. Requests honor base/quote asset type/subtype filters. Returned metadata includes display names, price/quantity precision, minimum notional, leverage and asset classifications where populated. Shared Perps uses `TradingMode.PerpetualLinear`; use returned base/quote metadata instead of guessing a quote asset or instrument name.

Native Perps and Shared order quantities are base-asset amounts. Shared sizing uses `SharedOrderQuantity.QuantityInBaseAsset`, and books identify base-asset quantities. Contract size metadata is 1; do not substitute integer contract counts. Prediction sizing follows token shares/order-specific market sizing rather than a currency-pair model.

`services.AddPolymarket(...)` registers clients, shared capabilities, book/tracker factories and user clients. `IPolymarketOrderBookFactory.CreatePrediction(tokenId)` and `CreatePerps(symbolId)` create distinct books; check start results and stop them in cleanup. `IPolymarketTrackerFactory.CreateUserPerpsDataTracker(...)` uses configured Perps credentials. `IPolymarketUserClientProvider` caches per-user clients; clear stale clients when credentials change.

Prediction and Perps options are under `PredictionOptions` / `PerpsOptions`; DI adds `Rest` / `Socket` transport levels. Prediction REST builder configuration uses `BuilderCode`; Perps has `BuilderCode` and `BuilderFeePercentage` on REST/socket options. Configure these explicitly for the intended fee policy. Use `PolymarketEnvironment.Live`, `Testnet` or explicit custom addresses according to the requested workflow; do not infer service availability merely from an environment name.

## Checkout-only sources

- [Public interfaces](../../../../Polymarket.Net/Interfaces/Clients/): exact signatures, pagination and credential prerequisites (checkout-only).
- [Implementations](../../../../Polymarket.Net/Clients/): Shared options, conversions and signing behavior (checkout-only).
- [Complete examples](../../../../Examples/ai-friendly/): independent Prediction and Perps programs (checkout-only).
