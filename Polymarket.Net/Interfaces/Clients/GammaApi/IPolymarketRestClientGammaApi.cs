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
    public interface IPolymarketRestClientGammaApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Options
        /// </summary>
        public PolymarketRestOptions ClientOptions { get; }

        /// <summary>
        /// Get list of all sports teams
        /// <para><a href="https://docs.polymarket.com/api-reference/pricing/get-market-price" /></para>
        /// </summary>
        /// <param name="league">Filter by league name</param>
        /// <param name="name">Filter by name</param>
        /// <param name="abbreviation">Filter by abbreviation</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="offset">Result offset</param>
        /// <param name="orderBy">Order by fields</param>
        /// <param name="ascending">Ascending order</param>
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
        /// <para><a href="https://docs.polymarket.com/api-reference/sports/get-sports-metadata-information" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get valid sport market types
        /// <para><a href="https://docs.polymarket.com/api-reference/sports/get-valid-sports-market-types" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get tags
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/list-tags" /></para>
        /// </summary>
        /// <param name="includeTemplate">Include template</param>
        /// <param name="isCarousel">Filter by carousel</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="offset">Result offset</param>
        /// <param name="orderBy">Order by fields</param>
        /// <param name="ascending">Ascending order</param>
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
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-tag-by-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeTemplate">Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get tag by slug
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-tag-by-id" /></para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeTemplate">Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get related tags for a tag
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="omitEmpty">Omit empty</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get related tags for a tag
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-slug" /></para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="omitEmpty">Omit empty</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get tags related to a tag by id
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="omitEmpty">Omit empty</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get tags related to a tag by slug
        /// <para><a href="https://docs.polymarket.com/api-reference/tags/get-related-tags-relationships-by-tag-slug" /></para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="omitEmpty">Omit empty</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Get events
        /// <para><a href="https://docs.polymarket.com/api-reference/events/list-events" /></para>
        /// </summary>
        /// <param name="ids">Filter by ids</param>
        /// <param name="tagId">Filter by tag id</param>
        /// <param name="excludeTagIds">Filter by excluded tag ids</param>
        /// <param name="slugs">Filter by slugs</param>
        /// <param name="tagSlug">Filter by tag slug</param>
        /// <param name="relatedTags">Filter by related tags</param>
        /// <param name="active">Whether to return active</param>
        /// <param name="archived">Whether to return archived</param>
        /// <param name="featured">Whether to return featured</param>
        /// <param name="cyom">Whether to return cyom</param>
        /// <param name="includeChat">Whether to include chat</param>
        /// <param name="includeTemplate">Whether to include template</param>
        /// <param name="recurrence">Whether to return recurrence</param>
        /// <param name="closed">Whether to return closed</param>
        /// <param name="liquidityMin">Filter by min liquidity</param>
        /// <param name="liquidityMax">Filter by max liquidity</param>
        /// <param name="volumeMin">Filter by min volume</param>
        /// <param name="volumeMax">Filter by max volume</param>
        /// <param name="startTimeMin">Filter start time by min value</param>
        /// <param name="startTimeMax">Filter start time by max value</param>
        /// <param name="endTimeMin">Filter end time by min value</param>
        /// <param name="endTimeMax">Filter end time by max value</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="offset">Result offset</param>
        /// <param name="orderBy">Order by fields</param>
        /// <param name="ascending">Ascending order</param>
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
        /// <para><a href="https://docs.polymarket.com/api-reference/events/get-event-by-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeChat">Include chat</param>
        /// <param name="includeTemplate">Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get event by slug
        /// <para><a href="https://docs.polymarket.com/api-reference/events/get-event-by-slug" /></para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeChat">Include chat</param>
        /// <param name="includeTemplate">Include template</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default);

        /// <summary>
        /// Get event tags
        /// <para><a href="https://docs.polymarket.com/api-reference/events/get-event-tags" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get events
        /// <para><a href="https://docs.polymarket.com/api-reference/events/list-events" /></para>
        /// </summary>
        /// <param name="ids">Filter by ids</param>
        /// <param name="slugs">Filter by slugs</param>
        /// <param name="tagId">Filter by tag id</param>
        /// <param name="cyom">Whether to return cyom</param>
        /// <param name="clobTokenIds">Filter by CLOB token ids</param>
        /// <param name="conditionIds">Filter by condition ids</param>
        /// <param name="marketMakerAddresses">Filter by market maker addresses</param>
        /// <param name="sportMarketTypes">Filter by sport market types</param>
        /// <param name="questionIds">Filter by question ids</param>
        /// <param name="closed">Whether to return closed</param>
        /// <param name="umaResolutionStatus">Filter by UMA resolution status</param>
        /// <param name="gameId">Filter by game id</param>
        /// <param name="rewardsMin">Filter by min rewards</param>
        /// <param name="relatedTags">Whether to return related tags</param>
        /// <param name="includeTag">Whether to return tag</param>
        /// <param name="liquidityMin">Filter by min liquidity</param>
        /// <param name="liquidityMax">Filter by max liquidity</param>
        /// <param name="volumeMin">Filter by min volume</param>
        /// <param name="volumeMax">Filter by max volume</param>
        /// <param name="startTimeMin">Filter start time by min value</param>
        /// <param name="startTimeMax">Filter start time by max value</param>
        /// <param name="endTimeMin">Filter end time by min value</param>
        /// <param name="endTimeMax">Filter end time by max value</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="offset">Result offset</param>
        /// <param name="orderBy">Order by fields</param>
        /// <param name="ascending">Ascending order</param>
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
        /// <para><a href="https://docs.polymarket.com/api-reference/markets/get-market-by-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeTag">Include tag</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketGammaMarket>> GetMarketByIdAsync(string id, bool? includeTag = null, CancellationToken ct = default);

        /// <summary>
        /// Get market by slug
        /// <para><a href="https://docs.polymarket.com/api-reference/markets/get-market-by-id" /></para>
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="includeTag">Include tag</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default);

        /// <summary>
        /// Get market tags
        /// <para><a href="https://docs.polymarket.com/api-reference/markets/get-market-tags-by-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Get series
        /// <para><a href="https://docs.polymarket.com/api-reference/series/list-series" /></para>
        /// </summary>
        /// <param name="slugs">Filter by slugs</param>
        /// <param name="categoryIds">Filter by category ids</param>
        /// <param name="categoryLabels">Filter by category labels</param>
        /// <param name="closed">Whether to return closed</param>
        /// <param name="includeChat">Whether to include chat</param>
        /// <param name="recurrence">Whether to return recurrence</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="offset">Result offset</param>
        /// <param name="orderBy">Order by fields</param>
        /// <param name="ascending">Ascending order</param>
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
        /// <para><a href="https://docs.polymarket.com/api-reference/series/get-series-by-id" /></para>
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeChat">Whether to include chat</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default);

        /// <summary>
        /// Public search
        /// <para><a href="https://docs.polymarket.com/api-reference/search/search-markets-events-and-profiles" /></para>
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="cache">Cache</param>
        /// <param name="eventStatus">Filter by event status</param>
        /// <param name="limitPerType">Page size per type</param>
        /// <param name="page">Page number</param>
        /// <param name="eventTags">Filter by event tags</param>
        /// <param name="keepClosedMarkets">Keep closed markets</param>
        /// <param name="sort">Sort order</param>
        /// <param name="ascending">Ascending order</param>
        /// <param name="searchTags">Filter by search tags</param>
        /// <param name="searchProfiles">Include profiles</param>
        /// <param name="recurrence">Recurrence</param>
        /// <param name="excludeTagIds">Exclude tag ids</param>
        /// <param name="optimized">Optimized</param>
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
