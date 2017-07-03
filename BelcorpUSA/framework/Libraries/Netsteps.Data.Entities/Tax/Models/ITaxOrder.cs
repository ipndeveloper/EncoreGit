using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
    public enum TaxOrderState
    {
        Unknown = 0,
        /// <summary>
        /// Tax information is available and recorded in the
        /// calculation.
        /// </summary>
        Complete = 1,
        /// <summary>
        /// Tax information is unavailable.
        /// </summary>
        Unavailable = 2,
        /// <summary>
        /// Tax information is not applicable (party is tax-exempt, etc.)
        /// </summary>
        NotApplicable = 3,
    }

    [DTO]
    public interface ITaxOrder
    {
		string OrderID { get; set; }
		string CountryCode { get; set; }
        TaxOrderState Status { get; set; }
        List<ITaxCustomer> Customers { get; set; }
    }
}


