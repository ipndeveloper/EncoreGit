using NetSteps.Commissions.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Calculation override
	/// </summary>
	public interface ICalculationOverride : IBaseOverride
	{
		
		/// <summary>
		/// Gets or sets the account title override identifier.
		/// </summary>
		/// <value>
		/// The account title override identifier.
		/// </value>
		int CalculationOverrideId { get; set; }

        /// <summary>
        /// Gets or sets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        ICalculationKind CalculationKind { get; set; }
        
        /// <summary>
		/// Gets or sets the period identifier.
		/// </summary>
		/// <value>
		/// The period identifier.
		/// </value>
		IPeriod Period { get; set; }

		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>
		/// The new value.
		/// </value>
		decimal NewValue { get; set; }

		/// <summary>
		/// Gets or sets whether to override if null
		/// </summary>
		bool OverrideIfNull { get; set; }
		
	}
}
