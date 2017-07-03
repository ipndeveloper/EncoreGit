using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Methods of disbursments available.
	/// </summary>
	public enum DisbursementMethodKind
	{
		/// <summary>
		/// Check disbursement
		/// </summary>
		Check = 1,
		/// <summary>
		/// EFT disbursement
		/// </summary>
		EFT = 2,
		//HSBCBank = 3,
		//HSBCDebit = 4
		/// <summary>
		/// HyperWallet disbursement
		/// </summary>
		HyperWallet = 5,
		/// <summary>
		/// Payoneer disbursement
		/// </summary>
		Payoneer = 6,
		/// <summary>
		/// ProPay disbursement
		/// </summary>
		ProPay = 7
	}
}
