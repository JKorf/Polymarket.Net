using CryptoExchange.Net.Authentication;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Enums;

var socketClient = new PolymarketSocketClient();
var tokenId = "TOKEN_ID";

// Public token stream. All handlers are optional; pass only the events you need.
var tokenSubscription = await socketClient.ClobApi.SubscribeToTokenUpdatesAsync(
    new[] { tokenId },
    onPriceChangeUpdate: update => Console.WriteLine($"Price update: {update.Data}"),
    onBookUpdate: update => Console.WriteLine($"Book update: {update.Data}"),
    onLastTradePriceUpdate: update => Console.WriteLine($"Last trade price: {update.Data}"),
    onTickSizeUpdate: update => Console.WriteLine($"Tick size update: {update.Data}"),
    onBestBidAskUpdate: update => Console.WriteLine($"Best bid/ask update: {update.Data}"));

if (!tokenSubscription.Success)
{
    Console.WriteLine($"Token subscription failed: {tokenSubscription.Error}");
    return;
}

// Platform stream for new market and market-resolved events.
var platformSubscription = await socketClient.ClobApi.SubscribeToPlatformUpdatesAsync(
    onNewMarketUpdate: update => Console.WriteLine($"New market: {update.Data}"),
    onMarketResolvedUpdate: update => Console.WriteLine($"Market resolved: {update.Data}"));

if (!platformSubscription.Success)
{
    await socketClient.UnsubscribeAsync(tokenSubscription.Data);
    Console.WriteLine($"Platform subscription failed: {platformSubscription.Error}");
    return;
}

// Sports updates are public as well.
var sportsSubscription = await socketClient.ClobApi.SubscribeToSportsUpdatesAsync(
    update => Console.WriteLine($"Sports update: {update.Data}"));

if (!sportsSubscription.Success)
{
    await socketClient.UnsubscribeAsync(platformSubscription.Data);
    await socketClient.UnsubscribeAsync(tokenSubscription.Data);
    Console.WriteLine($"Sports subscription failed: {sportsSubscription.Error}");
    return;
}

// Authenticated user stream. Configure a separate socket client with L1+L2 credentials.
var authenticatedSocketClient = new PolymarketSocketClient(options =>
{
    options.ApiCredentials = new PolymarketCredentials(
        new PolymarketL1Credential(
            SignType.Poly1271,
            "PRIVATE_KEY",
            "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS"),
        new HMACPassCredential(
            "L2_API_KEY",
            "L2_API_SECRET",
            "L2_API_PASSPHRASE"));
});

var userSubscription = await authenticatedSocketClient.ClobApi.SubscribeToUserUpdatesAsync(
    onOrderUpdate: update => Console.WriteLine($"Order update: {update.Data}"),
    onTradeUpdate: update => Console.WriteLine($"Trade update: {update.Data}"));

if (!userSubscription.Success)
{
    Console.WriteLine($"User subscription failed: {userSubscription.Error}");
}

Console.WriteLine("Press enter to unsubscribe.");
Console.ReadLine();

if (userSubscription.Success)
    await authenticatedSocketClient.UnsubscribeAsync(userSubscription.Data);

await socketClient.UnsubscribeAsync(sportsSubscription.Data);
await socketClient.UnsubscribeAsync(platformSubscription.Data);
await socketClient.UnsubscribeAsync(tokenSubscription.Data);
