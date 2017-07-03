using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Represents a Bonus Payout Line Item
	/// </summary>
	public interface IBonusPayout
	{
		/// <summary>
		/// The Account ID this payout is for
		/// </summary>
		int AccountID { get; }

		/// <summary>
		/// The PeriodID this payout is for
		/// </summary>
		int PeriodID { get; }

		/// <summary>
		/// The Order in which to present the payout
		/// </summary>
		int SortOrder { get; }

		/// <summary>
		/// The parent Bonus Category
		/// </summary>
		string BonusCategory { get; }

		/// <summary>
		/// The Bonus SubCategory
		/// </summary>
		string BonusSubcategory { get; }

		/// <summary>
		/// The name of the Bonus value
		/// </summary>
		string BonusName { get; }

		/// <summary>
		/// The value of the Bonus
		/// </summary>
		decimal BonusAmount { get; }
	}
}
