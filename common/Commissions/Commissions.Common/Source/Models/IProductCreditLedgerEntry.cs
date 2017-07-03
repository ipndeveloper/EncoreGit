
using NetSteps.Commissions.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Product Credit Ledger Entry
	/// </summary>
	public interface IProductCreditLedgerEntry : IBaseLedgerEntry
	{

		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		/// <value>
		/// The order identifier.
		/// </value>
		int? OrderId { get; set; }
		/// <summary>
		/// Gets or sets the order payment identifier.
		/// </summary>
		/// <value>
		/// The order payment identifier.
		/// </value>
		int? OrderPaymentId { get; set; }
	}
}
