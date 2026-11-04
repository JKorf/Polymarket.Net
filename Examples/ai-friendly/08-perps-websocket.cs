using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Web3;
using Polymarket.Net;
using Polymarket.Net.Clients;

// Get the numeric SymbolId from PerpsApi.ExchangeData.GetSymbolsAsync().
if (args.Length != 1 || !long.TryParse(args[0], out var symbolId))
{
    Console.WriteLine("Usage: dotnet run -- <instrumentId>");
    return;
}

using var socketClient = new PolymarketSocketClient();
var subscriptions = new List<UpdateSubscription>();

// Private streams use the same registered Perps proxy and secret as REST.
// Leave credentials unset to run only the public streams.
var proxyPrivateKey = Environment.GetEnvironmentVariable("PERPS_PROXY_PRIVATE_KEY");
var secret = Environment.GetEnvironmentVariable("PERPS_API_SECRET");
if (!string.IsNullOrEmpty(proxyPrivateKey) && !string.IsNullOrEmpty(secret))
{
    socketClient.SetApiCredentials(new PolymarketCredentials().WithPerps(
        secret: secret,
        signerKey: null,
        proxyKey: new EthKey(privateKey: proxyPrivateKey)));
}

// Keep every successful subscription so failures and normal shutdown both clean up.
try
{
    var ticker = await socketClient.PerpsApi.ExchangeData.SubscribeToTickerUpdatesAsync(
        symbolId, update => Console.WriteLine($"Ticker: {update.Data}"));
    if (!ticker.Success)
    {
        Console.WriteLine($"Ticker subscription failed: {ticker.Error}");
        return;
    }
    subscriptions.Add(ticker.Data);

    var trades = await socketClient.PerpsApi.ExchangeData.SubscribeToTradeUpdatesAsync(
        symbolId, update => Console.WriteLine($"Received {update.Data.Length} trades."));
    if (!trades.Success)
    {
        Console.WriteLine($"Trade subscription failed: {trades.Error}");
        return;
    }
    subscriptions.Add(trades.Data);

    if (!string.IsNullOrEmpty(proxyPrivateKey) && !string.IsNullOrEmpty(secret))
    {
        var orders = await socketClient.PerpsApi.Trading.SubscribeToOrderUpdatesAsync(
            update => Console.WriteLine($"User order: {update.Data}"));
        if (!orders.Success)
        {
            Console.WriteLine($"Order subscription failed: {orders.Error}");
            return;
        }
        subscriptions.Add(orders.Data);
    }

    Console.WriteLine("Press enter to unsubscribe.");
    Console.ReadLine();
}
finally
{
    foreach (var subscription in subscriptions)
        await socketClient.UnsubscribeAsync(subscription);
}
