using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentBillingProfileViewModel
    /// </summary>
	public class EnrollmentBillingProfileViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentBillingProfileViewModel() { }

        #endregion

        #region Properties

        /// <summary>
        /// CCNumber
        /// </summary>
        public string CCNumber { get; set; }

        /// <summary>
        /// CVV
        /// </summary>
		public string CVV { get; set; }

        /// <summary>
        /// ExpirationDate
        /// </summary>
		public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// NameOnCard
        /// </summary>
		public string NameOnCard { get; set; }

        /// <summary>
        /// PaymentTypeID
        /// </summary>
		public int PaymentTypeID { get; set; }
        
        #endregion

    }
}
