using System;
using System.Linq;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AccountTypeBusinessLogic
	{
		/// <summary>
		/// Controls whether this account type is eligible to send an enrollment completed email
		/// Default is yes.
		/// Clients can override this to determine if specific account types should not send the email.
		/// </summary>
		/// <param name="accountTypeID"></param>
		/// <returns></returns>
		public virtual bool CanSendEnrollmentCompletedEmail(int accountTypeID)
		{
			return true;
		}
	}
}
