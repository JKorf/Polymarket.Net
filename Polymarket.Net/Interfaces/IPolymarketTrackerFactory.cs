using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;

namespace Polymarket.Net.Interfaces
{
    /// <summary>
    /// Tracker factory
    /// </summary>
    public interface IPolymarketTrackerFactory : ITrackerFactory
    {
        /// <summary>
        /// Create a new Perps user data tracker
        /// </summary>
        /// <param name="userIdentifier">User identifier</param>
        /// <param name="config">Configuration</param>
        /// <param name="credentials">Credentials</param>
        /// <param name="environment">Environment</param>
        IUserFuturesDataTracker CreateUserPerpsDataTracker(string userIdentifier, PolymarketCredentials credentials, FuturesUserDataTrackerConfig? config = null, PolymarketEnvironment? environment = null);
        
        /// <summary>
        /// Create a new Perps user data tracker
        /// </summary>
        /// <param name="config">Configuration</param>
        IUserFuturesDataTracker CreateUserPerpsDataTracker(FuturesUserDataTrackerConfig? config = null);
    }
}
