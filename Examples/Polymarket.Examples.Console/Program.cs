using Polymarket.Net.Clients;

// REST
var restClient = new PolymarketRestClient();
var tokenId = "4153292802911610701832309484716814274802943278345248636922528170020319407796";
var ticker = await restClient.PredictionApi.ExchangeData.GetLastTradePriceAsync(tokenId);
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get last trade price: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client last price: {ticker.Data.LastTradePrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new PolymarketSocketClient();
var subscription = await socketClient.PredictionApi.SubscribeToTokenUpdatesAsync([tokenId], onLastTradePriceUpdate: x =>
{
    Console.WriteLine(x.Data.Price);
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to token updates: {subscription.Error}");
    return;
}

Console.ReadLine();
