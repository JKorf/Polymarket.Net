using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymarket.Net.Objects.Options
{
    /// <summary>
    /// Polymarket REST API options
    /// </summary>
    public  class PolymarketRestApiOptions : RestApiOptions
    {
        /// <summary>
        /// Builder code
        /// </summary>
        public string? BuilderCode { get; set; }

        internal PolymarketRestApiOptions Set(PolymarketRestApiOptions targetOptions)
        {
            targetOptions = base.Set(targetOptions);
            targetOptions.BuilderCode = BuilderCode;
            return targetOptions;
        }   
    }

    /// <summary>
    /// Polymarket Perps REST API options
    /// </summary>
    public class PolymarketRestPerpsApiOptions : PolymarketRestApiOptions
    {
        /// <summary>
        /// Fee percentage
        /// </summary>
        public decimal? BuilderFeePercentage { get; set; } = 0.01m;

        internal PolymarketRestPerpsApiOptions Set(PolymarketRestPerpsApiOptions targetOptions)
        {
            targetOptions = (PolymarketRestPerpsApiOptions)base.Set(targetOptions);
            targetOptions.BuilderFeePercentage = BuilderFeePercentage;
            return targetOptions;
        }
    }
}
