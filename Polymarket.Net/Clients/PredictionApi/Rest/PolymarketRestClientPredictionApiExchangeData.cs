using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces.Clients.PredictionApi;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polymarket.Net.Clients.PredictionApi.Rest
{
    internal class PolymarketRestClientPredictionApiExchangeData : IPolymarketRestClientPredictionApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly PolymarketRestClientPredictionApi _client;

        public PolymarketRestClientPredictionApiExchangeData(PolymarketRestClientPredictionApi client)
        {
            _client = client;
        }

        #region Get Sport Teams

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddCommaSeparated("order", orderBy);
            parameters.AddRaw("league", league?.ToArray());
            parameters.AddRaw("name", name?.ToArray());
            parameters.AddRaw("abbreviation", abbreviation?.ToArray());
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.BaseAddress, "teams", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketSportsTeam[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sports

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, "sports", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketSport[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sport Market Types

        /// <inheritdoc />
        public async Task<HttpResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, "sports/market-types", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            var result = await _client.SendAsync<PolymarketSportMarketTypes>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<string[]>(result);

            return HttpResult.Ok(result, result.Data.MarketTypes);
        }

        #endregion

        #region Get Tags

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetTagsAsync(
            bool? includeTemplate = null,
            bool? isCarousel = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddCommaSeparated("order", orderBy);
            parameters.Add("includeTemplate", includeTemplate);
            parameters.Add("isCarousel", isCarousel);
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, "tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, "tags/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, "tags/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"tags/{id}/related-tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"tags/slug/{slug}/related-tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"tags/{id}/related-tags/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"tags/slug/{slug}/related-tags/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Events

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEventPage>> GetEventsKeysetPaginationAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddArray("id", ids);
            parameters.AddArray("tag_id", tagIds);
            parameters.AddArray("exclude_tag_id", excludeTagIds);
            parameters.AddArray("slug", slugs);
            parameters.Add("tag_slug", tagSlug);
            parameters.Add("related_tags", relatedTags);
            parameters.Add("closed", closed);
            parameters.Add("live", live);
            parameters.Add("featured", featured);
            parameters.Add("cyom", cyom);
            parameters.Add("title_search", titleSearch);
            parameters.Add("liquidity_min", liquidityMin);
            parameters.Add("liquidity_max", liquidityMax);
            parameters.Add("volume_min", volumeMin);
            parameters.Add("volume_max", volumeMax);
            parameters.Add("start_date_min", startDateMin);
            parameters.Add("start_date_max", startDateMax);
            parameters.Add("end_date_min", endDateMin);
            parameters.Add("end_date_max", endDateMax);
            parameters.Add("start_time_min", startTimeMin);
            parameters.Add("start_time_max", startTimeMax);
            parameters.AddArray("series_id", seriesIds);
            parameters.AddArray("game_id", gameIds);
            parameters.Add("event_date", eventDate);
            parameters.Add("event_week", eventWeek);
            parameters.Add("featured_order", featuredOrder);
            parameters.Add("recurrence", recurrence);
            parameters.AddArray("created_by", createdBy);
            parameters.Add("parent_event_id", parentEventId);
            parameters.Add("include_children", includeChildren);
            parameters.Add("partner_slug", partnerSlug);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            parameters.Add("include_best_lines", includeBestLines);
            parameters.Add("locale", locale);
            parameters.Add("after_cursor", afterCursor);
            parameters.AddCommaSeparated("order", orderBy);
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);

            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"events/keyset", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketEventPage>(request, parameters, ct).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEvent[]>> GetEventsAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddArray("id", ids);
            parameters.Add("tag_id", tagId);
            parameters.AddArray("exclude_tag_id", excludeTagIds);
            parameters.AddArray("slug", slugs);
            parameters.Add("tag_slug", tagSlug);
            parameters.Add("related_tags", relatedTags);
            parameters.Add("active", active);
            parameters.Add("archived", archived);
            parameters.Add("featured", featured);
            parameters.Add("cyom", cyom);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            parameters.Add("recurrence", recurrence);
            parameters.Add("closed", closed);
            parameters.Add("liquidity_min", liquidityMin);
            parameters.Add("liquidity_max", liquidityMax);
            parameters.Add("volume_min", volumeMin);
            parameters.Add("volume_max", volumeMax);
            parameters.Add("start_date_min", startTimeMin);
            parameters.Add("start_date_max", startTimeMax);
            parameters.Add("end_date_min", endTimeMin);
            parameters.Add("end_date_max", endTimeMax);

            parameters.AddCommaSeparated("order", orderBy);
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"events", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketEvent[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"events/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"events/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event Tags

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"events/{id}/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGammaMarket[]>> GetMarketsAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddArray("id", ids);
            parameters.Add("tag_id", tagId);
            parameters.AddArray("slug", slugs);
            parameters.AddArray("clob_token_ids", clobTokenIds);
            parameters.AddArray("condition_ids", marketIds);
            parameters.AddArray("market_maker_address", marketMakerAddresses);
            parameters.Add("uma_resolution_status", umaResolutionStatus);
            parameters.Add("game_id", gameId);
            parameters.AddArray("sports_market_types", sportMarketTypes);
            parameters.AddArray("question_ids", questionIds);
            parameters.Add("related_tags", relatedTags);
            parameters.Add("cyom", cyom);
            parameters.Add("include_tag", includeTag);
            parameters.Add("closed", closed);
            parameters.Add("liquidity_min", liquidityMin);
            parameters.Add("liquidity_max", liquidityMax);
            parameters.Add("rewards_min", rewardsMin);
            parameters.Add("volume_min", volumeMin);
            parameters.Add("volume_max", volumeMax);
            parameters.Add("start_date_min", startTimeMin);
            parameters.Add("start_date_max", startTimeMax);
            parameters.Add("end_date_min", endTimeMin);
            parameters.Add("end_date_max", endTimeMax);

            parameters.AddCommaSeparated("order", orderBy);
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"markets", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(300, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketGammaMarket[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGammaMarket>> GetMarketByIdAsync(string id, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"markets/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"markets/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market Tags

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"markets/{id}/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Series

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSeries[]>> GetSeriesAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.AddArray("slug", slugs);
            parameters.AddArray("category_ids", categoryIds);
            parameters.AddArray("category_labels", categoryLabels);
            parameters.Add("include_chat", includeChat);
            parameters.Add("recurrence", recurrence);
            parameters.Add("closed", closed);
            parameters.AddCommaSeparated("order", orderBy);
            parameters.Add("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"series", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketSeries[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Series By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            var request = _definitions.GetOrCreate(HttpMethod.Get,_client.BaseAddress, $"series/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await _client.SendAsync<PolymarketSeries>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Search

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSearchResult>> SearchAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("q", query);
            parameters.Add("cache", cache);
            parameters.Add("events_status", eventStatus);
            parameters.Add("limit_per_type", limitPerType);
            parameters.Add("page", page);
            parameters.AddArray("events_tag", eventTags);
            parameters.Add("keep_closed_markets", keepClosedMarkets);
            parameters.Add("sort", sort);
            parameters.Add("ascending", ascending);
            parameters.Add("search_tags", searchTags);
            parameters.Add("search_profiles", searchProfiles);
            parameters.Add("recurrence", recurrence);
            parameters.AddArray("exclude_tag_id", excludeTagIds);
            parameters.Add("optimized", optimized);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.BaseAddress, $"public-search", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(350, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketSearchResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPosition[]>> GetPositionsAsync(
            string user,
            string? market = null,
            string? eventId = null,
            decimal? sizeThreshold = null,
            bool? redeemable = null,
            bool? mergeable = null,
            int? limit = null,
            int? offset = null,
            PositionSortBy? sortBy = null,
            SortDirection? sortDirection = null,
            string? title = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("user", user);
            parameters.Add("market", market);
            parameters.Add("eventId", eventId);
            parameters.Add("sizeThreshold", sizeThreshold);
            parameters.Add("redeemable", redeemable);
            parameters.Add("mergeable", mergeable);
            parameters.Add("limit", limit);
            parameters.Add("offset", offset);
            parameters.Add("sortBy", sortBy);
            parameters.Add("sortDirection", sortDirection);
            parameters.Add("title", title);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.DataApiAddress, "positions", PolymarketPlatform.RateLimiter.DataApi, 1, false);
            return await _client.SendAsync<PolymarketPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketClosedPosition[]>> GetClosedPositionsAsync(
            string user,
            string? market = null,
            string? eventId = null,
            int? limit = null,
            int? offset = null,
            ClosedPositionSortBy? sortBy = null,
            SortDirection? sortDirection = null,
            string? title = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("user", user);
            parameters.Add("market", market);
            parameters.Add("eventId", eventId);
            parameters.Add("limit", limit);
            parameters.Add("offset", offset);
            parameters.Add("sortBy", sortBy);
            parameters.Add("sortDirection", sortDirection);
            parameters.Add("title", title);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.DataApiAddress, "closed-positions", PolymarketPlatform.RateLimiter.DataApi, 1, false);
            return await _client.SendAsync<PolymarketClosedPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }


        #region Get Open Interest

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketOpenInterest[]>> GetOpenInterestAsync(IEnumerable<string>? marketIds = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.AddCommaSeparated("marketIds", marketIds);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.DataApiAddress, "/v2/oi", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            var result = await _client.SendV2Async<PolymarketOpenInterest[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Event Volume

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEventVolume>> GetEventVolumeAsync(IEnumerable<string> eventIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.AddCommaSeparated("event_id", eventIds);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.DataApiAddress, "/v2/live-volume", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            var result = await _client.SendV2Async<PolymarketEventVolume>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Lookup Tokens

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTokenInfo[]>> LookupTokensAsync(
            IEnumerable<string>? tokenIds = null,
            IEnumerable<string>? marketIds = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.AddCommaSeparated("tokenId", tokenIds);
            parameters.AddCommaSeparated("condition", marketIds);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.DataApiAddress, "/v2/tokens", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            var result = await _client.SendV2Async<PolymarketTokenInfo[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "time", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _client.SendAsync<long>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, DateTimeConverter.ConvertFromSeconds(result.Data)!);
        }

        #endregion

        #region Get Geographic Restrictions

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGeoRestriction>> GetGeographicRestrictionsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "https://polymarket.com", "api/geoblock", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketGeoRestriction>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Simplified Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSamplingSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "sampling-simplified-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketPage<PolymarketMarket>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sampling Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarketDetails>>> GetSamplingMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "sampling-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Simplified Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarket>>> GetSimplifiedMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "simplified-markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketPage<PolymarketMarket>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Markets

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPage<PolymarketMarketDetails>>> GetMarketsAsync(string? cursor = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("next_cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "markets", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketPage<PolymarketMarketDetails>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMarketDetails>> GetMarketAsync(string marketId, CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "markets/" + marketId, PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketMarketDetails>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPrice>> GetPriceAsync(string tokenId, OrderSide side, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            parameters.Add("side", side);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "price", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Prices

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, PolymarketBuySellPrice>>> GetPricesAsync(Dictionary<string, OrderSide> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketPriceRequest
                {
                    TokenId = x.Key,
                    Side = x.Value
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "prices", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<Dictionary<string, PolymarketBuySellPrice>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Midpoint Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMidPrice>> GetMidpointPriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "midpoint", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketMidPrice>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Midpoint Prices

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetMidpointPricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "midpoints", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPriceHistory[]>> GetPriceHistoryAsync(string market, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("market", market);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            parameters.Add("interval", interval);
            parameters.Add("fidelity", fidelity);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "/prices-history", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketPriceHistoryWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<PolymarketPriceHistory[]>(result);

            return HttpResult.Ok(result, result.Data.History);
        }

        #endregion

        #region Get Price History

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketPriceHistories>> GetPriceHistoriesAsync(IEnumerable<string> markets, DateTime? startTime = null, DateTime? endTime = null, DataInterval? interval = null, int? fidelity = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.AddArray("markets", markets);
            parameters.Add("startTs", startTime);
            parameters.Add("endTs", endTime);
            parameters.Add("interval", interval);
            parameters.Add("fidelity", fidelity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "/batch-prices-history", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketPriceHistories>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bid Ask Spread

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSpread>> GetBidAskSpreadsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "spread", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _client.SendAsync<PolymarketSpread>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bid Ask Spreads

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetBidAskSpreadsAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "spreads", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _client.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Info

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketOrderBook>> GetOrderBookAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "book", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(1500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Token Infos

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "books", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            var result = await _client.SendAsync<PolymarketOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tick Size

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTickSize>> GetTickSizeAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "tick-size", PolymarketPlatform.RateLimiter.ClobApi, 1, false,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await _client.SendAsync<PolymarketTickSize>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Negative Risk

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketNegRisk>> GetNegativeRiskAsyncAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "neg-risk", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketNegRisk>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Rate Bps

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketFeeRateBps>> GetFeeRateBpsAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "fee-rate", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketFeeRateBps>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Price

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTradePrice>> GetLastTradePriceAsync(string tokenId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            parameters.Add("token_id", tokenId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, "last-trade-price", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketTradePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Last Trade Prices

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTradePrice[]>> GetLastTradePricesAsync(IEnumerable<string> tokenIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(tokenIds.Select(x =>
                new PolymarketTokenRequest
                {
                    TokenId = x
                }
            ).ToArray(), PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _client.ClobApiAddress, "last-trades-prices", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            return await _client.SendAsync<PolymarketTradePrice[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market Info

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketMarketInfo>> GetMarketInfoAsync(string marketId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _client.ClobApiAddress, $"/clob-markets/{marketId}", PolymarketPlatform.RateLimiter.ClobApi, 1, false);
            var result = await _client.SendAsync<PolymarketMarketInfo>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
