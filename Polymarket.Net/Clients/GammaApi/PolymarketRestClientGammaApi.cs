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
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Models;
using System.Collections.Generic;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Clients.GammaApi
{
    /// <inheritdoc cref="IPolymarketRestClientGammaApi" />
    internal partial class PolymarketRestClientGammaApi : RestApiClient, IPolymarketRestClientGammaApi
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
        internal PolymarketRestClientGammaApi(ILogger logger, HttpClient? httpClient, PolymarketRestOptions options)
            : base(logger, httpClient, options.Environment.GammaRestClientAddress, options, options.GammaOptions)
        {
            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Delete] = HttpMethodParameterPosition.InBody;
            ArraySerialization = ArrayParametersSerialization.MultipleValues;

            OrderParameters = false;
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(PolymarketExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PolymarketExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new PolymarketAuthenticationProvider((PolymarketCredentials)credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => throw new NotImplementedException();

        #region Get Sport Teams

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSportsTeam[]>> GetSportTeamsAsync(
            IEnumerable<string>? league = null,
            IEnumerable<string>? name = null,
            IEnumerable<string>? abbreviation = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptional("league", league);
            parameters.AddOptional("name", name);
            parameters.AddOptional("abbreviation", abbreviation);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "teams", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketSportsTeam[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sports

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSport[]>> GetSportsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sports", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketSport[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sport Market Types

        /// <inheritdoc />
        public async Task<WebCallResult<string[]>> GetSportMarketTypesAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sports/market-types", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            var result = await SendAsync<PolymarketSportMarketTypes>(request, parameters, ct).ConfigureAwait(false);
            return result.As<string[]>(result.Data?.MarketTypes);
        }

        #endregion

        #region Get Tags

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag[]>> GetTagsAsync(
            bool? includeTemplate = null,
            bool? isCarousel = null,
            int? limit = null,
            int? offset = null,
            IEnumerable<string>? orderBy = null,
            bool? ascending = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptionalBoolString("includeTemplate", includeTemplate);
            parameters.AddOptionalBoolString("isCarousel", isCarousel);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Id

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag>> GetTagByIdAsync(string id, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "tags/" + id, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tag By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag>> GetTagBySlugAsync(string slug, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "tags/slug/" + slug, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Id

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", omitEmpty);
            parameters.AddOptionalEnum("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"tags/{id}/related-tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketRelatedTag[]>> GetRelatedTagsBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", omitEmpty);
            parameters.AddOptionalEnum("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"tags/slug/{slug}/related-tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketRelatedTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagByIdAsync(string id, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", omitEmpty);
            parameters.AddOptionalEnum("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"tags/{id}/related-tags/tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Related Tags By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag[]>> GetTagsRelatedToTagBySlugAsync(string slug, bool? omitEmpty = null, TagStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("includeTemplate", omitEmpty);
            parameters.AddOptionalEnum("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"tags/slug/{slug}/related-tags/tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Events

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketEvent[]>> GetEventsAsync(
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
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", ids);
            parameters.AddOptional("tag_id", tagId);
            parameters.AddOptional("exclude_tag_id", excludeTagIds);
            parameters.AddOptional("slug", slugs);
            parameters.AddOptional("tag_slug", tagSlug);
            parameters.AddOptionalBoolString("related_tags", relatedTags);
            parameters.AddOptionalBoolString("active", active);
            parameters.AddOptionalBoolString("archived", archived);
            parameters.AddOptionalBoolString("featured", featured);
            parameters.AddOptionalBoolString("cyom", cyom);
            parameters.AddOptionalBoolString("include_chat", includeChat);
            parameters.AddOptionalBoolString("include_template", includeTemplate);
            parameters.AddOptionalBoolString("recurrence", recurrence);
            parameters.AddOptionalBoolString("closed", closed);
            parameters.AddOptionalString("liquidity_min", liquidityMin);
            parameters.AddOptionalString("liquidity_max", liquidityMax);
            parameters.AddOptionalString("volume_min", volumeMin);
            parameters.AddOptionalString("volume_max", volumeMax);
            parameters.AddOptionalString("start_data_min", startTimeMin);
            parameters.AddOptionalString("start_data_max", startTimeMax);
            parameters.AddOptionalString("end_data_min", endTimeMin);
            parameters.AddOptionalString("end_data_max", endTimeMax);

            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"events", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketEvent[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Id

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketEvent>> GetEventByIdAsync(string id, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("include_chat", includeChat);
            parameters.AddOptionalBoolString("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"events/" + id, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketEvent>> GetEventBySlugAsync(string slug, bool? includeChat = null, bool? includeTemplate = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("include_chat", includeChat);
            parameters.AddOptionalBoolString("include_template", includeTemplate);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"events/slug/" + slug, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketEvent>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Event Tags

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag[]>> GetEventTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"events/{id}/tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Markets

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketGammaMarket[]>> GetMarketsAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("id", ids);
            parameters.AddOptional("tag_id", tagId);
            parameters.AddOptional("slug", slugs);
            parameters.AddOptional("clob_token_ids", clobTokenIds);
            parameters.AddOptional("market_maker_address", marketMakerAddresses);
            parameters.AddOptional("uma_resolution_status", umaResolutionStatus);
            parameters.AddOptional("game_id", gameId);
            parameters.AddOptional("sports_market_types", sportMarketTypes);
            parameters.AddOptional("question_ids", questionIds);
            parameters.AddOptionalBoolString("related_tags", relatedTags);
            parameters.AddOptionalBoolString("cyom", cyom);
            parameters.AddOptionalBoolString("include_tag", includeTag);
            parameters.AddOptionalBoolString("closed", closed);
            parameters.AddOptionalString("liquidity_min", liquidityMin);
            parameters.AddOptionalString("liquidity_max", liquidityMax);
            parameters.AddOptionalString("rewards_min", rewardsMin);
            parameters.AddOptionalString("volume_min", volumeMin);
            parameters.AddOptionalString("volume_max", volumeMax);
            parameters.AddOptionalString("start_data_min", startTimeMin);
            parameters.AddOptionalString("start_data_max", startTimeMax);
            parameters.AddOptionalString("end_data_min", endTimeMin);
            parameters.AddOptionalString("end_data_max", endTimeMax);

            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"markets", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketGammaMarket[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Id

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketGammaMarket>> GetMarketByIdAsync(string id, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"markets/" + id, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market By Slug

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketGammaMarket>> GetMarketBySlugAsync(string slug, bool? includeTag = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("include_tag", includeTag);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"markets/slug/" + slug, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketGammaMarket>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Market Tags

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketTag[]>> GetMarketTagsAsync(string id, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"markets/{id}/tags", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketTag[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Series

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSeries[]>> GetSeriesAsync(
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
            var parameters = new ParameterCollection();
            parameters.AddOptional("slug", slugs);
            parameters.AddOptional("category_ids", categoryIds);
            parameters.AddOptional("category_labels", categoryLabels);
            parameters.AddOptionalBoolString("include_chat", includeChat);
            parameters.AddOptionalBoolString("recurrence", recurrence);
            parameters.AddOptionalBoolString("closed", closed);
            parameters.AddOptionalCommaSeparated("order", orderBy);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.Add("limit", limit ?? 20);
            parameters.Add("offset", offset ?? 0);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"series", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketSeries[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Series By Id

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSeries>> GetSeriesByIdAsync(string id, bool? includeChat = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalBoolString("include_chat", includeChat);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"series/" + id, PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketSeries>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Search

        /// <inheritdoc />
        public async Task<WebCallResult<PolymarketSearchResult>> SearchAsync(
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
            var parameters = new ParameterCollection();
            parameters.Add("q", query);
            parameters.AddOptionalBoolString("cache", cache);
            parameters.AddOptional("events_status", eventStatus);
            parameters.AddOptional("limit_per_type", limitPerType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("events_tag", eventTags);
            parameters.AddOptional("keep_closed_markets", keepClosedMarkets);
            parameters.AddOptional("sort", sort);
            parameters.AddOptionalBoolString("ascending", ascending);
            parameters.AddOptionalBoolString("search_tags", searchTags);
            parameters.AddOptionalBoolString("search_profiles", searchProfiles);
            parameters.AddOptional("recurrence", recurrence);
            parameters.AddOptional("exclude_tag_id", excludeTagIds);
            parameters.AddOptionalBoolString("optimized", optimized);
            var request = _definitions.GetOrCreate(HttpMethod.Get, $"public-search", PolymarketExchange.RateLimiter.Polymarket, 1, false);
            return await SendAsync<PolymarketSearchResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
