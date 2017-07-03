using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentOrderViewModel
    /// </summary>
	public class EnrollmentOrderViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentOrderViewModel() 
        {
            this.Products = new List<ProductViewModel>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// AccountID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// AccountTypeID
        /// </summary>
		public int AccountTypeID { get; set; }

        /// <summary>
        /// CurrencyID
        /// </summary>
		public int CurrencyID { get; set; }

        /// <summary>
        /// OrderTypeID
        /// </summary>
		public int OrderTypeID { get; set; }

        /// <summary>
        /// SiteID
        /// </summary>
		public int SiteID { get; set; }

        /// <summary>
        /// Products
        /// </summary>
        public IList<ProductViewModel> Products { get; set; }
                
        #endregion

    }
}
