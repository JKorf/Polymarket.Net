using CryptoExchange.Net.Objects;
using Polymarket.Net.Objects;

namespace Polymarket.Net
{
    /// <summary>
    /// Polymarket environments
    /// </summary>
    public class PolymarketEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest Clob API address
        /// </summary>
        public string ClobRestClientAddress { get; }
        /// <summary>
        /// Rest Gamma API address
        /// </summary>
        public string GammaRestClientAddress { get; }

        /// <summary>
        /// Socket API address
        /// </summary>
        public string SocketClientAddress { get; }

        internal PolymarketEnvironment(
            string name,
            string clobRestAddress,
            string gammaRestAddress,
            string streamAddress) :
            base(name)
        {
            ClobRestClientAddress = clobRestAddress;
            GammaRestClientAddress = gammaRestAddress;
            SocketClientAddress = streamAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PolymarketEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the Polymarket environment by name
        /// </summary>
        public static PolymarketEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static PolymarketEnvironment Live { get; }
            = new PolymarketEnvironment(TradeEnvironmentNames.Live,
                                     PolymarketApiAddresses.Default.ClobRestClientAddress,
                                     PolymarketApiAddresses.Default.GammaRestClientAddress,
                                     PolymarketApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <returns></returns>
        public static PolymarketEnvironment CreateCustom(
                        string name,
                        string clobRestAddress,
                        string gammaRestAddress,
                        string spotSocketStreamsAddress)
            => new PolymarketEnvironment(name, clobRestAddress, gammaRestAddress, spotSocketStreamsAddress);
    }
}
