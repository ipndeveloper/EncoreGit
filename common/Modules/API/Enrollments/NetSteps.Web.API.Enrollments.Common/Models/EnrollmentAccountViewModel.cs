using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentAccountViewModel
    /// </summary>
	public class EnrollmentAccountViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentAccountViewModel() { }

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
        /// DateOfBirth
        /// </summary>
		public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Email
        /// </summary>
		public string Email { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
		public string FirstName { get; set; }

        /// <summary>
        /// GenderID
        /// </summary>
		public int GenderID { get; set; }

        /// <summary>
        /// IsEntity
        /// </summary>
		public bool IsEntity { get; set; }

        /// <summary>
        /// LanguageID
        /// </summary>
		public int LanguageID { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
		public string LastName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
		public string MiddleName { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
		public string PhoneNumber { get; set; }

        /// <summary>
        /// Placement
        /// </summary>
		public int Placement { get; set; }

        /// <summary>
        /// SponsorID
        /// </summary>
		public int SponsorID { get; set; }

        /// <summary>
        /// TaxExempt
        /// </summary>
		public bool TaxExempt { get; set; }

        /// <summary>
        /// TaxNumber
        /// </summary>
		public string TaxNumber { get; set; }

        #endregion

    }
}
