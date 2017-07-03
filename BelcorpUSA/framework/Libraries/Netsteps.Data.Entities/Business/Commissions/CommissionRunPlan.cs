// -----------------------------------------------------------------------
// <copyright file="CommissionPlan.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{

    public class CommissionRunPlan
    {        

        #region Constructor

        public CommissionRunPlan()
        {
            
        }

        #endregion

        #region Properties

        public int PlanID { get; set; }

        public string PlanCode { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool DefaultPlan { get; set; }

        public string TermName { get; set; }

        public int CommissionRunTypeID { get; set; }

        public string CommissionRunTypeName { get; set; }

        public string CommissionRunName { get; set; }

        #endregion       
        
    }
}
