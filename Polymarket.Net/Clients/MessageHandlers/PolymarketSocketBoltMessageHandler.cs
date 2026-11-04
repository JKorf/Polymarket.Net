using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Polymarket.Net.Objects.Models;
using System;
using System.Net.WebSockets;
using Polymarket.Net.Objects.Internal;

namespace Polymarket.Net.Clients.MessageHandlers
{
    internal class PolymarketSocketBoltMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PolymarketPlatform._serializerContext;

        public PolymarketSocketBoltMessageHandler()
        {
            AddTopicMapping<PolymarketBoltUpdate<PolymarketBoltSnapshotUpdate<PolymarketPriceUpdate[]>>>(x => x.Payload.Symbol);
            AddTopicMapping<PolymarketBoltUpdate<PolymarketEquityPriceUpdate>>(x => x.Payload.Symbol);
            AddTopicMapping<PolymarketBoltUpdate<PolymarketPriceUpdate>>(x => x.Payload.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("rid"),
                ],
                ForceIfFound = true,
                TypeIdentifierCallback = x => x.FieldValue("rid")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel"),
                    new PropertyFieldReference("snapshot"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("channel")! + "snapshot",
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("channel")!,
            },
        ];

        protected override string? GetTypeIdentifierNonJson(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return null;
        }
    }
}
