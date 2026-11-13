using Polymarket.Net.Interfaces;
using CryptoExchange.Net;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var collection = new ServiceCollection();
collection.AddPolymarket();
var provider = collection.BuildServiceProvider();

var bookFactory = provider.GetRequiredService<IPolymarketOrderBookFactory>();

// Create and start the order book
var book = bookFactory.CreatePrediction("4153292802911610701832309484716814274802943278345248636922528170020319407796");
var result = await book.StartAsync();
if (!result.Success)
{
    Console.WriteLine(result);
    return;
}

// Create Spectre table
var table = new Table();
table.ShowRowSeparators = true;
table.AddColumn("Bid Quantity", x => { x.RightAligned(); })
     .AddColumn("Bid Price", x => { x.RightAligned(); })
     .AddColumn("Ask Price", x => { x.LeftAligned(); })
     .AddColumn("Ask Quantity", x => { x.LeftAligned(); });

for(var i = 0; i < 10; i++)
    table.AddEmptyRow();

await AnsiConsole.Live(table)
    .StartAsync(async ctx =>
    {
        while (true)
        {
            var snapshot = book.Book;
            for (var i = 0; i < 10; i++)
            {
                var bid = snapshot.bids.ElementAt(i);
                var ask = snapshot.asks.ElementAt(i);
                table.UpdateCell(i, 0, ExchangeHelpers.Normalize(bid.Quantity).ToString());
                table.UpdateCell(i, 1, ExchangeHelpers.Normalize(bid.Price).ToString());
                table.UpdateCell(i, 2, ExchangeHelpers.Normalize(ask.Price).ToString());
                table.UpdateCell(i, 3, ExchangeHelpers.Normalize(ask.Quantity).ToString());
            }

            ctx.Refresh();
            await Task.Delay(500);
        }
    });
