using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Polymarket.Net.Objects.Models;
using System;
using System.Net.WebSockets;
using Polymarket.Net.Objects.Internal;
using System.Linq;

namespace Polymarket.Net.Clients.MessageHandlers
{
    internal class PolymarketSocketPerpsMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PolymarketPlatform._serializerContext;

        public PolymarketSocketPerpsMessageHandler()
        {
            AddTopicMapping<PolymarketPerpsSocketUpdate>(x => string.Join("", x.Channel.Split(new [] { "::" }, StringSplitOptions.None).Skip(1)));
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                ForceIfFound = true,
                TypeIdentifierCallback = x => x.FieldValue("id")!,
            },
             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("ch"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("ch")!.Split(':')[0],
            }
        ];
    }
}
