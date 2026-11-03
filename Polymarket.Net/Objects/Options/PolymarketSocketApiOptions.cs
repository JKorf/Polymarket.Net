using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Polymarket Perps Socket API options
    /// </summary>
    public class PolymarketSocketPerpsApiOptions : SocketApiOptions
    {
        /// <summary>
        /// Builder code
        /// </summary>
        public string? BuilderCode { get; set; }
        /// <summary>
        /// Fee percentage
        /// </summary>
        public decimal? BuilderFeePercentage { get; set; } = 0.01m;

        internal PolymarketSocketPerpsApiOptions Set(PolymarketSocketPerpsApiOptions targetOptions)
        {
            targetOptions = base.Set(targetOptions);
            targetOptions.BuilderCode = BuilderCode;
            targetOptions.BuilderFeePercentage = BuilderFeePercentage;
            return targetOptions;
        }
    }
}
