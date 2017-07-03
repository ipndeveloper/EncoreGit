using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Downline.Service.Entities
{
	public class Account
	{
		[Key]
		public int AccountID { get; set; }

		public short AccountTypeID { get; set; }

		public short AccountStatusID { get; set; }

		public string AccountNumber { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string EmailAddress { get; set; }

		public DateTime? EnrollmentDateUTC { get; set; }

		public DateTime? NextRenewalUTC { get; set; }
	}
}
