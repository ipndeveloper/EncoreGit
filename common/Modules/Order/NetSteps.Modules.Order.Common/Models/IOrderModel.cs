using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
    /// <summary>
    /// Order Model
    /// </summary>
    [DTO]
    public interface IOrderModel
    {
        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }
        /// <summary>
        /// CurrencyID
        /// </summary>
        int CurrencyID { get; set; }
        /// <summary>
        /// SiteID
        /// </summary>
        int? SiteID { get; set; }
        /// <summary>
        /// AccountTypeID
        /// </summary>
        short AccountTypeID { get; set; }
        /// <summary>
        /// OrderTypeID
        /// </summary>
        short OrderTypeID { get; set; }
        /// <summary>
        /// ParentOrderID
        /// </summary>
        int? ParentOrderID { get; set; }
        /// <summary>
        /// List of Products
        /// </summary>
        IList<IProduct> Products { get; set; }
    }
}
