using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Order.Common.Models
{
    /// <summary>
    /// Order View Model
    /// </summary>
    public class OrderViewModel
    {

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderViewModel() { }

        #endregion

        #region Properties

        /// <summary>
        /// AccountID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// CurrencyID
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// SiteID
        /// </summary>
        public int? SiteID { get; set; }

        /// <summary>
        /// AccountTypeID
        /// </summary>
        public short AccountTypeID { get; set; }

        /// <summary>
        /// OrderTypeID
        /// </summary>
        public short OrderTypeID { get; set; }

        /// <summary>
        /// ParentOrderID
        /// </summary>
        public int? ParentOrderID { get; set; }

        /// <summary>
        /// ShippingMethodID
        /// </summary>
        public int? ShippingMethodID { get; set; }

        /// <summary>
        /// List of Products
        /// </summary>
        public IList<ProductViewModel> Products { get; set; }
		
		/// <summary>
		/// Flag to tell if we are getting a quote or not
		/// </summary>
		public bool GetQuote { get; set; }

        #endregion

    }
}
