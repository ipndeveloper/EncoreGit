using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Commissions.Models
{
	public class CommissionPlanProfileModel
	{

		#region Constructors

		public CommissionPlanProfileModel()
		{
		}

		#endregion

		#region Properties

		public int PlanID { get; set; }
		public ICommissionPlan Plan { get; set; }
		public IEnumerable<JobDetailsModel> Jobs { get; set; }
        public IEnumerable<CommissionRunKind> AvailableRunTypes { get; set; }

		#endregion

		#region Methods

		#endregion

	}
}