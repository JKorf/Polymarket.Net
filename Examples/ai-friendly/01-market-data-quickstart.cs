using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

// Public market/event data does not require API credentials.
// Keep one client for the application instead of creating a new client per request.
var client = new PolymarketRestClient();

// Gamma API is the easiest way to discover human-readable markets and metadata.
var markets = await client.GammaApi.GetMarketsAsync(closed: false, limit: 5);
if (!markets.Success)
{
    Console.WriteLine($"Could not load Gamma markets: {markets.Error}");
    return;
}

foreach (var market in markets.Data)
{
    Console.WriteLine($"{market.Question} | condition id: {market.MarketId}");
}

// Search is also exposed through GammaApi.
var search = await client.GammaApi.SearchAsync("election", limitPerType: 3);
if (!search.Success)
{
    Console.WriteLine($"Search failed: {search.Error}");
    return;
}

Console.WriteLine("Search completed.");

// CLOB API exposes token-level trading data. Orders and order books use token ids,
// not symbols such as BTCUSDT.
var clobMarkets = await client.ClobApi.ExchangeData.GetMarketsAsync();
if (!clobMarkets.Success)
{
    Console.WriteLine($"Could not load CLOB markets: {clobMarkets.Error}");
    return;
}

var firstToken = clobMarkets.Data.Data
    .SelectMany(x => x.Tokens)
    .FirstOrDefault();

if (firstToken == null)
{
    Console.WriteLine("No CLOB tokens were returned.");
    return;
}

var tokenId = firstToken.TokenId;

var book = await client.ClobApi.ExchangeData.GetOrderBookAsync(tokenId);
if (!book.Success)
{
    Console.WriteLine($"Could not load order book: {book.Error}");
    return;
}

Console.WriteLine($"Order book for {tokenId}: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");

var midpoint = await client.ClobApi.ExchangeData.GetMidpointPriceAsync(tokenId);
if (!midpoint.Success)
{
    Console.WriteLine($"Could not load midpoint: {midpoint.Error}");
    return;
}

Console.WriteLine($"Midpoint loaded for {tokenId}");

var buyPrice = await client.ClobApi.ExchangeData.GetPriceAsync(tokenId, OrderSide.Buy);
if (!buyPrice.Success)
{
    Console.WriteLine($"Could not load buy price: {buyPrice.Error}");
    return;
}

Console.WriteLine($"Buy price loaded for {tokenId}");
