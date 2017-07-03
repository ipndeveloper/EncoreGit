using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Profile of a propay disbursement.
	/// </summary>
	public interface IPropayDisbursementProfile : IDisbursementProfile
	{
		/// <summary>
		/// Gets the propay account number.
		/// </summary>
		/// <value>
		/// The propay account number.
		/// </value>
		int PropayAccountNumber { get; }
	}
}
