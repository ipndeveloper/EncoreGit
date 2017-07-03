using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Constants
	/// </summary>
	public class EnrollmentConstants
	{
		/// <summary>
		/// Account Type
		/// </summary>
		public enum AccountType
		{
			/// <summary>
			/// Distributor Account Type
			/// </summary>
			Distributor = 1,
			/// <summary>
			/// Prefered Customer Account Type
			/// </summary>
			PreferredCustomer = 2,
			/// <summary>
			/// Retail Customer Account Type
			/// </summary>
			RetailCustomer = 3,
			/// <summary>
			/// Employee Account Type
			/// </summary>
			Employee = 4,
			/// <summary>
			/// Prospect Account Type
			/// </summary>
			Prospect = 5
		}

		/// <summary>
		/// Gender
		/// </summary>
        public enum Gender
        {
			/// <summary>
			/// Male
			/// </summary>
            Male = 1,
			/// <summary>
			/// Female
			/// </summary>
            Female = 2        
        }

		/// <summary>
		/// Shipping Methods
		/// </summary>
        public enum ShippingMethod
        {
			/// <summary>
			/// FedEX Ground
			/// </summary>
            FedEXGrd = 1,
			/// <summary>
			/// FedEX 2 Day
			/// </summary>
        	FedEX2Day = 2,
			/// <summary>
			/// FedEX Standard Overnight
			/// </summary>
        	FedEXStandardOvernight = 3,
			/// <summary>
			/// FedEX Priority Overnight
			/// </summary>
        	FedEXPriorityOvernight = 4,
			/// <summary>
			/// FedEX Express Saver
			/// </summary>
        	FedEXEXpressSaver = 5,
			/// <summary>
			/// USPS
			/// </summary>
        	USPS = 6
        }

		/// <summary>
		/// Countrys
		/// </summary>
        public enum Country
        {
			/// <summary>
			/// United States
			/// </summary>
            USA = 1,
			/// <summary>
			/// Canada
			/// </summary>
            CAN	= 2,
			/// <summary>
			/// Australia
			/// </summary>
            AUS = 3,
			/// <summary>
			/// Great Britan
			/// </summary>
            GBR = 6,
			/// <summary>
			/// Ireland
			/// </summary>
            IRL = 7,
			/// <summary>
			/// Sweden
			/// </summary>
            SWE = 8,
			/// <summary>
			/// Netherlands
			/// </summary>
            NLD = 9,
			/// <summary>
			/// Belgium
			/// </summary>
            BEL = 10
        }

	}
}
