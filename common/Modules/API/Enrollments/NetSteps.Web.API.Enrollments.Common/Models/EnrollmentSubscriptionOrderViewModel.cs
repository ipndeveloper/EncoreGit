using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentSubscriptionOrderViewModel
    /// </summary>
	public class EnrollmentSubscriptionOrderViewModel : EnrollmentOrderViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentSubscriptionOrderViewModel() { }

        #endregion

        #region Properties

        /// <summary>
        /// AutoshipScheduleID
        /// </summary>
        public int AutoshipScheduleID { get; set; }

        /// <summary>
        /// MarketID
        /// </summary>
		public int MarketID { get; set; }

        /// <summary>
        /// Url
        /// </summary>
		public string Url { get; set; }
        
        #endregion

    }
}
