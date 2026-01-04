using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Polymarket.Net.Objects;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Signing;
using Polymarket.Net.Utils;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Polymarket.Net
{
    internal class PolymarketAuthenticationProvider : AuthenticationProvider<PolymarketCredentials>
    {
        private bool _updatedBaseBytes;

        private const string _l1SignMessage = "This message attests that I control the given wallet";

        private static IStringMessageSerializer _serializer = new SystemTextJsonMessageSerializer(PolymarketExchange._serializerContext);

        private static readonly byte[] _defaultBytes = [0x64, 0x5f, 0x51, 0x63, 0x50, 0x34, 0x39, 0x42, 0x6e, 0x42, 0x7a, 0x68, 0x5a, 0x72, 0x75, 0x30, 0x6b, 0x35, 0x67, 0x53, 0x30, 0x4a, 0x52, 0x70, 0x72, 0x2d, 0x59, 0x52, 0x61, 0x63, 0x50, 0x56, 0x77, 0x61, 0x62, 0x7a, 0x6f, 0x6a, 0x67, 0x79, 0x37, 0x46, 0x34, 0x3d];

        public string PublicAddress => ((PolymarketCredentials)_credentials).PublicAddress;
        public override ApiCredentialsType[] SupportedCredentialTypes => [ApiCredentialsType.Hmac];

        public PolymarketAuthenticationProvider(PolymarketCredentials credentials) : base(credentials)
        {
#warning some way to better differentiate public/private key creds and apikey/secret/pass creds. Maybe just a 
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.Authenticated)
                return;

            if ((requestConfig.Path.Equals("/auth/api-key") && requestConfig.Method == HttpMethod.Post)
                || (requestConfig.Path.Equals("/auth/derive-api-key") && requestConfig.Method == HttpMethod.Get))
            {
                // L1 authentication
                // SignL1Neth(requestConfig);
                SignL1Custom(requestConfig);
            }
            else
            {
                // L2 authentication
                SignL2(apiClient, requestConfig);
            }
        }

        private void SignL1Custom(RestRequestConfiguration requestConfig)
        {
            var timestamp = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow);
            requestConfig.GetPositionParameters().TryGetValue("nonce", out var nonce);

            var typeRaw = GetEncodedClobAuth(timestamp.ToString()!, nonce == null ? 0 : (long)nonce);
            var msg = LightEip712TypedDataEncoder.EncodeTypedDataRaw(typeRaw);
            var keccakSigned = InternalSha3Keccack.CalculateHash(msg);

            var signature = SignHash(keccakSigned);
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("POLY_ADDRESS", _credentials.PublicAddress);
            requestConfig.Headers.Add("POLY_SIGNATURE", signature.ToLowerInvariant());
            requestConfig.Headers.Add("POLY_TIMESTAMP", timestamp.Value.ToString());
            requestConfig.Headers.Add("POLY_NONCE", nonce?.ToString() ?? "0");
        }

        //private void SignL1Neth(RestRequestConfiguration requestConfig)
        //{
        //    var timestamp = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow);
        //    requestConfig.GetPositionParameters().TryGetValue("nonce", out var nonce);

        //    var typedData = new UsdClassTransfer
        //    {
        //        Address = ApiKey,
        //        Message = _l1SignMessage,
        //        Nonce = nonce == null ? 0 : (long)nonce,
        //        Timestamp = timestamp.Value.ToString()
        //    };
        //    var msg = EncodeEip721Neth(typedData, 137, "ClobAuth");
        //    var keccakSigned = BytesToHexString(InternalSha3Keccack.CalculateHash(msg));

        //    var messageBytes = ConvertHexStringToByteArray(keccakSigned);
        //    var sign = new MessageSigner().SignAndCalculateV(messageBytes, new EthECKey(_credentials.Secret));
        //    var signature = sign.CreateStringSignature();

        //    requestConfig.Headers ??= new Dictionary<string, string>();
        //    requestConfig.Headers.Add("POLY_ADDRESS", ApiKey);
        //    requestConfig.Headers.Add("POLY_SIGNATURE", signature);
        //    requestConfig.Headers.Add("POLY_TIMESTAMP", timestamp.Value.ToString());
        //    requestConfig.Headers.Add("POLY_NONCE", nonce?.ToString() ?? "0");
        //}

        private void SignL2(RestApiClient client, RestRequestConfiguration requestConfig)
        {
            if (!_updatedBaseBytes)
            {
                var sanitized = _credentials.L2Secret!.Replace('-', '+').Replace('_', '/');
                _sBytes = Convert.FromBase64String(sanitized);
                _updatedBaseBytes = true;
            }

            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("POLY_ADDRESS", "0xb3EF3f8Af99268f3FD71bEF7e4d6F6b8a6217B0C");//_credentials.PublicAddress);
            requestConfig.Headers.Add("POLY_API_KEY", _credentials.L2ApiKey!);
            requestConfig.Headers.Add("POLY_PASSPHRASE", _credentials.L2Pass!);

            var timestamp = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow);
            requestConfig.Headers.Add("POLY_TIMESTAMP", timestamp.Value.ToString());

            var signData = timestamp + requestConfig.Method.ToString() + requestConfig.Path;
            if (requestConfig.Method == HttpMethod.Post || requestConfig.Method == HttpMethod.Delete)
            {
                var body = (requestConfig.BodyParameters == null || requestConfig.BodyParameters.Count == 0) ? string.Empty : GetSerializedBody(_serializer, requestConfig.BodyParameters);
                signData += body;
                requestConfig.SetBodyContent(body);
            }

            var signature = SignHMACSHA256(signData, SignOutputType.Base64);
            signature = signature.Replace('+', '-').Replace('/', '_');
            requestConfig.Headers.Add("POLY_SIGNATURE", signature);


            var options = (PolymarketRestOptions)client.ClientOptions;
            var key = string.IsNullOrWhiteSpace(options.BuilderApiKey) ? "019b4abf-48c5-7c0b-ae70-c01221c5ead1" : options.BuilderApiKey;
            var pass = string.IsNullOrWhiteSpace(options.BuilderPass) ? "1ee7b124de529ff26079bfe79da3e6d1997cdcfd481ebb7468a019ef3c6767d5" : options.BuilderPass;
            byte[] secret;
            if (!string.IsNullOrWhiteSpace(options.BuilderSecret))
                secret = Convert.FromBase64String(options.BuilderSecret!.Replace('-', '+').Replace('_', '/'));
            else
                secret = _defaultBytes;

            using var encryptor = new System.Security.Cryptography.HMACSHA256(secret);
            var resultBytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(signData));
            var builderSignature = BytesToBase64String(resultBytes);
            builderSignature = builderSignature.Replace('+', '-').Replace('/', '_');

            requestConfig.Headers.Add("POLY_BUILDER_API_KEY", key!);
            requestConfig.Headers.Add("POLY_BUILDER_PASSPHRASE", pass!);
            requestConfig.Headers.Add("POLY_BUILDER_SIGNATURE", builderSignature);
            requestConfig.Headers.Add("POLY_BUILDER_TIMESTAMP", timestamp.Value.ToString());
        }

        //public byte[] EncodeEip721Neth(
        //    object msg,
        //    int chainId,
        //    string primaryType)
        //{
        //    var typeDef = new TypedData<DomainWithNameVersionAndChainId>
        //    {
        //        Domain = new DomainWithNameVersionAndChainId
        //        {
        //            Name = "ClobAuthDomain",
        //            Version = "1",
        //            ChainId = chainId
        //        },
        //        Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(DomainWithNameVersionAndChainId), messageType),
        //        PrimaryType = primaryType,
        //    };

        //    var signer = new Eip712TypedDataSigner();
        //    var encodedData = signer.EncodeTypedData((UsdClassTransfer)msg, typeDef);
        //    return encodedData;
        //}

        //[Struct("ClobAuth")]
        //public class UsdClassTransfer
        //{
        //    [Parameter("address", "address", 1)]
        //    public string Address { get; set; }
        //    [Parameter("string", "timestamp", 2)]
        //    public string Timestamp { get; set; }
        //    [Parameter("uint256", "nonce", 3)]
        //    public long Nonce { get; set; }
        //    [Parameter("string", "message", 4)]
        //    public string Message { get; set; }
        //}

        //private Nethereum.ABI.EIP712.TypedDataRaw GetTypeDataRawNeth(ParameterCollection order)
        //{
        //    return new Nethereum.ABI.EIP712.TypedDataRaw
        //    {
        //        PrimaryType = "Order",
        //        DomainRawValues = new Nethereum.ABI.EIP712.MemberValue[]
        //        {
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "string", Value = "Polymarket CTF Exchange" },
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "string", Value = "1" },
        //            //new MemberValue { TypeName = "uint256", Value = 137 },
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = 80002 },
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "address", Value = PolymarketUtils.ExchangeContract }
        //        },
        //        Message = new Nethereum.ABI.EIP712.MemberValue[]
        //        {
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = order["salt"].ToString() },
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "address", Value = order["maker"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "address", Value = order["signer"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "address", Value = order["taker"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["tokenId"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["makerAmount"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["takerAmount"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["expiration"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["nonce"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint256", Value = (string)order["feeRateBps"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint8", Value = (byte)(int)order["side"]},
        //            new Nethereum.ABI.EIP712.MemberValue { TypeName = "uint8", Value = (byte)(int)order["signatureType"]}
        //        },
        //        Types = new Dictionary<string, Nethereum.ABI.EIP712.MemberDescription[]>
        //        {
        //            { "EIP712Domain",
        //                new Nethereum.ABI.EIP712.MemberDescription[]
        //                {
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "name", Type = "string" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "version", Type = "string" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "chainId", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "verifyingContract", Type = "address" }
        //                }
        //            },
        //            { "Order",
        //                new Nethereum.ABI.EIP712.MemberDescription[]
        //                {
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "salt", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "maker", Type = "address" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "signer", Type = "address" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "taker", Type = "address" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "tokenId", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "makerAmount", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "takerAmount", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "expiration", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "nonce", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "feeRateBps", Type = "uint256" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "side", Type = "uint8" },
        //                    new Nethereum.ABI.EIP712.MemberDescription { Name = "signatureType", Type = "uint8" },
        //                }
        //            }
        //        }
        //    };
        //}


        //public string GetOrderSignatureNethWorking(ParameterCollection parameters)
        //{
        //    var typeRaw = GetTypeDataRawNeth(parameters);
        //    var signer = new Eip712TypedDataSigner();
        //    var encodedTypedData = signer.EncodeTypedData(typeRaw.ToJson());
        //    var hashedEncodedTypedData = Sha3Keccack.Current.CalculateHash(encodedTypedData);
        //    var key = new EthECKey(_credentials.L1PrivateKey);
        //    var signature = key.SignAndCalculateV(hashedEncodedTypedData);
        //    return EthECDSASignature.CreateStringSignature(signature); // Correct signature
        //}

        public string GetOrderSignature(ParameterCollection parameters, int chainId, bool negativeRisk)
        {
            var typeRaw = GetTypeDataRawCustom(parameters, chainId, negativeRisk);
            var msg = LightEip712TypedDataEncoder.EncodeTypedDataRaw(typeRaw);
            var orderHashBytes = InternalSha3Keccack.CalculateHash(msg);
            return SignHash(orderHashBytes);
        }

        private string SignHash(byte[] hash)
        {
            using var secp256k1 = new Secp256k1();
            var signature = new byte[65];
            secp256k1.SignRecoverable(signature, hash, HexToBytesString(_credentials.L1PrivateKey));

            var signCompact = new byte[64];
            secp256k1.SignatureSerializeCompact(signCompact, signature);
            var hexCompactR = BytesToHexString(new ArraySegment<byte>(signCompact, 0, 32));
            var hexCompactS = BytesToHexString(new ArraySegment<byte>(signCompact, 32, 32));
            var hexCompactV = BytesToHexString([(byte)(signature[64] + 27)]);

            var result = "0x" + hexCompactR.PadLeft(64, '0') +
                   hexCompactS.PadLeft(64, '0') +
                   hexCompactV;
            return result;
        }

        private string GetContract(ParameterCollection order, int chainId, bool negativeRisk)
        {
            if (chainId == 137)            
                return negativeRisk ? PolymarketContractsConfig.PolygonNegRiskConfig.Exchange : PolymarketContractsConfig.PolygonConfig.Exchange;            
            else            
                return negativeRisk ? PolymarketContractsConfig.AmoyNegRiskConfig.Exchange : PolymarketContractsConfig.AmoyConfig.Exchange;            
        }

        private Signing.TypedDataRaw GetTypeDataRawCustom(ParameterCollection order, int chainId, bool negativeRisk)
        {
            return new Signing.TypedDataRaw
            {
                PrimaryType = "Order",
                DomainRawValues = new Signing.MemberValue[]
                {
                    new Signing.MemberValue { TypeName = "string", Value = "Polymarket CTF Exchange" },
                    new Signing.MemberValue { TypeName = "string", Value = "1" },
                    new Signing.MemberValue { TypeName = "uint256", Value = chainId },
                    new Signing.MemberValue { TypeName = "address", Value = GetContract(order, chainId, negativeRisk) }
                },
                Message = new Signing.MemberValue[]
                {
                    new Signing.MemberValue { TypeName = "uint256", Value = order["salt"].ToString()! },
                    new Signing.MemberValue { TypeName = "address", Value = order["maker"]},
                    new Signing.MemberValue { TypeName = "address", Value = order["signer"]},
                    new Signing.MemberValue { TypeName = "address", Value = order["taker"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["tokenId"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["makerAmount"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["takerAmount"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["expiration"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["nonce"]},
                    new Signing.MemberValue { TypeName = "uint256", Value = (string)order["feeRateBps"]},
                    new Signing.MemberValue { TypeName = "uint8", Value = (byte)((string)order["side"] == "BUY" ? 0 : 1)},
                    new Signing.MemberValue { TypeName = "uint8", Value = (byte)(int)order["signatureType"]}
                },
                Types = new Dictionary<string, Signing.MemberDescription[]>
                {
                    { "EIP712Domain",
                        new Signing.MemberDescription[]
                        {
                            new Signing.MemberDescription { Name = "name", Type = "string" },
                            new Signing.MemberDescription { Name = "version", Type = "string" },
                            new Signing.MemberDescription { Name = "chainId", Type = "uint256" },
                            new Signing.MemberDescription { Name = "verifyingContract", Type = "address" }
                        }
                    },
                    { "Order",
                        new Signing.MemberDescription[]
                        {
                            new Signing.MemberDescription { Name = "salt", Type = "uint256" },
                            new Signing.MemberDescription { Name = "maker", Type = "address" },
                            new Signing.MemberDescription { Name = "signer", Type = "address" },
                            new Signing.MemberDescription { Name = "taker", Type = "address" },
                            new Signing.MemberDescription { Name = "tokenId", Type = "uint256" },
                            new Signing.MemberDescription { Name = "makerAmount", Type = "uint256" },
                            new Signing.MemberDescription { Name = "takerAmount", Type = "uint256" },
                            new Signing.MemberDescription { Name = "expiration", Type = "uint256" },
                            new Signing.MemberDescription { Name = "nonce", Type = "uint256" },
                            new Signing.MemberDescription { Name = "feeRateBps", Type = "uint256" },
                            new Signing.MemberDescription { Name = "side", Type = "uint8" },
                            new Signing.MemberDescription { Name = "signatureType", Type = "uint8" },
                        }
                    }
                }
            };
        }

        public TypedDataRaw GetEncodedClobAuth(string timestamp, long nonce)
        {
            return new Signing.TypedDataRaw
            {
                PrimaryType = "ClobAuth",
                DomainRawValues = new Signing.MemberValue[]
                {
                    new Signing.MemberValue { TypeName = "string", Value = "ClobAuthDomain" },
                    new Signing.MemberValue { TypeName = "string", Value = "1" },
                    new Signing.MemberValue { TypeName = "uint256", Value = 137 },
                },
                Message = new Signing.MemberValue[]
                {
                    new Signing.MemberValue { TypeName = "address", Value = _credentials.Key },
                    new Signing.MemberValue { TypeName = "string", Value = timestamp },
                    new Signing.MemberValue { TypeName = "uint256", Value = nonce },
                    new Signing.MemberValue { TypeName = "string", Value = _l1SignMessage }
                },
                Types = new Dictionary<string, Signing.MemberDescription[]>
                {
                    { "EIP712Domain",
                        new Signing.MemberDescription[]
                        {
                            new Signing.MemberDescription { Name = "name", Type = "string" },
                            new Signing.MemberDescription { Name = "version", Type = "string" },
                            new Signing.MemberDescription { Name = "chainId", Type = "uint256" }
                        }
                    },
                    { "ClobAuth",
                        new Signing.MemberDescription[]
                        {
                            new Signing.MemberDescription { Name = "address", Type = "address" },
                            new Signing.MemberDescription { Name = "timestamp", Type = "string" },
                            new Signing.MemberDescription { Name = "nonce", Type = "uint256" },
                            new Signing.MemberDescription { Name = "message", Type = "string" }
                        }
                    }
                }
            };
        }
    }
}
