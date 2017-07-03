using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Commissions.Models
{
    public class RunnerModel : BaseCommissionsModel
    {

        #region Constructors

        public RunnerModel()
        {
            //For default model binding.
        }

        public RunnerModel(IEnumerable<ICommissionRunPlan> commissionRunPlans, IEnumerable<IPeriod> openPeriods, bool commissionRunInProgress, int systemEventApplicationID)
            : base(commissionRunPlans, openPeriods, commissionRunInProgress) 
        {
            this.SystemEventApplicationID = systemEventApplicationID;
        }

        #endregion

        #region Properties

        public int SystemEventApplicationID { get; private set; }

        #endregion

        #region Methods

        #endregion

    }
}