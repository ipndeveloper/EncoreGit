using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Calculation Kind (formerly type)
	/// </summary>
	public interface ICalculationKind
	{
		/// <summary>
		/// the calculation kind identifier
		/// </summary>
		int CalculationKindId { get; }

		/// <summary>
		/// the calculation code
		/// </summary>
		string Code { get; }

		/// <summary>
		/// friendly name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// is overridable by user
		/// </summary>
		bool IsUserOverridable { get; }

		/// <summary>
		/// is real time
		/// </summary>
		bool IsRealTime { get; }

		/// <summary>
		/// the term name
		/// </summary>
		string TermName { get; }

		/// <summary>
		/// date last modified
		/// </summary>
		DateTime DateModified { get; }

		/// <summary>
		/// needs clarification from commissions
		/// </summary>
		bool? ReportVisibility { get; }

		/// <summary>
		/// the client code
		/// </summary>
		string ClientCode { get; }

		/// <summary>
		/// the client name
		/// </summary>
		string ClientName { get; }
	}
}
