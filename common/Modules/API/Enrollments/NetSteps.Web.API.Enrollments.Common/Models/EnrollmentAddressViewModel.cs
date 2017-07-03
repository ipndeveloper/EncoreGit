using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentAddressViewModel
    /// </summary>
	public class EnrollmentAddressViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentAddressViewModel() { }

        #endregion

        #region Methods

        /// <summary>
        /// AddressLine1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2
        /// </summary>
		public string AddressLine2 { get; set; }

        /// <summary>
        /// AddressLine3
        /// </summary>
		public string AddressLine3 { get; set; }

        /// <summary>
        /// Attention
        /// </summary>
		public string Attention { get; set; }

        /// <summary>
        /// City
        /// </summary>
		public string City { get; set; }

        /// <summary>
        /// CountryID
        /// </summary>
		public int CountryID { get; set; }

        /// <summary>
        /// County
        /// </summary>
		public string County { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
		public string PostalCode { get; set; }

        /// <summary>
        /// State
        /// </summary>
		public string State { get; set; }        

        #endregion

    }
}
