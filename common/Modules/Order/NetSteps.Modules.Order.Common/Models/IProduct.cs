using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
    /// <summary>
    /// Product
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
        string Sku { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        int Quantity { get; set; } 
    }
}
