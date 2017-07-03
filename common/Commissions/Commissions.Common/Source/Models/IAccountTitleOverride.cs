using NetSteps.Commissions.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Account title override
	/// </summary>
	public interface IAccountTitleOverride : IBaseOverride
	{
		
		/// <summary>
		/// Gets or sets the account title override identifier.
		/// </summary>
		/// <value>
		/// The account title override identifier.
		/// </value>
		int AccountTitleOverrideId { get; set; }

		/// <summary>
		/// Gets or sets the period.
		/// </summary>
		/// <value>
		/// The period.
		/// </value>
		IPeriod Period { get; set; }

		/// <summary>
		/// Gets or sets the override title.
		/// </summary>
		/// <value>
		/// The override title.
		/// </value>
		ITitle OverrideTitle { get; set; }

		/// <summary>
		/// Gets or sets the type of the title.
		/// </summary>
		/// <value>
		/// The type of the title.
		/// </value>
		ITitleKind OverrideTitleKind { get; set; }
	}
}
