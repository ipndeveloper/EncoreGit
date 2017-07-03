// -----------------------------------------------------------------------
// <copyright file="CommissionRun.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommissionRun
    {

        #region Constructor

        public CommissionRun()
        {
            this.OpenPeriods = new List<Period>();
            this.SystemEventLog = new SystemEventLog();
            this.SystemEventSetting = new SystemEventSetting();
            this.NextCommissionRunStep = new CommissionRunStep();
            this.CurrentCommissionRunStep = new CommissionRunStep();
            this.CommissionRunSteps = new List<CommissionRunStep>();
        }

        #endregion

        #region Properties

        public int CommissionRunID { get; set; }

        public bool CommissionRunCanBePublished { get; set; }

        public string CommissionRunName { get; set; }

        public List<CommissionRunPlan> CommissionRunPlans { get; set; }
        
        public int CommissionRunProgress
        {
            get
            {
                List<CommissionRunStep> commissionRunSteps = this.CommissionRunSteps.Where(crs => crs.CommissionRunStepNumber <= this.CommissionRunStepNumber).ToList();
                
                return commissionRunSteps.Count > 0 ? commissionRunSteps.Sum(crs => crs.SQLTimeOutSeconds) : 1;       
            }
        }

        public List<CommissionRunStep> CommissionRunSteps { get; set; }

        public int CommissionRunStepNumber
        {
            get { return this.CurrentCommissionRunStep.CommissionRunStepNumber; }
        }

        public Constants.CommissionRunType CommissionRunType { get; set; }

        public int CommissionRunTypeID { get; set; }

        public string CurrentCommissionRunProgress
        {
            get
            {
                return string.Format("{0}%", Math.Round((decimal)this.CommissionRunProgress / (decimal)this.TotalCommissionRunProgress * 100));                 
            }
        }

        public CommissionRunStep CurrentCommissionRunStep { get; set; }

        public int DefaultCommissionRunPlanID { get; set; }

        public bool Enabled { get; set; }

        public bool InProgress { get; set; }

        public int LastSystemEventLogID { get; set; }

        public CommissionRunStep NextCommissionRunStep { get; set; }

        public List<Period> OpenPeriods { get; set; }

        public int PeriodID { get; set; }

        public int PlanID { get; set; }        

        public int SystemEventID { get; set; }

        public SystemEventLog SystemEventLog { get; set; }

        public SystemEventSetting SystemEventSetting { get; set; }

        public int TotalCommissionRunProgress
        {
            get { return this.CommissionRunSteps.Count > 0 ? this.CommissionRunSteps.Sum(crs => crs.SQLTimeOutSeconds) : 1; }
        }

        public int TotalCommissionRunSteps
        {
            get { return this.CommissionRunSteps.Count; }
        }       

        #endregion

        #region Methods

        public object ToJson()
        {
            return new
            {
                result = this.InProgress,
                systemEventID = this.SystemEventID,
                currentStep = this.CurrentCommissionRunStep.CommissionRunStepNumber,
                nextStep = this.NextCommissionRunStep.CommissionRunStepNumber,
                totalSteps = this.TotalCommissionRunSteps,
                commissionRunProgress = this.CommissionRunProgress,
                totalCommissionRunProgress = this.TotalCommissionRunProgress,
                currentCommissionRunProgress = this.CurrentCommissionRunProgress,
                displaycommissionRunStepSystemEventLog = (this.CommissionRunType == Constants.CommissionRunType.Publish && !this.CommissionRunCanBePublished) ? false : true,
                commissionRunSystemEventLog = this.SystemEventLog.ToJson(),
                commissionRunStepSystemEventLog = this.NextCommissionRunStep.SystemEventLog.ToJson(),
                periods = this.OpenPeriods.Select(p => 
                    new 
                    { 
                        PeriodID = p.PeriodID, 
                        Value = PlanID == (int)Constants.Plan.Monthly ? p.EndDate.ToString("yyyy-MM") : p.PeriodID.ToString() 
                    })                            
            };
        }

        #endregion

    }
}
