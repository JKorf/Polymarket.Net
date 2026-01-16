using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Polymarket.Net.Objects.Models;

namespace Polymarket.Net.Clients.MessageHandlers
{
    internal class PolymarketSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PolymarketExchange._serializerContext;

        public PolymarketSocketSpotMessageHandler()
        {
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
        ];
    }
}
