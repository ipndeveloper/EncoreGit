using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.CalculationKind
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICalculationKindService
	{
		/// <summary>
		/// Gets the calculation kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ICalculationKind> GetCalculationKinds();

		/// <summary>
		/// Gets the kind of the calculation.
		/// </summary>
		/// <param name="calculationKindId">The calculation kind identifier.</param>
		/// <returns></returns>
		ICalculationKind GetCalculationKind(int calculationKindId);

		/// <summary>
		/// Gets the kind of the calculation.
		/// </summary>
		/// <param name="calculationKindCode">The calculation kind code.</param>
		/// <returns></returns>
		ICalculationKind GetCalculationKind(string calculationKindCode);
	}
}
