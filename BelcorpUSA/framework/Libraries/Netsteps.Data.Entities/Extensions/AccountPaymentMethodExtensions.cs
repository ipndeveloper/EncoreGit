namespace NetSteps.Data.Entities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Author: John Egbert
	/// Description: AccountPaymentMethod Extensions
	/// Created: 05-24-2010
	/// </summary>
	public static class AccountPaymentMethodExtensions
	{
		public static AccountPaymentMethod GetByAccountPaymentMethodID(this IEnumerable<AccountPaymentMethod> accountPaymentMethods, int accountPaymentMethodID)
		{
			if (accountPaymentMethods == null)
			{
				throw new ArgumentNullException("accountPaymentMethods");
			}

			return accountPaymentMethods
				 .FirstOrDefault(apm => apm.AccountPaymentMethodID == accountPaymentMethodID);
		}

		public static AccountPaymentMethod GetDefaultAccountPaymentMethod(this IEnumerable<AccountPaymentMethod> accountPaymentMethods)
		{
			if (accountPaymentMethods == null)
			{
				throw new ArgumentNullException("accountPaymentMethods");
			}

			return accountPaymentMethods
				 .OrderByDescending(apm => apm.IsDefault)
				 .FirstOrDefault();
		}
	}
}
