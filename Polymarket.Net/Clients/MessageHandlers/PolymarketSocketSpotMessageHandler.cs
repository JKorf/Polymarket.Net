using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Polymarket.Net.Objects.Models;
using System.Linq;
using System;
using System.Net.WebSockets;

namespace Polymarket.Net.Clients.MessageHandlers
{
    internal class PolymarketSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PolymarketPlatform._serializerContext;

        public PolymarketSocketSpotMessageHandler()
        {
            AddTopicMapping<PolymarketBookUpdate>(x => x.TokenId);
            AddTopicMapping<PolymarketBookUpdate[]>(x => x.First().TokenId);
            AddTopicMapping<PolymarketLastTradePriceUpdate>(x => x.TokenId);
            AddTopicMapping<PolymarketTickSizeUpdate>(x => x.TokenId);
            AddTopicMapping<PolymarketBestBidAskUpdate>(x => x.TokenId);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event_type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("event_type")!,
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event_type") { Depth = 2 },
                ],
                TypeIdentifierCallback = x => x.FieldValue("event_type")! + "_snapshot",
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("gameId"),
                ],
                StaticIdentifier = "sports"
            }
        ];

        protected override string? GetTypeIdentifierNonJson(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return null;
        }
    }
}
