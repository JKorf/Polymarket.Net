using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Models
{
    /// <summary>
    /// Sport update
    /// </summary>
    public record PolymarketSportsUpdate
    {
        /// <summary>
        /// ["<c>gameId</c>"] Game id
        /// </summary>
        [JsonPropertyName("gameId")]
        public long GameId { get; set; }
        /// <summary>
        /// ["<c>leagueAbbreviation</c>"] League abbreviation
        /// </summary>
        [JsonPropertyName("leagueAbbreviation")]
        public string LeagueAbbreviation { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>homeTeam</c>"] Home team name
        /// </summary>
        [JsonPropertyName("homeTeam")]
        public string HomeTeam { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>awayTeam</c>"] Away team name
        /// </summary>
        [JsonPropertyName("awayTeam")]
        public string AwayTeam { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>score</c>"] Score
        /// </summary>
        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>period</c>"] Period
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>elapsed</c>"] Elapsed
        /// </summary>
        [JsonPropertyName("elapsed")]
        public string? Elapsed { get; set; }
        /// <summary>
        /// ["<c>turn</c>"] Turn
        /// </summary>
        [JsonPropertyName("turn")]
        public string? Turn { get; set; }
        /// <summary>
        /// ["<c>live</c>"] Is live
        /// </summary>
        [JsonPropertyName("live")]
        public bool Live { get; set; }
        /// <summary>
        /// ["<c>ended</c>"] Has ended
        /// </summary>
        [JsonPropertyName("ended")]
        public bool Ended { get; set; }
    }
}
