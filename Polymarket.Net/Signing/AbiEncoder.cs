using CryptoExchange.Net;
using Polymarket.Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Signing
{
    internal static class AbiEncoder
    {
        public static byte[] AbiValueEncodeString(string value)
        {
            var abiValueEncoded = InternalSha3Keccack.CalculateHash(Encoding.UTF8.GetBytes(value));
            return abiValueEncoded;
        }

        public static byte[] AbiValueEncodeBool(bool value)
            => AbiValueEncodeInt((byte)(value ? 1 : 0));

        public static byte[] AbiValueEncodeInt(byte value)
            => AbiValueEncodeBigInteger(false, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(short value)
            => AbiValueEncodeBigInteger(true, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(int value)
            => AbiValueEncodeBigInteger(true, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(long value)
            => AbiValueEncodeBigInteger(true, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(ushort value)
            => AbiValueEncodeBigInteger(false, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(uint value)
            => AbiValueEncodeBigInteger(false, new BigInteger(value));

        public static byte[] AbiValueEncodeInt(ulong value)
            => AbiValueEncodeBigInteger(false, new BigInteger(value));

        public static byte[] AbiValueEncodeBigInteger(bool signed, BigInteger value)
        {
            var result = new byte[32];
            if (signed && value < 0)
            {
                // Pad with FF
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = 0xFF;
                }
            }

            var t = value.ToByteArray();
            if (t.Length == 33)
            {
                var strip1 = new byte[32];
                Array.Copy(t, 1, strip1, 0, 32);
                t = strip1;
            }

            if (BitConverter.IsLittleEndian)
                t = t.AsEnumerable().Reverse().ToArray();

            t.CopyTo(result, result.Length - t.Length);
            return result;
        }

        public static byte[] AbiValueEncodeAddress(string value)
        {
            var result = new byte[32];
            var h = value.HexStringToBytes();
            h.CopyTo(result, result.Length - h.Length);
            return result;
        }

        public static byte[] AbiValueEncodeHexBytes(int length, string value)
            => AbiValueEncodeBytes(value.Length, value.HexStringToBytes());

        public static byte[] AbiValueEncodeBytes(int length, byte[] value)
        {
            if (length != 32)
                throw new Exception("Only 32 bytes size supported");

            if (value.Length == 32)
                return value;

            var result = new byte[32];
            value.CopyTo(result, result.Length - value.Length);
            return result;
        }
    }
}
