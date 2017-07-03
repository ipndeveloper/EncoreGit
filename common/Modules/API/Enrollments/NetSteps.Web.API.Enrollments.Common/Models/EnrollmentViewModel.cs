using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentViewModel
    /// </summary>
    public class EnrollmentViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentViewModel()
        {
            this.Account = new EnrollmentAccountViewModel();
            this.Order = new EnrollmentOrderViewModel();
            this.User = new EnrollmentUserViewModel();
            this.ProductSubscriptionOrder = new EnrollmentSubscriptionOrderViewModel();
            this.SiteSubscriptionOrder = new EnrollmentSubscriptionOrderViewModel();
            this.BillingAddress = new EnrollmentAddressViewModel();
            this.MainAddress = new EnrollmentAddressViewModel();
            this.ShippingAddress = new EnrollmentAddressViewModel();
            this.BillingProfile = new EnrollmentBillingProfileViewModel();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Account
        /// </summary>
        public EnrollmentAccountViewModel Account { get; set; }

        /// <summary>
        /// BillingAddress
        /// </summary>
        public EnrollmentAddressViewModel BillingAddress { get; set; }

        /// <summary>
        /// BillingProfile
        /// </summary>
        public EnrollmentBillingProfileViewModel BillingProfile { get; set; }

        /// <summary>
        /// MainAddress
        /// </summary>
        public EnrollmentAddressViewModel MainAddress { get; set; }

        /// <summary>
        /// Order
        /// </summary>
        public EnrollmentOrderViewModel Order { get; set; }

        /// <summary>
        /// ProductSubscriptionOrder
        /// </summary>
        public EnrollmentSubscriptionOrderViewModel ProductSubscriptionOrder { get; set; }

        /// <summary>
        /// ShippingAddress
        /// </summary>
        public EnrollmentAddressViewModel ShippingAddress { get; set; }

        /// <summary>
        /// SiteSubscriptionOrder
        /// </summary>
        public EnrollmentSubscriptionOrderViewModel SiteSubscriptionOrder { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public EnrollmentUserViewModel User { get; set; }

        #endregion

    }
}
