using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Models;
using Polymarket.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Interfaces.Clients.GammaApi
{
    /// <summary>
    /// Polymarket Gamma API endpoints
    /// </summary>
    public interface IPolymarketRestClientGammaApi : IRestApiClient<PolymarketCredentials>, IDisposable
    {
        /// <summary>
        /// Options
        /// </summary>
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Get list of all sports teams
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.polymarket.com/api-reference/pricing/get-market-price" /><br />
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
        Task<WebCallResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
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
        Task<WebCallResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default);

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
        Task<WebCallResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag[]>> GetTagsAsync(
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
        Task<WebCallResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

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
        /// <param name="startTimeMin">["<c>start_data_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_data_max</c>"] Filter start time by max value</param>
        /// <param name="endTimeMin">["<c>end_data_min</c>"] Filter end time by min value</param>
        /// <param name="endTimeMax">["<c>end_data_max</c>"] Filter end time by max value</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketEvent[]>> GetEventsAsync(
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
        Task<WebCallResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get events
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
        /// <param name="conditionIds">["<c>condition_ids</c>"] Filter by condition ids</param>
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
        /// <param name="startTimeMin">["<c>start_data_min</c>"] Filter start time by min value</param>
        /// <param name="startTimeMax">["<c>start_data_max</c>"] Filter start time by max value</param>
        /// <param name="endTimeMin">["<c>end_data_min</c>"] Filter end time by min value</param>
        /// <param name="endTimeMax">["<c>end_data_max</c>"] Filter end time by max value</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="offset">["<c>offset</c>"] Result offset</param>
        /// <param name="orderBy">["<c>order</c>"] Order by fields</param>
        /// <param name="ascending">["<c>ascending</c>"] Ascending order</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketGammaMarket[]>> GetMarketsAsync(
            long[]? ids = null,
            long? tagId = null,
            string[]? slugs = null,
            string[]? clobTokenIds = null,
            string[]? conditionIds = null,
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
        Task<WebCallResult<PolymarketGammaMarket>> GetMarketByIdAsync(string id, bool? includeTag = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketSeries[]>> GetSeriesAsync(
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
        Task<WebCallResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default);

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
        Task<WebCallResult<PolymarketSearchResult>> SearchAsync(
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
