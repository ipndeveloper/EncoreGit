using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Commissions.Models
{
    public class BaseCommissionsModel
    {

        #region Constructors

        public BaseCommissionsModel()
        {
            //For default model binding.
        }

        public BaseCommissionsModel(IEnumerable<ICommissionRunPlan> commissionRunPlans, IEnumerable<IPeriod> openPeriods, bool commissionRunInProgress) 
        {
            this.OpenPeriods = openPeriods;
            this.CommissionRunPlans = commissionRunPlans;
            this.CommissionRunInProgress = commissionRunInProgress;
        }

        #endregion

        #region Properties

        public bool CommissionRunInProgress { get; private set; }        

        public IEnumerable<IPeriod> OpenPeriods { get; set; }

        public IEnumerable<ICommissionRunPlan> CommissionRunPlans { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}