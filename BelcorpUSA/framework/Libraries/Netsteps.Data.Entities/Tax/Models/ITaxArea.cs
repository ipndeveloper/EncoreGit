using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
    [DTO]
    public interface ITaxArea
    {
        /// <summary>
        /// Tax provider specific identifier for a tax area.
        /// </summary>
        string TaxAreaID { get; set; }
        /// <summary>
        /// Indicates whether the provider indicated confidence level.
        /// </summary>
        bool HasConfidenceLevel { get; set; }
        /// <summary>
        /// Tax provider's confidence in the tax area matching the address info.
        /// </summary>
        int ConfidenceLevel { get; set; }
        /// <summary>
        /// Applicable tax jurisdictions.
        /// </summary>
        IEnumerable<IJurisdiction> Jurisdictions { get; set; }
    }
}
