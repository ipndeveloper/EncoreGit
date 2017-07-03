using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountRenewal.Common.Models
{
    /// <summary>
    /// Renewal Product
    /// </summary>
    [DTO]
    public interface IProduct
    {
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
