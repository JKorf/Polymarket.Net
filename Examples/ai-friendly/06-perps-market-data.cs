using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

// Public Perps market data does not require credentials. Reuse the client.
using var client = new PolymarketRestClient();

// Discover numeric instrument ids rather than guessing an id or a trading symbol.
var symbols = await client.PerpsApi.ExchangeData.GetSymbolsAsync(symbolType: InstrumentType.Perpetual);
if (!symbols.Success)
{
    Console.WriteLine($"Could not load instruments: {symbols.Error}");
    return;
}

foreach (var symbol in symbols.Data)
    Console.WriteLine($"{symbol.Symbol}: id {symbol.SymbolId}, minimum notional {symbol.MinNotional}");

var instrument = symbols.Data.FirstOrDefault();
if (instrument == null)
{
    Console.WriteLine("No perpetual instruments were returned.");
    return;
}

var symbolId = instrument.SymbolId;
var tickers = await client.PerpsApi.ExchangeData.GetTickersAsync(symbolId);
if (!tickers.Success)
{
    Console.WriteLine($"Could not load tickers: {tickers.Error}");
    return;
}

Console.WriteLine($"Loaded {tickers.Data.Length} tickers for {instrument.Symbol}.");

// Supported depths are 10, 100, 500 and 1000.
var book = await client.PerpsApi.ExchangeData.GetOrderBookAsync(symbolId, depth: 10);
if (!book.Success)
{
    Console.WriteLine($"Could not load order book: {book.Error}");
    return;
}

Console.WriteLine($"Book: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");

// Use UTC for history requests. Paged history results expose Data, More and Cursor.
var klines = await client.PerpsApi.ExchangeData.GetKlinesAsync(
    symbolId, KlineInterval.OneHour, startTime: DateTime.UtcNow.AddDays(-1));
if (!klines.Success)
{
    Console.WriteLine($"Could not load candles: {klines.Error}");
    return;
}

Console.WriteLine($"Loaded {klines.Data.Data.Length} candles; more available: {klines.Data.More}");

var funding = await client.PerpsApi.ExchangeData.GetFundingRateHistoryAsync(
    symbolId, startTime: DateTime.UtcNow.AddDays(-1));
if (!funding.Success)
{
    Console.WriteLine($"Could not load funding: {funding.Error}");
    return;
}

Console.WriteLine($"Loaded {funding.Data.Data.Length} funding rates.");
