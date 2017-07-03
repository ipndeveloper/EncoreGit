using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Enrollments.Common.Models
{
    /// <summary>
    /// Default implementation of EnrollmentUserViewModel
    /// </summary>
    public class EnrollmentUserViewModel
    {

        #region Constructor(s)

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public EnrollmentUserViewModel() { }

        #endregion

        #region Properties

        /// <summary>
        /// AccountID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// LanguageID
        /// </summary>
        public int LanguageID { get; set; }
        
        #endregion

    }
}
