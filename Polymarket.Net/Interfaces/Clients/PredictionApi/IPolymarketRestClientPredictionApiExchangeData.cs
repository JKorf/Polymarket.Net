using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.PredictionApi
{
    /// <summary>
    /// Polymarket Prediction API exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPolymarketRestClientPredictionApiExchangeData
    {
        /// <summary>
        /// Get server time
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/data/get-server-time" /><br />
        /// Endpoint:<br />
        /// GET /time
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get geographical restrictions for calling client
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/geoblock" /><br />
        /// Endpoint:<br />
        /// GET /api/geoblock
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get sampling simplified markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-sampling-simplified-markets" /><br />
        /// Endpoint:<br />
        /// GET /sampling-simplified-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get sampling markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-sampling-markets" /><br />
        /// Endpoint:<br />
        /// GET /sampling-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get simplified markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-simplified-markets" /><br />
        /// Endpoint:<br />
        /// GET /simplified-markets
        /// </para>
        /// </summary>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-market-price" /><br />
        /// Endpoint:<br />
        /// GET /price
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] Token id</param>
        /// <param name="side">["<c>side</c>"] Side</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default);

        /// <summary>
        /// Get buy/sell prices for all markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-market-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /prices
        /// </para>
        /// </summary>
        /// <param name="tokens">Tokens to retrieve prices for</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokens, CancellationToken ct = default);

        /// <summary>
        /// Get the midpoint price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/data/get-midpoint-price" /><br />
        /// Endpoint:<br />
        /// GET /midpoint
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get mid point prices for tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-midpoint-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /midpoints
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] Tokens to request</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get price history for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-prices-history" /><br />
        /// Endpoint:<br />
        /// GET /prices-history
        /// </para>
        /// </summary>
        /// <param name="market">["<c>market</c>"] The market</param>
        /// <param name="startTime">["<c>startTs</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTs</c>"] Filter by end time</param>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="fidelity">["<c>fidelity</c>"] Fidelity in minutes</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default);

        /// <summary>
        /// Get price history for multiple markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-prices-history" /><br />
        /// Endpoint:<br />
        /// GET /prices-history
        /// </para>
        /// </summary>
        /// <param name="markets">["<c>markets</c>"] The markets to get history for</param>
        /// <param name="startTime">["<c>startTs</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTs</c>"] Filter by end time</param>
        /// <param name="interval">["<c>interval</c>"] Interval</param>
        /// <param name="fidelity">["<c>fidelity</c>"] Fidelity in minutes</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketPriceHistories>> GetPriceHistoriesAsync(IEnumerable<string> markets, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for a a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-spread" /><br />
        /// Endpoint:<br />
        /// GET /spread
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get bid/ask spread for specified token ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-spreads" /><br />
        /// Endpoint:<br />
        /// POST /spreads
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] Token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-order-book" /><br />
        /// Endpoint:<br />
        /// GET /book
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get order book info for multiple tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-order-books-request-body" /><br />
        /// Endpoint:<br />
        /// POST /books
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get tick size for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-tick-size" /><br />
        /// Endpoint:<br />
        /// GET /tick-size
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get negative risk for a token
        /// <para>
        /// Endpoint:<br />
        /// GET /neg-risk
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get fee rate in basis points
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-fee-rate" /><br />
        /// Endpoint:<br />
        /// GET /fee-rate
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for a token
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-last-trade-price" /><br />
        /// Endpoint:<br />
        /// GET /last-trade-price
        /// </para>
        /// </summary>
        /// <param name="tokenId">["<c>token_id</c>"] The token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get last trade price for tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/market-data/get-last-trade-prices-request-body" /><br />
        /// Endpoint:<br />
        /// POST /last-trades-prices
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>token_id</c>"] The token ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default);

        /// <summary>
        /// Get info on a market
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-clob-market-info" /><br />
        /// Endpoint:<br />
        /// GET /clob-markets/{tokenId}<br />
        /// </para>
        /// </summary>
        /// <param name="marketId">["<c>condition_id</c>"] Id of the market</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketMarketInfo>> GetMarketInfoAsync(string marketId, CancellationToken ct = default);

        /// <summary>
        /// Get list of all sports teams
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/sports/list-teams" /><br />
        /// Endpoint:<br />
        /// GET /teams
        /// </para>
        /// </summary>
        /// <param name="league">["<c>league</c>"] Filter by league name</param>
        /// <param name="name">["<c>name</c>"] Filter by name</param>
        /// <param name="abbreviation">["<c>abbreviation</c>"] Filter by abbreviation</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open interest
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-open-interest" /><br />
        /// Endpoint:<br />
        /// GET /v2/oi
        /// </para>
        /// </summary>
        /// <param name="marketIds">["<c>condition</c>"] Filter by market IDs</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketOpenInterest[]>> GetOpenInterestAsync(IEnumerable<string>? marketIds = null, CancellationToken ct = default);

        /// <summary>
        /// Get event volumes
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-live-volume-for-an-event" /><br />
        /// Endpoint:<br />
        /// GET /v2/live-volume
        /// </para>
        /// </summary>
        /// <param name="eventIds">["<c>event_id</c>"] Filter by event IDs</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketEventVolume>> GetEventVolumeAsync(IEnumerable<string> eventIds, CancellationToken ct = default);

        /// <summary>
        /// Look up tokens
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/look-up-tokens" /><br />
        /// Endpoint:<br />
        /// GET /v2/tokens
        /// </para>
        /// </summary>
        /// <param name="tokenIds">["<c>tokenId</c>"] Filter by token IDs</param>
        /// <param name="marketIds">["<c>condition</c>"] Filter by market IDs</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<PolymarketTokenInfo[]>> LookupTokensAsync(
            IEnumerable<string>? tokenIds = null,
            IEnumerable<string>? marketIds = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get sports meta data
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/sports/get-sports-metadata-information" /><br />
        /// Endpoint:<br />
        /// GET /sports
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get valid sport market types
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/sports/get-valid-sports-market-types" /><br />
        /// Endpoint:<br />
        /// GET /sports/market-types
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get tags
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/list-tags" /><br />
        /// Endpoint:<br />
        /// GET /tags
        /// </para>
        /// </summary>
        /// <param name="includeTemplate">["<c>includeTemplate</c>"] Include template</param>
        /// <param name="isCarousel">["<c>isCarousel</c>"] Filter by carousel</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag[]>> GetTagsAsync(
            bool? includeTemplate = null,
            bool? isCarousel = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get tag by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-tag-by-id" /><br />
        /// Endpoint:<br />
        /// GET /tags/{id}
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeTemplate">["<c>includeTemplate</c>"] Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get tag by slug
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-tag-by-id" /><br />
        /// Endpoint:<br />
        /// GET /tags/slug/{slug}
        /// </para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeTemplate">["<c>includeTemplate</c>"] Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get related tags for a tag
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-id" /><br />
        /// Endpoint:<br />
        /// GET /tags/{id}/related-tags
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="omitEmpty">["<c>includeTemplate</c>"] Omit empty</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get related tags for a tag
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-slug" /><br />
        /// Endpoint:<br />
        /// GET /tags/slug/{slug}/related-tags
        /// </para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="omitEmpty">["<c>includeTemplate</c>"] Omit empty</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get tags related to a tag by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-id" /><br />
        /// Endpoint:<br />
        /// GET /tags/{id}/related-tags/tags
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="omitEmpty">["<c>includeTemplate</c>"] Omit empty</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get tags related to a tag by slug
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-slug" /><br />
        /// Endpoint:<br />
        /// GET /tags/slug/{slug}/related-tags/tags
        /// </para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="omitEmpty">["<c>includeTemplate</c>"] Omit empty</param>
        /// <param name="status">["<c>status</c>"] Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get events
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/events/list-events-keyset-pagination" /><br />
        /// Endpoint:<br />
        /// GET /events/keyset
        /// </para>
        /// </summary>
        /// <param name="ids">["<c>id</c>"] Filter by ids</param>
        /// <param name="tagIds">["<c>tag_id</c>"] Filter by tag ids</param>
        /// <param name="excludeTagIds">["<c>exclude_tag_id</c>"] Filter by excluded tag ids</param>
        /// <param name="slugs">["<c>slug</c>"] Filter by slugs</param>
        /// <param name="tagSlug">["<c>tag_slug</c>"] Filter by tag slug</param>
        /// <param name="relatedTags">["<c>related_tags</c>"] Filter by related tags</param>
        /// <param name="closed">["<c>closed</c>"] Whether to return closed</param>
        /// <param name="live">["<c>live</c>"] Whether to return live</param>
        /// <param name="featured">["<c>featured</c>"] Whether to return featured</param>
        /// <param name="cyom">["<c>cyom</c>"] Whether to return cyom</param>
        /// <param name="titleSearch">["<c>title_search</c>"] Search by title</param>
        /// <param name="liquidityMin">["<c>liquidity_min</c>"] Filter by min liquidity</param>
        /// <param name="liquidityMax">["<c>liquidity_max</c>"] Filter by max liquidity</param>
        /// <param name="volumeMin">["<c>volume_min</c>"] Filter by min volume</param>
        /// <param name="volumeMax">["<c>volume_max</c>"] Filter by max volume</param>
        /// <param name="startDateMin">["<c>start_date_min</c>"] Filter start date by min value</param>
        /// <param name="startDateMax">["<c>start_date_max</c>"] Filter start date by max value</param>
        /// <param name="endDateMin">["<c>end_date_min</c>"] Filter end date by min value</param>
        /// <param name="endDateMax">["<c>end_date_max</c>"] Filter end date by max value</param>
        /// <param name="startTimeMin">["<c>start_time_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_time_max</c>"] Filter start time by max value</param>
        /// <param name="seriesIds">["<c>series_id</c>"] Filter by series ids</param>
        /// <param name="gameIds">["<c>game_id</c>"] Filter by game ids</param>
        /// <param name="eventDate">["<c>event_date</c>"] Filter by event date</param>
        /// <param name="eventWeek">["<c>event_week</c>"] Filter by event week</param>
        /// <param name="featuredOrder">["<c>featured_order</c>"] Whether to return featured order</param>
        /// <param name="recurrence">["<c>recurrence</c>"] Whether to return recurrence</param>
        /// <param name="createdBy">["<c>created_by</c>"] Filter by created by</param>
        /// <param name="parentEventId">["<c>parent_event_id</c>"] Filter by parent event id</param>
        /// <param name="includeChildren">["<c>include_children</c>"] Whether to include children</param>
        /// <param name="partnerSlug">["<c>partner_slug</c>"] Partner slug</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Whether to include chat</param>
        /// <param name="includeTemplate">["<c>include_template</c>"] Whether to include template</param>
        /// <param name="includeBestLines">["<c>include_best_lines</c>"] Whether to include best lines</param>
        /// <param name="locale">["<c>locale</c>"] Locale</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="afterCursor">["<c>after_cursor</c>"] Cursor from previous response</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketEventPage>> GetEventsKeysetPaginationAsync(
            long[]? ids = null,
            long[]? tagIds = null,
            long[]? excludeTagIds = null,
            string[]? slugs = null,
            string? tagSlug = null,
            bool? relatedTags = null,
            bool? closed = null,
            bool? live = null,
            bool? featured = null,
            bool? cyom = null,
            string? titleSearch = null,
            decimal? liquidityMin = null,
            decimal? liquidityMax = null,
            decimal? volumeMin = null,
            decimal? volumeMax = null,
            DateTime? startDateMin = null,
            DateTime? startDateMax = null,
            DateTime? endDateMin = null,
            DateTime? endDateMax = null,
            DateTime? startTimeMin = null,
            DateTime? startTimeMax = null,
            long[]? seriesIds = null,
            long[]? gameIds = null,
            DateTime? eventDate = null,
            int? eventWeek = null,
            bool? featuredOrder = null,
            string? recurrence = null,
            string[]? createdBy = null,
            long? parentEventId = null,
            bool? includeChildren = null,
            string? partnerSlug = null,
            bool? includeChat = null,
            bool? includeTemplate = null,
            bool? includeBestLines = null,
            string? locale = null,
            int? limit = null,
            string? afterCursor = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get events
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/events/list-events" /><br />
        /// Endpoint:<br />
        /// GET /events
        /// </para>
        /// </summary>
        /// <param name="ids">["<c>id</c>"] Filter by ids</param>
        /// <param name="tagId">["<c>tag_id</c>"] Filter by tag id</param>
        /// <param name="excludeTagIds">["<c>exclude_tag_id</c>"] Filter by excluded tag ids</param>
        /// <param name="slugs">["<c>slug</c>"] Filter by slugs</param>
        /// <param name="tagSlug">["<c>tag_slug</c>"] Filter by tag slug</param>
        /// <param name="relatedTags">["<c>related_tags</c>"] Filter by related tags</param>
        /// <param name="active">["<c>active</c>"] Whether to return active</param>
        /// <param name="archived">["<c>archived</c>"] Whether to return archived</param>
        /// <param name="featured">["<c>featured</c>"] Whether to return featured</param>
        /// <param name="cyom">["<c>cyom</c>"] Whether to return cyom</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Whether to include chat</param>
        /// <param name="includeTemplate">["<c>include_template</c>"] Whether to include template</param>
        /// <param name="recurrence">["<c>recurrence</c>"] Whether to return recurrence</param>
        /// <param name="closed">["<c>closed</c>"] Whether to return closed</param>
        /// <param name="liquidityMin">["<c>liquidity_min</c>"] Filter by min liquidity</param>
        /// <param name="liquidityMax">["<c>liquidity_max</c>"] Filter by max liquidity</param>
        /// <param name="volumeMin">["<c>volume_min</c>"] Filter by min volume</param>
        /// <param name="volumeMax">["<c>volume_max</c>"] Filter by max volume</param>
        /// <param name="startTimeMin">["<c>start_date_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_date_max</c>"] Filter start time by max value</param>
        /// <param name="endTimeMin">["<c>end_date_min</c>"] Filter end time by min value</param>
        /// <param name="endTimeMax">["<c>end_date_max</c>"] Filter end time by max value</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketEvent[]>> GetEventsAsync(
            long[]? ids = null,
            long? tagId = null,
            long[]? excludeTagIds = null,
            string[]? slugs = null,
            string? tagSlug = null,
            bool? relatedTags = null,
            bool? active = null,
            bool? archived = null,
            bool? featured = null,
            bool? cyom = null,
            bool? includeChat = null,
            bool? includeTemplate = null,
            bool? recurrence = null,
            bool? closed = null,
            decimal? liquidityMin = null,
            decimal? liquidityMax = null,
            decimal? volumeMin = null,
            decimal? volumeMax = null,
            DateTime? startTimeMin = null,
            DateTime? startTimeMax = null,
            DateTime? endTimeMin = null,
            DateTime? endTimeMax = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get event by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/events/get-event-by-id" /><br />
        /// Endpoint:<br />
        /// GET /events/{id}
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Include chat</param>
        /// <param name="includeTemplate">["<c>include_template</c>"] Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get event by slug
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/events/get-event-by-slug" /><br />
        /// Endpoint:<br />
        /// GET /events/slug/{slug}
        /// </para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Include chat</param>
        /// <param name="includeTemplate">["<c>include_template</c>"] Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get event tags
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/events/get-event-tags" /><br />
        /// Endpoint:<br />
        /// GET /events/{id}/tags
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get markets with keyset pagination
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/list-markets-keyset-pagination" /><br />
        /// Endpoint:<br />
        /// GET /markets
        /// </para>
        /// </summary>
        /// <param name="ids">["<c>id</c>"] Filter by ids</param>
        /// <param name="slugs">["<c>slug</c>"] Filter by slugs</param>
        /// <param name="tagId">["<c>tag_id</c>"] Filter by tag id</param>
        /// <param name="cyom">["<c>cyom</c>"] Whether to return cyom</param>
        /// <param name="clobTokenIds">["<c>clob_token_ids</c>"] Filter by CLOB token ids</param>
        /// <param name="marketIds">["<c>condition_ids</c>"] Filter by market/condition ids</param>
        /// <param name="marketMakerAddresses">["<c>market_maker_address</c>"] Filter by market maker addresses</param>
        /// <param name="sportMarketTypes">["<c>sports_market_types</c>"] Filter by sport market types</param>
        /// <param name="questionIds">["<c>question_ids</c>"] Filter by question ids</param>
        /// <param name="closed">["<c>closed</c>"] Whether to return closed</param>
        /// <param name="umaResolutionStatus">["<c>uma_resolution_status</c>"] Filter by UMA resolution status</param>
        /// <param name="gameId">["<c>game_id</c>"] Filter by game id</param>
        /// <param name="rewardsMin">["<c>rewards_min</c>"] Filter by min rewards</param>
        /// <param name="relatedTags">["<c>related_tags</c>"] Whether to return related tags</param>
        /// <param name="includeTag">["<c>include_tag</c>"] Whether to return tag</param>
        /// <param name="liquidityMin">["<c>liquidity_min</c>"] Filter by min liquidity</param>
        /// <param name="liquidityMax">["<c>liquidity_max</c>"] Filter by max liquidity</param>
        /// <param name="volumeMin">["<c>volume_min</c>"] Filter by min volume</param>
        /// <param name="volumeMax">["<c>volume_max</c>"] Filter by max volume</param>
        /// <param name="startTimeMin">["<c>start_date_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_date_max</c>"] Filter start time by max value</param>
        /// <param name="endTimeMin">["<c>end_date_min</c>"] Filter end time by min value</param>
        /// <param name="endTimeMax">["<c>end_date_max</c>"] Filter end time by max value</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="tagMatch">["<c>tag_match</c>"] Filter by tag match</param>
        /// <param name="rfqEnabled">["<c>rfq_enabled</c>"] Filter by RFQ enabled</param>
        /// <param name="locale">["<c>locale</c>"] Locale</param>
        /// <param name="version">["<c>version</c>"] v1 or v2</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="cursor">["<c>after_cursor</c>"] Cursor for pagination</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketMarketPage>> GetMarketsKeysetPaginationAsync(
            long[]? ids = null,
            long? tagId = null,
            string[]? slugs = null,
            string[]? clobTokenIds = null,
            string[]? marketIds = null,
            string[]? marketMakerAddresses = null,
            bool? closed = null,
            bool? relatedTags = null,
            bool? includeTag = null,
            bool? cyom = null,
            string? umaResolutionStatus = null,
            string? gameId = null,
            string[]? sportMarketTypes = null,
            decimal? rewardsMin = null,
            string[]? questionIds = null,
            decimal? liquidityMin = null,
            decimal? liquidityMax = null,
            decimal? volumeMin = null,
            decimal? volumeMax = null,
            DateTime? startTimeMin = null,
            DateTime? startTimeMax = null,
            DateTime? endTimeMin = null,
            DateTime? endTimeMax = null,
            string? tagMatch = null,
            bool? rfqEnabled = null,
            string? locale = null,
            string? version = null,
            int? limit = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            string? cursor = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get markets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/list-markets" /><br />
        /// Endpoint:<br />
        /// GET /markets
        /// </para>
        /// </summary>
        /// <param name="ids">["<c>id</c>"] Filter by ids</param>
        /// <param name="slugs">["<c>slug</c>"] Filter by slugs</param>
        /// <param name="tagId">["<c>tag_id</c>"] Filter by tag id</param>
        /// <param name="cyom">["<c>cyom</c>"] Whether to return cyom</param>
        /// <param name="clobTokenIds">["<c>clob_token_ids</c>"] Filter by CLOB token ids</param>
        /// <param name="marketIds">["<c>condition_ids</c>"] Filter by market/condition ids</param>
        /// <param name="marketMakerAddresses">["<c>market_maker_address</c>"] Filter by market maker addresses</param>
        /// <param name="sportMarketTypes">["<c>sports_market_types</c>"] Filter by sport market types</param>
        /// <param name="questionIds">["<c>question_ids</c>"] Filter by question ids</param>
        /// <param name="closed">["<c>closed</c>"] Whether to return closed</param>
        /// <param name="umaResolutionStatus">["<c>uma_resolution_status</c>"] Filter by UMA resolution status</param>
        /// <param name="gameId">["<c>game_id</c>"] Filter by game id</param>
        /// <param name="rewardsMin">["<c>rewards_min</c>"] Filter by min rewards</param>
        /// <param name="relatedTags">["<c>related_tags</c>"] Whether to return related tags</param>
        /// <param name="includeTag">["<c>include_tag</c>"] Whether to return tag</param>
        /// <param name="liquidityMin">["<c>liquidity_min</c>"] Filter by min liquidity</param>
        /// <param name="liquidityMax">["<c>liquidity_max</c>"] Filter by max liquidity</param>
        /// <param name="volumeMin">["<c>volume_min</c>"] Filter by min volume</param>
        /// <param name="volumeMax">["<c>volume_max</c>"] Filter by max volume</param>
        /// <param name="startTimeMin">["<c>start_date_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_date_max</c>"] Filter start time by max value</param>
        /// <param name="endTimeMin">["<c>end_date_min</c>"] Filter end time by min value</param>
        /// <param name="endTimeMax">["<c>end_date_max</c>"] Filter end time by max value</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketGammaMarket[]>> GetMarketsAsync(
            long[]? ids = null,
            long? tagId = null,
            string[]? slugs = null,
            string[]? clobTokenIds = null,
            string[]? marketIds = null,
            string[]? marketMakerAddresses = null,
            bool? closed = null,
            bool? relatedTags = null,
            bool? includeTag = null,
            bool? cyom = null,
            string? umaResolutionStatus = null,
            string? gameId = null,
            string[]? sportMarketTypes = null,
            decimal? rewardsMin = null,
            string[]? questionIds = null,
            decimal? liquidityMin = null,
            decimal? liquidityMax = null,
            decimal? volumeMin = null,
            decimal? volumeMax = null,
            DateTime? startTimeMin = null,
            DateTime? startTimeMax = null,
            DateTime? endTimeMin = null,
            DateTime? endTimeMax = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get market by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-market-by-id" /><br />
        /// Endpoint:<br />
        /// GET /markets/{id}
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeTag">["<c>include_tag</c>"] Include tag</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketGammaMarket>> GetMarketByIdAsync(long id, bool? includeTag = null, CancellationToken ct = default);

        /// <summary>
        /// Get market reference by token id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-market-by-token" /><br />
        /// Endpoint:<br />
        /// GET /markets-by-token/{token_id}
        /// </para>
        /// </summary>
        /// <param name="tokenId">Token id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTokenMarket>> GetMarketByTokenAsync(string tokenId, CancellationToken ct = default);

        /// <summary>
        /// Get market by slug
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-market-by-id" /><br />
        /// Endpoint:<br />
        /// GET /markets/slug/{slug}
        /// </para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeTag">["<c>include_tag</c>"] Include tag</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default);

        /// <summary>
        /// Get market tags
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-market-tags-by-id" /><br />
        /// Endpoint:<br />
        /// GET /markets/{id}/tags
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get resolution state
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/markets/get-resolution-state" /><br />
        /// Endpoint:<br />
        /// GET /v2/resolutions
        /// </para>
        /// </summary>
        /// <param name="questionId">Get by question id</param>
        /// <param name="condition">Get by condition</param>
        /// <param name="eventId">Get by event id</param>
        /// <param name="ct">Cancellation token </param>
        Task<HttpResult<PolymarketResolutionState[]>> GetResolutionStateAsync(
            string? questionId = null,
            string? condition = null,            
            string? eventId = null, 
            CancellationToken ct = default);

        /// <summary>
        /// Get series
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/series/list-series" /><br />
        /// Endpoint:<br />
        /// GET /series
        /// </para>
        /// </summary>
        /// <param name="slugs">["<c>slug</c>"] Filter by slugs</param>
        /// <param name="categoryIds">["<c>category_ids</c>"] Filter by category ids</param>
        /// <param name="categoryLabels">["<c>category_labels</c>"] Filter by category labels</param>
        /// <param name="closed">["<c>closed</c>"] Whether to return closed</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Whether to include chat</param>
        /// <param name="recurrence">["<c>recurrence</c>"] Whether to return recurrence</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSeries[]>> GetSeriesAsync(
            string[]? slugs = null,
            string[]? categoryIds = null,
            string[]? categoryLabels = null,
            bool? closed = null,
            bool? includeChat = null,
            bool? recurrence = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get series by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/series/get-series-by-id" /><br />
        /// Endpoint:<br />
        /// GET /series/{id}
        /// </para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeChat">["<c>include_chat</c>"] Whether to include chat</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default);

        /// <summary>
        /// Public search
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/search/search-markets-events-and-profiles" /><br />
        /// Endpoint:<br />
        /// GET /public-search
        /// </para>
        /// </summary>
        /// <param name="query">["<c>q</c>"] Search query</param>
        /// <param name="cache">["<c>cache</c>"] Cache</param>
        /// <param name="eventStatus">["<c>events_status</c>"] Filter by event status</param>
        /// <param name="limitPerType">["<c>limit_per_type</c>"] Page size per type</param>
        /// <param name="page">["<c>page</c>"] Page number</param>
        /// <param name="eventTags">["<c>events_tag</c>"] Filter by event tags</param>
        /// <param name="keepClosedMarkets">["<c>keep_closed_markets</c>"] Keep closed markets</param>
        /// <param name="sort">["<c>sort</c>"] Sort order</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="searchTags">["<c>search_tags</c>"] Filter by search tags</param>
        /// <param name="searchProfiles">["<c>search_profiles</c>"] Include profiles</param>
        /// <param name="recurrence">["<c>recurrence</c>"] Recurrence</param>
        /// <param name="excludeTagIds">["<c>exclude_tag_id</c>"] Exclude tag ids</param>
        /// <param name="optimized">["<c>optimized</c>"] Optimized</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PolymarketSearchResult>> SearchAsync(
            string query,
            bool? cache = null,
            string? eventStatus = null,
            int? limitPerType = null,
            int? page = null,
            string[]? eventTags = null,
            int? keepClosedMarkets = null,
            string? sort = null,
            bool? ascending = null,
            bool? searchTags = null,
            bool? searchProfiles = null,
            string? recurrence = null,
            long[]? excludeTagIds = null,
            bool? optimized = null,
            CancellationToken ct = default);
    }
}
