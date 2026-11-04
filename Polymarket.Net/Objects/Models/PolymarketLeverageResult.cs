using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Leverage info
/// </summary>
public record PolymarketLeverageResult
{
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>instrument_id</c>"] Instrument id
    /// </summary>
    [JsonPropertyName("instrument_id")]
    public long InstrumentId { get; set; }
    /// <summary>
    /// ["<c>leverage</c>"] Leverage
    /// </summary>
    [JsonPropertyName("leverage")]
    public decimal Leverage { get; set; }
    /// <summary>
    /// ["<c>cross</c>"] Cross
    /// </summary>
    [JsonPropertyName("cross")]
    public bool Cross { get; set; }
}

