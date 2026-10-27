using System;
using System.Text.Json.Serialization;
using Polymarket.Net.Enums;

namespace Polymarket.Net.Objects.Models;

/// <summary>
/// Collateral asset
/// </summary>
public record PolymarketCollateralAsset
{
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>address</c>"] Address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>decimals</c>"] Decimals
    /// </summary>
    [JsonPropertyName("decimals")]
    public int Decimals { get; set; }
    /// <summary>
    /// ["<c>collateral_ratio</c>"] Collateral ratio
    /// </summary>
    [JsonPropertyName("collateral_ratio")]
    public decimal CollateralRatio { get; set; }
    /// <summary>
    /// ["<c>withdrawal_fee</c>"] Withdrawal fee
    /// </summary>
    [JsonPropertyName("withdrawal_fee")]
    public decimal WithdrawalFee { get; set; }
}

