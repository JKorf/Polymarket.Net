using Polymarket.Net;
using Polymarket.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Polymarket.Net.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Polymarket services
builder.Services.AddPolymarket();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddPolymarket(options =>
{
    options.ApiCredentials = new PolymarketCredentials()
        .WithL1(
            SignType.EOA, // Externally Owned Account wallet, when using an existing wallet to connect to Polymarket
            EthKey.FromPrivateKey("0x00.."), // The private key for the wallet
            "POLYMARKET_FUNDING_OR_DEPOSIT_ADDRESS")
        .WithL2(
            "KEY", // The L2 API key as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
            "SEC", // The L2 API secret as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
            "PASS"); // The L2 API passphrase as previously retrieved with `polymarketRestClient.ClobApi.Account.GetOrCreateApiCredentialsAsync()`
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{TokenId}", async ([FromServices] IPolymarketRestClient client, string tokenId) =>
{
    var result = await client.PredictionApi.ExchangeData.GetLastTradePriceAsync(tokenId);
    return result.Success
        ? Results.Ok(result.Data.LastTradePrice)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IPolymarketRestClient client) =>
{
    var result = await client.PredictionApi.Account.GetBalanceAllowanceAsync(AssetType.Collateral);
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();
