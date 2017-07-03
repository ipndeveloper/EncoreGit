using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission plan contract
	/// </summary>
	public interface ICommissionPlan
	{
		/// <summary>
		/// The commission plan identifier
		/// </summary>
		int CommissionPlanId { get; set; }

		/// <summary>
		/// Gets or sets the plan code.
		/// </summary>
		/// <value>
		/// The plan code.
		/// </value>
		string PlanCode { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the name of the term.
		/// </summary>
		/// <value>
		/// The name of the term.
		/// </value>
		string TermName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is enabled]; otherwise, <c>false</c>.
		/// </value>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is default].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is default]; otherwise, <c>false</c>.
		/// </value>
		bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the disbursement frequency.
        /// </summary>
        /// <value>
        /// The disbursement frequency.
        /// </value>
        DisbursementFrequencyKind DisbursementFrequency { get; set; }
	}

}
