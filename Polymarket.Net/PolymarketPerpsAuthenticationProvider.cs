using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Web3;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Utils;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Polymarket.Net
{
    internal class PolymarketPerpsAuthenticationProvider : AuthenticationProvider<PolymarketCredentials>
    {
        public override string Key => ApiCredentials.Perps.Key;

        public PolymarketPerpsAuthenticationProvider(PolymarketCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            if (requestConfig.BodyParameters?.ContainsKey("sig") == true)
                // already has signature
                return;

            // Private reads
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("polymarket-proxy", ApiCredentials.Perps.ProxyKey.PublicAddress);
            requestConfig.Headers.Add("polymarket-secret", ApiCredentials.Perps.Secret);
            
        }

        public void AuthenticateOpRequest(RestApiClient apiClient, Parameters parameters, EthKey signKey)
        {
            var hash = HashOperation(apiClient, parameters);
            var time = GetMillisecondTimestampLong(apiClient);
            var salt = ExchangeHelpers.RandomLong(8);
            var payload = GetOperationPayload(time, salt, hash, ((PolymarketRestOptions)apiClient.ClientOptions).Environment.ChainId);
            var msg = CeEip712TypedDataEncoder.EncodeTypedDataRaw(payload);
            var keccakSigned = CeSha3Keccack.CalculateHash(msg);
            var signature = signKey.Sign(keccakSigned);

            parameters.Add("sig", signature);
            parameters.Add("salt", salt);
            parameters.Add("ts", time);
        }


        public byte[] HashOperation(RestApiClient apiClient, Parameters parameters)
        {
            var op = (Parameters)parameters["op"];
            var type = op["type"];
            var args = op["args"];
            var signPayload = new List<object>() { type };
            if (args is Parameters opArgs)
            {
                signPayload.Add(GetValues(opArgs));
            }
            else if(args is Parameters[] opArgsArray)
            {
                var nestedArgs = new List<object>();
                foreach (var opArgsItem in opArgsArray)
                {
                    nestedArgs.Add(GetValues(opArgsItem));
                }
                signPayload.Add(nestedArgs.ToArray());
            }
            else if (args is Array arrayArgs)
            {
                signPayload.Add(arrayArgs.Cast<object>().ToArray());
            }

            var packed = new MessagePackConverter().
                Pack(signPayload);

            return CeSha3Keccack.CalculateHash(packed);
        }

        private object[] GetValues(Parameters parameters)
        {
            if (parameters.BodyValue != null)
            {
                return (parameters.BodyValue as Array)!.Cast<object>().ToArray();
            }

            return parameters.Select(x => x.Value).ToArray();
        }

        public void AuthenticateCreateProxyRequest(RestApiClient apiClient, Parameters parameters, EthKey signKey, string proxyAddress)
        {
            var time = GetMillisecondTimestampLong(apiClient);
            var salt = ExchangeHelpers.RandomLong(8);

            var expireArg = (long)((Parameters)((Parameters)parameters["op"])["args"])["expiry"];

            var proxyPayload = GetCreateProxyPayload(time, expireArg, salt, proxyAddress,((PolymarketRestOptions)apiClient.ClientOptions).Environment.ChainId);
            var msg = CeEip712TypedDataEncoder.EncodeTypedDataRaw(proxyPayload);
            var keccakSigned = CeSha3Keccack.CalculateHash(msg);
            var signature = signKey.Sign(keccakSigned);

            parameters.Add("sig", signature);
            parameters.Add("salt", salt);
            parameters.Add("ts", time);
        }

        public CeTypedDataRaw GetOperationPayload(long ts, long salt, byte[] opHash, uint chainId)
        {
            return new CeTypedDataRaw
            {
                PrimaryType = "Op",
                DomainRawValues = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "string", Value = "Polymarket" },
                    new CeMemberValue { TypeName = "string", Value = "1" },
                    new CeMemberValue { TypeName = "uint256", Value = chainId },
                },
                Message = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "bytes32", Value = opHash },
                    new CeMemberValue { TypeName = "uint64", Value = salt },
                    new CeMemberValue { TypeName = "uint64", Value = ts }
                },
                Types = new Dictionary<string, CeMemberDescription[]>
                {
                    { "EIP712Domain",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "name", Type = "string" },
                            new CeMemberDescription { Name = "version", Type = "string" },
                            new CeMemberDescription { Name = "chainId", Type = "uint256" }
                        }
                    },
                    { "Op",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "data", Type = "bytes32" },
                            new CeMemberDescription { Name = "salt", Type = "uint64" },
                            new CeMemberDescription { Name = "ts", Type = "uint64" }
                        }
                    }
                }
            };
        }

        public CeTypedDataRaw GetCreateProxyPayload(long ts, long exp, long salt, string proxyAddress, uint chainId)
        {
            return new CeTypedDataRaw
            {
                PrimaryType = "CreateProxy",
                DomainRawValues = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "string", Value = "Polymarket" },
                    new CeMemberValue { TypeName = "string", Value = "1" },
                    new CeMemberValue { TypeName = "uint256", Value = chainId },
                },
                Message = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "address", Value = proxyAddress },
                    new CeMemberValue { TypeName = "uint64", Value = exp },
                    new CeMemberValue { TypeName = "uint64", Value = salt },
                    new CeMemberValue { TypeName = "uint64", Value = ts }
                },
                Types = new Dictionary<string, CeMemberDescription[]>
                {
                    { "EIP712Domain",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "name", Type = "string" },
                            new CeMemberDescription { Name = "version", Type = "string" },
                            new CeMemberDescription { Name = "chainId", Type = "uint256" }
                        }
                    },
                    { "CreateProxy",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "addr", Type = "address" },
                            new CeMemberDescription { Name = "exp", Type = "uint64" },
                            new CeMemberDescription { Name = "salt", Type = "uint64" },
                            new CeMemberDescription { Name = "ts", Type = "uint64" }
                        }
                    }
                }
            };
        }

        public CeTypedDataRaw GetDeleteProxyPayload(long ts, long salt, string proxyAddress, uint chainId)
        {
            return new CeTypedDataRaw
            {
                PrimaryType = "DeleteProxy",
                DomainRawValues = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "string", Value = "Polymarket" },
                    new CeMemberValue { TypeName = "string", Value = "1" },
                    new CeMemberValue { TypeName = "uint256", Value = chainId },
                },
                Message = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "address", Value = proxyAddress },
                    new CeMemberValue { TypeName = "uint64", Value = salt },
                    new CeMemberValue { TypeName = "uint64", Value = ts }
                },
                Types = new Dictionary<string, CeMemberDescription[]>
                {
                    { "EIP712Domain",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "name", Type = "string" },
                            new CeMemberDescription { Name = "version", Type = "string" },
                            new CeMemberDescription { Name = "chainId", Type = "uint256" }
                        }
                    },
                    { "DeleteProxy",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "addr", Type = "address" },
                            new CeMemberDescription { Name = "salt", Type = "uint64" },
                            new CeMemberDescription { Name = "ts", Type = "uint64" }
                        }
                    }
                }
            };
        }
    }
}
