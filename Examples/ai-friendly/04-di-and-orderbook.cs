using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Polymarket.Net;
using Polymarket.Net.Enums;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Interfaces.Clients;

var services = new ServiceCollection();

services.AddLogging();
services.AddPolymarket(options =>
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

using var provider = services.BuildServiceProvider();

var restClient = provider.GetRequiredService<IPolymarketRestClient>();
var socketClient = provider.GetRequiredService<IPolymarketSocketClient>();
var orderBookFactory = provider.GetRequiredService<IPolymarketOrderBookFactory>();
var userClientProvider = provider.GetRequiredService<IPolymarketUserClientProvider>();

var time = await restClient.ClobApi.ExchangeData.GetServerTimeAsync();
if (!time.Success)
{
    Console.WriteLine(time.Error);
    return;
}

Console.WriteLine($"Server time: {time.Data}");

// Local order books use CLOB token ids.
var tokenId = "TOKEN_ID";
var orderBook = orderBookFactory.CreateClob(tokenId);

// StartAsync connects the socket and builds the local book. This is commented so
// the example is safe to copy without a real token id.
// var start = await orderBook.StartAsync();
// if (!start.Success) { Console.WriteLine(start.Error); return; }

Console.WriteLine($"Created local order book for {tokenId}: {orderBook.Symbol}");

// The user client provider lets services manage separate credentials per user.
userClientProvider.InitializeUserClient(
    "alice",
    new PolymarketCredentials()
        .WithL1(SignType.Poly1271, "ALICE_PRIVATE_KEY", "ALICE_POLYMARKET_ADDRESS")
        .WithL2("ALICE_L2_KEY", "ALICE_L2_SECRET", "ALICE_L2_PASS"));

var aliceRestClient = userClientProvider.GetRestClient("alice");
var aliceSocketClient = userClientProvider.GetSocketClient("alice");

Console.WriteLine(aliceRestClient != null && aliceSocketClient != null
    ? "User clients initialized."
    : "User clients not initialized.");

// Keep the concrete socket client for unsubscribe calls when you create subscriptions.
Console.WriteLine(socketClient.ClobApi != null ? "Socket client ready." : "Socket client unavailable.");
