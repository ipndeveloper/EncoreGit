using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission run plan?
	/// </summary>
	public interface ICommissionRunPlan
	{
		/// <summary>
		/// Gets or sets the plan identifier.
		/// </summary>
		/// <value>
		/// The plan identifier.
		/// </value>
		 int PlanId { get; set; }

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
		 /// Gets or sets a value indicating whether [enabled].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
		 /// </value>
		 bool Enabled { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [default plan].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [default plan]; otherwise, <c>false</c>.
		 /// </value>
		 bool DefaultPlan { get; set; }

		 /// <summary>
		 /// Gets or sets the name of the term.
		 /// </summary>
		 /// <value>
		 /// The name of the term.
		 /// </value>
		 string TermName { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run type identifier.
		 /// </summary>
		 /// <value>
		 /// The commission run type identifier.
		 /// </value>
		 CommissionRunKind RunKind { get; set; }

		 /// <summary>
		 /// Gets or sets the name of the commission run type.
		 /// </summary>
		 /// <value>
		 /// The name of the commission run type.
		 /// </value>
		 string RunTypeName { get; set; }

		 /// <summary>
		 /// Gets or sets the name of the commission run.
		 /// </summary>
		 /// <value>
		 /// The name of the commission run.
		 /// </value>
		 string RunName { get; set; }
	}
}
