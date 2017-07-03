using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Commissions.Models
{
	public class PublishModel : BaseCommissionsModel
	{

		#region Constructors

		public PublishModel()
		{
			//For default model binding.
		}

		public PublishModel(IEnumerable<ICommissionRunPlan> commissionRunPlans, IEnumerable<IPeriod> openPeriods, bool commissionRunInProgress)
			: base(commissionRunPlans, openPeriods, commissionRunInProgress)
		{

		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		#endregion

	}
}