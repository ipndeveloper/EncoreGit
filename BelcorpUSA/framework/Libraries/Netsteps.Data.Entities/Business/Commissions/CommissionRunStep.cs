// -----------------------------------------------------------------------
// <copyright file="CommissionRunStep.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{

    public class CommissionRunStep
    {

        #region Constructor

        public CommissionRunStep()
        {
            this.SystemEventLog = new SystemEventLog();
        }

        #endregion

        #region Properties

        public string CommissionProcedureName { get; set; }

        public int CommissionRunID { get; set; }

        public int CommissionRunProcedureID { get; set; }        

        public int CommissionRunStepID { get; set; }

        public int CommissionRunStepNumber { get; set; }

        public string DisplayMessage { get; set; }

        public bool Enabled { get; set; }

        public bool IncludePeriodIDParameter { get; set; }

        public int SortOrder { get; set; }

        public int SQLTimeOutSeconds { get; set; }

        public SystemEventLog SystemEventLog { get; set; }
        
        #endregion        

    }
}
