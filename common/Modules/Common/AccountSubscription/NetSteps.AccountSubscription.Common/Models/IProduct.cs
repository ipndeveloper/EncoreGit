using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Product
    /// </summary>
    [DTO]
    public interface IProduct
    {
        /// <summary>
        /// IntervalCount
        /// </summary>
        int IntervalCount { get; set; }

        /// <summary>
        /// ProductID
        /// </summary>
        int ProductID { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        string SKU { get; set; }
    }
}
