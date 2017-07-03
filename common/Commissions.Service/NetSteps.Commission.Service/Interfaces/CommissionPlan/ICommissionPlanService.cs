using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.CommissionPlan
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICommissionPlanService
	{
		/// <summary>
		/// Gets the commission plan.
		/// </summary>
		/// <param name="commissionPlanId">The commission plan identifier.</param>
		/// <returns></returns>
		ICommissionPlan GetCommissionPlan(int commissionPlanId);

		/// <summary>
		/// Gets the commission plan.  I do not think this is a good long term fix - we should be able to set up commissions apart from its disbursement.
		/// </summary>
		/// <param name="disbursementFrequency">The disbursement frequency.</param>
		/// <returns></returns>
		ICommissionPlan GetCommissionPlan(DisbursementFrequencyKind disbursementFrequency);

		/// <summary>
		/// Gets the commission plans.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ICommissionPlan> GetCommissionPlans();

		/// <summary>
		/// Gets the commission plans.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		IEnumerable<ICommissionPlan> GetCommissionPlans(Predicate<ICommissionPlan> filter);

	}
}
