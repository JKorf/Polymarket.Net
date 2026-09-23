using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polymarket.Net.Interfaces.Clients.GammaApi;
using Polymarket.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Polymarket.Net.Clients.MessageHandlers;
using Polymarket.Net.Objects.Models;
using System.Collections.Generic;
using Polymarket.Net.Enums;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Linq;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <inheritdoc cref="IPolymarketRestClientGammaApi" />
    internal partial class PolymarketRestClientGammaApi : RestApiClient<PolymarketEnvironment, PolymarketAuthenticationProvider, PolymarketCredentials>, IPolymarketRestClientGammaApi
    {
        #region fields 
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        protected override ErrorMapping ErrorMapping => PolymarketErrors.Errors;

        public new PolymarketRestOptions ClientOptions => (PolymarketRestOptions)base.ClientOptions;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PolymarketRestMessageHandler(PolymarketErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public string ExchangeName => "Polymarket";
        #endregion

        #region constructor/destructor
        internal PolymarketRestClientGammaApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, PolymarketRestOptions options)
            : base(loggerFactory, PolymarketPlatform.Metadata.Id, httpClient, options.Environment.GammaRestClientAddress, options, options.GammaOptions)
        {
            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InBody;
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);


        /// <inheritdoc />
        protected override PolymarketAuthenticationProvider CreateAuthenticationProvider(PolymarketCredentials credentials)
            => new PolymarketAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "teams", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketSportsTeam[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sports

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "sports", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketSport[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sport Market Types

        /// <inheritdoc />
        public async Task<HttpResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "sports/market-types", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            var result = await SendAsync<PolymarketSportMarketTypes>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(200, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "tags/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "tags/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"tags/{id}/related-tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"tags/slug/{slug}/related-tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"tags/{id}/related-tags/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("includeTemplate", omitEmpty);
            parameters.Add("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"tags/slug/{slug}/related-tags/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"events/keyset", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await SendAsync<PolymarketEventPage>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"events", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await SendAsync<PolymarketEvent[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"events/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            parameters.Add("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"events/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event Tags

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"events/{id}/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"markets", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(300, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await SendAsync<PolymarketGammaMarket[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGammaMarket>> GetMarketByIdAsync(string id, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"markets/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Slug

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"markets/slug/" + slug, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market Tags

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"markets/{id}/tags", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"series", PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketSeries[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Series By Id

        /// <inheritdoc />
        public async Task<HttpResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(PolymarketPlatform._gammaParameterSerializationSettings);
            parameters.Add("include_chat", includeChat);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"series/" + id, PolymarketPlatform.RateLimiter.GammaApi, 1, false);
            return await SendAsync<PolymarketSeries>(request, parameters, ct).ConfigureAwait(false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, $"public-search", PolymarketPlatform.RateLimiter.GammaApi, 1, false,
                limitGuard: new SingleLimitGuard(350, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));
            return await SendAsync<PolymarketSearchResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
