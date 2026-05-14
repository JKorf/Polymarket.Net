using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Authentication.Signing;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Polymarket.Net.Enums;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.Objects.Sockets;
using Polymarket.Net.Utils;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Polymarket.Net
{
    internal class PolymarketAuthenticationProvider : AuthenticationProvider<PolymarketCredentials>
    {
        private const string _l1SignMessage = "This message attests that I control the given wallet";
        private byte[]? _hmacBytes;

        const string _orderTypeString = "Order(uint256 salt,address maker,address signer,uint256 tokenId,uint256 makerAmount," +
                                        "uint256 takerAmount,uint8 side,uint8 signatureType,uint256 timestamp,bytes32 metadata,bytes32 builder)";

        private static byte[] _orderTypeHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes(_orderTypeString));
        private static byte[] _domainTypeHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes("EIP712Domain(string name,string version,uint256 chainId,address verifyingContract)"));

        private static byte[] _ctfExchangeNameHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes("Polymarket CTF Exchange"));
        private static byte[] _ctfExchangeVersionHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes("2"));

        private static byte[] _soladyTypeHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes($"TypedDataSign(Order contents,string name,string version,uint256 chainId,"
                                                                                                + "address verifyingContract,bytes32 salt)"
                                                                                                + $"{_orderTypeString}"));
        private static byte[] _depositWalletNameHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes("DepositWallet"));
        private static byte[] _depositWalletVersionHash = CeSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes("1"));
        private static byte[] _depositWalletSaltHash = new byte[32];

        private static byte[] _abiDomainTypeHash = CeAbiEncoder.AbiValueEncodeBytes(32, _domainTypeHash);
        private static byte[] _abiExchangeName = CeAbiEncoder.AbiValueEncodeBytes(32, _ctfExchangeNameHash);
        private static byte[] _abiExchangeVersion = CeAbiEncoder.AbiValueEncodeBytes(32, _ctfExchangeVersionHash);

        private static IStringMessageSerializer _serializer = new SystemTextJsonMessageSerializer(PolymarketPlatform._serializerContext);

        public override string Key => ApiCredentials.L1.Key;

        public PolymarketAuthenticationProvider(PolymarketCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.Authenticated)
                return;

            if ((requestConfig.Path.Equals("/auth/api-key") && requestConfig.Method == HttpMethod.Post)
                || (requestConfig.Path.Equals("/auth/derive-api-key") && requestConfig.Method == HttpMethod.Get))
            {
                // L1 authentication
                SignL1Custom(requestConfig, ((PolymarketRestOptions)apiClient.ClientOptions).Environment.ChainId);
            }
            else
            {
                if (ApiCredentials.L2 == null)
                    throw new InvalidOperationException("Layer 2 credentials required");

                // L2 authentication
                SignL2(apiClient, requestConfig);
            }
        }

        public override Query? GetAuthenticationQuery(SocketApiClient apiClient, SocketConnection connection, Dictionary<string, object?>? context = null)
        {
            if (ApiCredentials.L2 == null)
                throw new InvalidOperationException("Layer 2 credentials required");

            return new PolymarketInitialQuery<object>("USER", ApiCredentials.L2.Key, ApiCredentials.L2.Secret!, ApiCredentials.L2.Pass!);
        }

        private void SignL1Custom(RestRequestConfiguration requestConfig, uint chainId)
        {
            var timestamp = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow);
            requestConfig.GetPositionParameters().TryGetValue("nonce", out var nonce);

            var typeRaw = GetEncodedClobAuth(timestamp.ToString()!, nonce == null ? 0 : (long)nonce, chainId);
            var msg = CeEip712TypedDataEncoder.EncodeTypedDataRaw(typeRaw);
            var keccakSigned = CeSha3Keccack.CalculateHash(msg);

            var signature = SignHash(keccakSigned);
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("POLY_ADDRESS", ApiCredentials.L1.GetPublicAddress());
            requestConfig.Headers.Add("POLY_SIGNATURE", signature.ToLowerInvariant());
            requestConfig.Headers.Add("POLY_TIMESTAMP", timestamp.Value.ToString());
            requestConfig.Headers.Add("POLY_NONCE", nonce?.ToString() ?? "0");
        }

        private void SignL2(RestApiClient client, RestRequestConfiguration requestConfig)
        {
            _hmacBytes ??= Convert.FromBase64String(ApiCredentials.L2!.Secret!.Replace('-', '+').Replace('_', '/'));
            var timestamp = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow);
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("POLY_ADDRESS", ApiCredentials.L1.Key!);
            requestConfig.Headers.Add("POLY_API_KEY", ApiCredentials.L2!.Key!);
            requestConfig.Headers.Add("POLY_PASSPHRASE", ApiCredentials.L2.Pass!);
            requestConfig.Headers.Add("POLY_TIMESTAMP", timestamp.Value.ToString());

            var signData = timestamp + requestConfig.Method.ToString() + requestConfig.Path;
            if (requestConfig.Method == HttpMethod.Post || requestConfig.Method == HttpMethod.Delete)
            {
                var body = (requestConfig.BodyParameters == null || requestConfig.BodyParameters.Count == 0) ? string.Empty : GetSerializedBody(_serializer, requestConfig.BodyParameters);
                signData += body;
                requestConfig.SetBodyContent(body);
            }

            string signature;
            using (var hmac = new HMACSHA256(_hmacBytes))
                signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signData)));

            requestConfig.Headers.Add("POLY_SIGNATURE", signature.Replace('+', '-').Replace('/', '_'));
        }

        public string GetOrderSignature(ParameterCollection parameters, uint chainId, bool negativeRisk)
        {
            var signType = (int)parameters["signatureType"];
            if (signType == (int)SignType.Poly1271)
            {
                var abiChainId = CeAbiEncoder.AbiValueEncodeBigInteger(false, new BigInteger(chainId));
                var abiContractAddress = CeAbiEncoder.AbiValueEncodeAddress(GetContract(parameters, chainId, negativeRisk));
                var domainSeparatorBytes = CombineBytes(_abiDomainTypeHash, _abiExchangeName, _abiExchangeVersion, abiChainId, abiContractAddress);
                var appDomainSepBytes = CeSha3Keccack.CalculateHash(domainSeparatorBytes);
                var appDomainSep = BytesToHexString(appDomainSepBytes);

                var abiTypeHash = CeAbiEncoder.AbiValueEncodeBytes(32, _orderTypeHash);
                var abiSalt = CeAbiEncoder.AbiValueEncodeBigInteger(false, new BigInteger((ulong)parameters["salt"]));
                var abiMaker = CeAbiEncoder.AbiValueEncodeAddress((string)parameters["maker"]);
                var abiSigner = CeAbiEncoder.AbiValueEncodeAddress((string)parameters["signer"]);
                var abiTokenId = CeAbiEncoder.AbiValueEncodeBigInteger(false, BigInteger.Parse((string)parameters["tokenId"]));
                var abiMakerAmount = CeAbiEncoder.AbiValueEncodeBigInteger(false, BigInteger.Parse((string)parameters["makerAmount"]));
                var abiTakerAmount = CeAbiEncoder.AbiValueEncodeBigInteger(false, BigInteger.Parse((string)parameters["takerAmount"]));
                var abiSide = CeAbiEncoder.AbiValueEncodeInt((byte)((string)parameters["side"] == "BUY" ? 0 : 1));
                var abiSignType = CeAbiEncoder.AbiValueEncodeInt((int)parameters["signatureType"]);
                var abiTimestamp = CeAbiEncoder.AbiValueEncodeBigInteger(false, BigInteger.Parse((string)parameters["timestamp"]));
                var abiMetadata = CeAbiEncoder.AbiValueEncodeBytes(32, ((string)parameters["metadata"]).HexStringToBytes());
                var abiBuilder = CeAbiEncoder.AbiValueEncodeBytes(32, ((string)parameters["builder"]).HexStringToBytes());
                var allBytes = CombineBytes(abiTypeHash, abiSalt, abiMaker, abiSigner, abiTokenId, abiMakerAmount, abiTakerAmount, 
                                            abiSide, abiSignType, abiTimestamp, abiMetadata, abiBuilder);
                var orderDataHashBytes = CeSha3Keccack.CalculateHash(allBytes);
                var orderDataHash = BytesToHexString(orderDataHashBytes);

                var abi2TypeHash = CeAbiEncoder.AbiValueEncodeBytes(32, _soladyTypeHash);
                var abi2Content = CeAbiEncoder.AbiValueEncodeBytes(32, orderDataHashBytes);
                var abi2DepNameHash = CeAbiEncoder.AbiValueEncodeBytes(32, _depositWalletNameHash);
                var abi2DepNameVersion = CeAbiEncoder.AbiValueEncodeBytes(32, _depositWalletVersionHash);
                var abi2ChainId = CeAbiEncoder.AbiValueEncodeInt(chainId);
                var abi2Signer = CeAbiEncoder.AbiValueEncodeAddress((string)parameters["signer"]);
                var abi2DepSalt = CeAbiEncoder.AbiValueEncodeBytes(32, _depositWalletSaltHash);
                var allbytes2 = CombineBytes(abi2TypeHash, abi2Content, abi2DepNameHash, abi2DepNameVersion, abi2ChainId, abi2Signer, abi2DepSalt);
                var signedTypeDataBytes = CeSha3Keccack.CalculateHash(allbytes2);
                var signedTypeData = BytesToHexString(signedTypeDataBytes);

                var toDigest = CombineBytes(new byte[] { 0x19, 0x01 }, appDomainSepBytes, signedTypeDataBytes);
                var digest = CeSha3Keccack.CalculateHash(toDigest);
                var signed = SignHash(digest);

                var lenHex = _orderTypeString.Length.ToString("x").PadLeft(4, '0');
                var result = $"{signed}{appDomainSep}{orderDataHash}{BytesToHexString(Encoding.UTF8.GetBytes(_orderTypeString))}{lenHex}";
                return result;
            }
            else
            {
                var typeRaw = GetTypeDataRawCustom(parameters, chainId, negativeRisk);
                var msg = CeEip712TypedDataEncoder.EncodeTypedDataRaw(typeRaw);
                var orderHashBytes = CeSha3Keccack.CalculateHash(msg);
                return SignHash(orderHashBytes);
            }
        }

        private string SignHash(byte[] hash)
        {
            (var signature, var recover) = Secp256k1.SignRecoverable(hash, HexToBytesString(ApiCredentials.L1.PrivateKey));
            var hexCompactR = BytesToHexString(new ArraySegment<byte>(signature, 0, 32));
            var hexCompactS = BytesToHexString(new ArraySegment<byte>(signature, 32, 32));
            var hexCompactV = BytesToHexString([(byte)(recover + 27)]);

            var result = "0x" + hexCompactR.PadLeft(64, '0') +
                   hexCompactS.PadLeft(64, '0') +
                   hexCompactV;
            return result;
        }

        private string GetContract(ParameterCollection order, uint chainId, bool negativeRisk)
        {
            if (chainId == 137)            
                return negativeRisk ? PolymarketContractsConfig.PolygonNegRiskConfig.Exchange : PolymarketContractsConfig.PolygonConfig.Exchange;            
            else if (chainId == 80002)
                return negativeRisk ? PolymarketContractsConfig.AmoyNegRiskConfig.Exchange : PolymarketContractsConfig.AmoyConfig.Exchange;
            else
                throw new InvalidOperationException("Unknown chainId: " + chainId);
        }

        private CeTypedDataRaw GetTypeDataRawCustom(ParameterCollection order, uint chainId, bool negativeRisk)
        {
            return new CeTypedDataRaw
            {
                PrimaryType = "Order",
                DomainRawValues = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "string", Value = "Polymarket CTF Exchange" },
                    new CeMemberValue { TypeName = "string", Value = "2" },
                    new CeMemberValue { TypeName = "uint256", Value = chainId },
                    new CeMemberValue { TypeName = "address", Value = GetContract(order, chainId, negativeRisk) }
                },
                Message = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "uint256", Value = order["salt"].ToString()! },
                    new CeMemberValue { TypeName = "address", Value = order["maker"]},
                    new CeMemberValue { TypeName = "address", Value = order["signer"]},
                    new CeMemberValue { TypeName = "uint256", Value = (string)order["tokenId"]},
                    new CeMemberValue { TypeName = "uint256", Value = (string)order["makerAmount"]},
                    new CeMemberValue { TypeName = "uint256", Value = (string)order["takerAmount"]},
                    new CeMemberValue { TypeName = "uint8", Value = (byte)((string)order["side"] == "BUY" ? 0 : 1)},
                    new CeMemberValue { TypeName = "uint8", Value = (byte)(int)order["signatureType"]},
                    new CeMemberValue { TypeName = "uint256", Value = order["timestamp"]},
                    new CeMemberValue { TypeName = "bytes32", Value = ((string)order["metadata"]).HexStringToBytes()},
                    new CeMemberValue { TypeName = "bytes32", Value = ((string)order["builder"]).HexStringToBytes()},
                },
                Types = new Dictionary<string, CeMemberDescription[]>
                {
                    { "EIP712Domain",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "name", Type = "string" },
                            new CeMemberDescription { Name = "version", Type = "string" },
                            new CeMemberDescription { Name = "chainId", Type = "uint256" },
                            new CeMemberDescription { Name = "verifyingContract", Type = "address" }
                        }
                    },
                    { "Order",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "salt", Type = "uint256" },
                            new CeMemberDescription { Name = "maker", Type = "address" },
                            new CeMemberDescription { Name = "signer", Type = "address" },
                            new CeMemberDescription { Name = "tokenId", Type = "uint256" },
                            new CeMemberDescription { Name = "makerAmount", Type = "uint256" },
                            new CeMemberDescription { Name = "takerAmount", Type = "uint256" },
                            new CeMemberDescription { Name = "side", Type = "uint8" },
                            new CeMemberDescription { Name = "signatureType", Type = "uint8" },
                            new CeMemberDescription { Name = "timestamp", Type = "uint256" },
                            new CeMemberDescription { Name = "metadata", Type = "bytes32" },
                            new CeMemberDescription { Name = "builder", Type = "bytes32" },
                        }
                    }
                }
            };
        }

        public CeTypedDataRaw GetEncodedClobAuth(string timestamp, long nonce, uint chainId)
        {
            return new CeTypedDataRaw
            {
                PrimaryType = "ClobAuth",
                DomainRawValues = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "string", Value = "ClobAuthDomain" },
                    new CeMemberValue { TypeName = "string", Value = "1" },
                    new CeMemberValue { TypeName = "uint256", Value = chainId },
                },
                Message = new CeMemberValue[]
                {
                    new CeMemberValue { TypeName = "address", Value = ApiCredentials.L1.GetPublicAddress() },
                    new CeMemberValue { TypeName = "string", Value = timestamp },
                    new CeMemberValue { TypeName = "uint256", Value = nonce },
                    new CeMemberValue { TypeName = "string", Value = _l1SignMessage }
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
                    { "ClobAuth",
                        new CeMemberDescription[]
                        {
                            new CeMemberDescription { Name = "address", Type = "address" },
                            new CeMemberDescription { Name = "timestamp", Type = "string" },
                            new CeMemberDescription { Name = "nonce", Type = "uint256" },
                            new CeMemberDescription { Name = "message", Type = "string" }
                        }
                    }
                }
            };
        }

        private byte[] CombineBytes(params byte[][] byteArrays)
        {
            var result = new byte[byteArrays.Sum(b => b.Length)];
            var written = 0;
            foreach (var byteArray in byteArrays)
            {
                Buffer.BlockCopy(byteArray, 0, result, written, byteArray.Length);
                written += byteArray.Length;
            }
            return result;
        }
    }
}
