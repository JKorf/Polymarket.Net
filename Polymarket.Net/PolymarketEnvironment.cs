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
        /// Chain id
        /// </summary>
        public uint ChainId { get; }

        /// <summary>
        /// Rest Clob API address
        /// </summary>
        public string ClobRestClientAddress { get; }
        
        /// <summary>
        /// Rest Gamma API address
        /// </summary>
        public string GammaRestClientAddress { get; }

        /// <summary>
        /// Rest Data API address
        /// </summary>
        public string DataRestClientAddress { get; }

        /// <summary>
        /// Socket Clob API address
        /// </summary>
        public string ClobSocketClientAddress { get; }

        /// <summary>
        /// Socket Sport API address
        /// </summary>
        public string SportSocketClientAddress { get; }

        /// <summary>
        /// Socket PolyBolt API address
        /// </summary>
        public string PolyBoltSocketClientAddress { get; }

        /// <summary>
        /// REST Perps API address
        /// </summary>
        public string PerpsRestClientAddress { get; }

        /// <summary>
        /// Socket Perps API address
        /// </summary>
        public string PerpsSocketClientAddress { get; }

        internal PolymarketEnvironment(
            string name,
            uint chainId,
            string clobRestAddress,
            string gammaRestAddress,
            string dataRestAddress,
            string clobStreamAddress,
            string sportStreamAddress,
            string polyBoltStreamAddress,
            string perpsRestAddress,
            string perpsSocketAddress) :
            base(name)
        {
            ChainId = chainId;
            ClobRestClientAddress = clobRestAddress;
            GammaRestClientAddress = gammaRestAddress;
            DataRestClientAddress = dataRestAddress;
            ClobSocketClientAddress = clobStreamAddress;
            SportSocketClientAddress = sportStreamAddress;
            PolyBoltSocketClientAddress = polyBoltStreamAddress;
            PerpsRestClientAddress = perpsRestAddress;
            PerpsSocketClientAddress = perpsSocketAddress;
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
             TradeEnvironmentNames.Testnet => Testnet,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name, Testnet.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static PolymarketEnvironment Live { get; }
            = new PolymarketEnvironment(TradeEnvironmentNames.Live,
                                     137,
                                     PolymarketApiAddresses.Default.ClobRestClientAddress,
                                     PolymarketApiAddresses.Default.GammaRestClientAddress,
                                     PolymarketApiAddresses.Default.DataRestClientAddress,
                                     PolymarketApiAddresses.Default.ClobSocketClientAddress,
                                     PolymarketApiAddresses.Default.SportsSocketClientAddress,
                                     PolymarketApiAddresses.Default.PolyBoltSocketClientAddress,
                                     PolymarketApiAddresses.Default.PerpsRestClientAddress,
                                     PolymarketApiAddresses.Default.PerpsSocketClientAddress);

        /// <summary>
        /// Test environment
        /// </summary>
        public static PolymarketEnvironment Testnet { get; }
            = new PolymarketEnvironment(TradeEnvironmentNames.Testnet,
                                     80002,
                                     PolymarketApiAddresses.Default.ClobRestClientAddress,
                                     PolymarketApiAddresses.Default.GammaRestClientAddress,
                                     PolymarketApiAddresses.Default.DataRestClientAddress,
                                     PolymarketApiAddresses.Default.ClobSocketClientAddress,
                                     PolymarketApiAddresses.Default.SportsSocketClientAddress,
                                     PolymarketApiAddresses.Default.PolyBoltSocketClientAddress,
                                     PolymarketApiAddresses.Default.PerpsRestClientAddress,
                                     PolymarketApiAddresses.Default.PerpsSocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <returns></returns>
        public static PolymarketEnvironment CreateCustom(
                        string name,
                        uint chainId,
                        string clobRestAddress,
                        string gammaRestAddress,
                        string dataRestAddress,
                        string clobSocketStreamsAddress,
                        string sportSocketStreamsAddress,
                        string polyBoltSocketStreamsAddress,
                        string perpsRestAddress,
                        string perpsSocketAddress)
            => new PolymarketEnvironment(
                name, 
                chainId, 
                clobRestAddress,
                gammaRestAddress, 
                dataRestAddress,
                clobSocketStreamsAddress,
                sportSocketStreamsAddress,
                polyBoltSocketStreamsAddress,
                perpsRestAddress,
                perpsSocketAddress
                );
    }
}
