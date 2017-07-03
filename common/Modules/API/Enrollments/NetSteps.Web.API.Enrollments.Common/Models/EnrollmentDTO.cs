using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.Enrollments.Common;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Order.Common;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentDTO
    /// </summary>
    public class EnrollmentDTO
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentDTO() { }

        #endregion

        #region Properties

        /// <summary>
        /// CreateAccount
        /// </summary>
        public bool CreateAccount { get; set; }

        /// <summary>
        /// CreateEnrollmentOrder
        /// </summary>
        public bool CreateEnrollmentOrder { get; set; }

        /// <summary>
        /// CreateProductSubscription
        /// </summary>
        public bool CreateProductSubscription { get; set; }

        /// <summary>
        /// CreateSiteSubscription
        /// </summary>
        public bool CreateSiteSubscription { get; set; }

        /// <summary>
        /// CreateUser
        /// </summary>
        public bool CreateUser { get; set; }

        /// <summary>
        /// AccountID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// LanguageID
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public IEnrollingAccount Account { get; set; }

        /// <summary>
        /// Order
        /// </summary>
		public IOrderCreate Order { get; set; }

        /// <summary>
        /// ProductSubscriptionOrder
        /// </summary>
        public IEnrollmentSubscriptionOrder ProductSubscriptionOrder { get; set; }

        /// <summary>
        /// SiteSubscriptionOrder
        /// </summary>
        public IEnrollmentSubscriptionOrder SiteSubscriptionOrder { get; set; }        

        #endregion

    }
}
