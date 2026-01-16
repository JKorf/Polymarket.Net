using CryptoExchange.Net.Objects;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Internal;
using Polymarket.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Polymarket.Net.Converters
{
    [JsonSerializable(typeof(PolymarketMarketResolvedUpdate))]
    [JsonSerializable(typeof(PolymarketNewMarketUpdate))]
    [JsonSerializable(typeof(PolymarketBestBidAskUpdate))]
    [JsonSerializable(typeof(PolymarketTickSizeUpdate))]
    [JsonSerializable(typeof(PolymarketBookUpdate[]))]
    [JsonSerializable(typeof(PolymarketBookUpdate))]
    [JsonSerializable(typeof(PolymarketLastTradePriceUpdate))]
    [JsonSerializable(typeof(PolymarketPriceChangeUpdate))]
    [JsonSerializable(typeof(PolymarketSearchResult))]
    [JsonSerializable(typeof(PolymarketSeries))]
    [JsonSerializable(typeof(PolymarketSeries[]))]
    [JsonSerializable(typeof(PolymarketGammaMarket))]
    [JsonSerializable(typeof(PolymarketGammaMarket[]))]
    [JsonSerializable(typeof(PolymarketEvent[]))]
    [JsonSerializable(typeof(PolymarketRelatedTag[]))]
    [JsonSerializable(typeof(PolymarketTag))]
    [JsonSerializable(typeof(PolymarketTag[]))]
    [JsonSerializable(typeof(PolymarketSportMarketTypes))]
    [JsonSerializable(typeof(PolymarketSport[]))]
    [JsonSerializable(typeof(PolymarketSportsTeam[]))]
    [JsonSerializable(typeof(PolymarketGeoRestriction))]
    [JsonSerializable(typeof(PolymarketPrice))]
    [JsonSerializable(typeof(Dictionary<string, PolymarketBuySellPrice>))]
    [JsonSerializable(typeof(PolymarketPriceRequest[]))]
    [JsonSerializable(typeof(PolymarketMidPrice))]
    [JsonSerializable(typeof(PolymarketPriceHistoryWrapper[]))]
    [JsonSerializable(typeof(Dictionary<string, decimal>))]
    [JsonSerializable(typeof(PolymarketTokenRequest[]))]
    [JsonSerializable(typeof(PolymarketOrderBook))]
    [JsonSerializable(typeof(PolymarketOrderBook[]))]
    [JsonSerializable(typeof(PolymarketCreds))]
    [JsonSerializable(typeof(PolymarketPage<PolymarketOrder>))]
    [JsonSerializable(typeof(PolymarketOrderResult))]
    [JsonSerializable(typeof(ParameterCollection))]
    [JsonSerializable(typeof(IDictionary<string, object>))]
    [JsonSerializable(typeof(string[]))]
    [JsonSerializable(typeof(PolymarketCancelResult))]
    [JsonSerializable(typeof(PolymarketPage<PolymarketMarket>))]
    [JsonSerializable(typeof(PolymarketPage<PolymarketMarketDetails>))]
    [JsonSerializable(typeof(PolymarketMarketDetails))]
    [JsonSerializable(typeof(PolymarketTickSize))]
    [JsonSerializable(typeof(PolymarketNegRisk))]
    [JsonSerializable(typeof(PolymarketFeeRateBps))]
    [JsonSerializable(typeof(PolymarketSpread))]
    [JsonSerializable(typeof(PolymarketTradePrice))]
    [JsonSerializable(typeof(PolymarketTradePrice[]))]
    [JsonSerializable(typeof(PolymarketApiKeys))]
    [JsonSerializable(typeof(PolymarketClosedOnlyMode))]
    [JsonSerializable(typeof(PolymarketPage<PolymarketTrade>))]
    [JsonSerializable(typeof(PolymarketOrder))]
    [JsonSerializable(typeof(PolymarketOrderScoring))]
    [JsonSerializable(typeof(Dictionary<string, bool>))]
    [JsonSerializable(typeof(PolymarketNotification[]))]
    [JsonSerializable(typeof(PolymarketBalanceAllowance))]

    [JsonSerializable(typeof(PolymarketSocketRequest))]

    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(ulong))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class PolymarketSourceGenerationContext : JsonSerializerContext
    {
    }
}
