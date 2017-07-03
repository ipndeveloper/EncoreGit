using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// KPI for an account
	/// </summary>
	public interface IAccountKPI
	{
		/// <summary>
		/// Gets or sets the type of the data.
		/// </summary>
		/// <value>
		/// The type of the data.
		/// </value>
		string DataType { get; set; }
		/// <summary>
		/// Gets or sets the kpi type code.
		/// </summary>
		/// <value>
		/// The kpi type code.
		/// </value>
		string KPITypeCode { get; set; }
		/// <summary>
		/// Gets or sets the kpi value.
		/// </summary>
		/// <value>
		/// The kpi value.
		/// </value>
		string KPIValue { get; set; }
		/// <summary>
		/// Gets or sets the name of the term.
		/// </summary>
		/// <value>
		/// The name of the term.
		/// </value>
		string TermName { get; set; }
	}
}
