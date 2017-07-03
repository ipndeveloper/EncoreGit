// -----------------------------------------------------------------------
// <copyright file="BaseCommission.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseCommissionRun
    {

        #region Members

        private ICommissionsUiRepository _repository;

        #endregion

        #region Constructor

        public BaseCommissionRun() : this(null)
        {
        }
        public BaseCommissionRun(ICommissionsUiRepository commissionsRepo)
        {
            _repository = commissionsRepo ?? Create.New<ICommissionsUiRepository>();
        }

        #endregion

        #region Public Methods

        public bool CloseSystemEvents(int systemEventApplicationID)
        {
            return this._repository.CloseSystemEvents(systemEventApplicationID);
        }

        public int GetDefaultCommissionRunPlanID(List<CommissionRunPlan> commissionRunPlans)
        {
            int defaultCommissionRunPlanID = (int)Constants.Plan.Monthly;
            CommissionRunPlan defaultCommissionRunPlan = commissionRunPlans.FirstOrDefault(c => c.DefaultPlan == true);

            if (defaultCommissionRunPlan != null)
            {
                defaultCommissionRunPlanID = defaultCommissionRunPlan.PlanID;
            }
            else if (commissionRunPlans.Count > 0)
            {
                defaultCommissionRunPlanID = commissionRunPlans.First().PlanID;
            }

            return defaultCommissionRunPlanID;
        }

        public List<SystemEvent> GetSystemEvents(int systemEventApplicationID, DateTime? endTime)
        {
            return this._repository.GetSystemEvents(systemEventApplicationID, endTime);
        }

        public SystemEventSetting GetSystemEventSettings()
        {
            return this._repository.GetSystemEventSettings();
        }

        public bool IsCommissionRunInProgress(int systemEventApplicationID)
        {
            List<SystemEvent> systemEvents = this.GetSystemEvents(systemEventApplicationID, null);

            return systemEvents.Count > 0;
        }

        public List<CommissionRunPlan> LoadByCommissionRunType(Constants.CommissionRunType commissionRunType)
        {
            return this._repository.GetCommissionRunPlan((int)commissionRunType);
        }

        #endregion

        #region Protected Methods

        protected virtual void CleanupFailedCommissionRun(CommissionRun commissionRun)
        {
        }

        protected void EndSystemEvent(CommissionRun commissionRun, bool completed)
        {
            this._repository.EndSystemEvent(commissionRun.SystemEventID, completed);
        }

        protected void ExecuteCommissionRunProcedure(CommissionRun commissionRun)
        {
            this._repository.ExecuteCommissionRunProcedure(
                commissionRun.PeriodID,
                commissionRun.CurrentCommissionRunStep.SQLTimeOutSeconds,
                commissionRun.CurrentCommissionRunStep.IncludePeriodIDParameter,
                commissionRun.CurrentCommissionRunStep.CommissionProcedureName);
        }

        protected void ExecuteCurrentCommissionRunStep(CommissionRun commissionRun, int commissionRunStepNumber)
        {
            commissionRun.CurrentCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber);
            this.ExecuteCommissionRunProcedure(commissionRun);
        }

        protected CommissionRun GetCommissionRun(int planID, int periodID, Constants.CommissionRunType commissionRunType)
        {
            CommissionRun commissionRun = this._repository.GetCommissionRun(planID, (int)commissionRunType);
            commissionRun.CommissionRunType = commissionRunType;
            commissionRun.PeriodID = periodID;

            return commissionRun;
        }

        protected List<SystemEvent> GetSystemEventsByPeriod(CommissionRun commissionRun, int systemEventApplicationID)
        {
            return this._repository.GetSystemEventsByPeriod(commissionRun.PeriodID, systemEventApplicationID);
        }

        protected SystemEventLog GetSystemEventLog(int systemEventID, int previousSystemEventLogID)
        {
            return this._repository.GetSystemEventLog(systemEventID, previousSystemEventLogID);
        }

        protected void HandleFailedCommissionRun(CommissionRun commissionRun, int systemEventApplicationID, string errorMessage)
        {
            commissionRun.InProgress = false;            

            this._repository.InsertSystemEventLog(
                commissionRun.SystemEventID,
                systemEventApplicationID,
                commissionRun.SystemEventSetting.SystemEventLogErrorTypeID,
                errorMessage);

            this.EndSystemEvent(commissionRun, false);
            CleanupFailedCommissionRun(commissionRun);
        }

        protected void InsertSystemEventLog(CommissionRun commissionRun, int systemEventApplicationID, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this._repository.InsertSystemEventLog(
                    commissionRun.SystemEventID,
                    systemEventApplicationID,
                    commissionRun.SystemEventSetting.SystemEventLogNoticeTypeID,
                    message);
            }
        }

        protected void OpenCommissionPeriod(CommissionRun commissionRun)
        {
            this._repository.OpenCommissionPeriod(commissionRun.PeriodID);
        }

        protected void StartSystemEvent(CommissionRun commissionRun, int systemEventApplicationID)
        {
            commissionRun.SystemEventID = this._repository.StartSystemEvent(commissionRun.PeriodID, systemEventApplicationID);
        }

        #endregion

    }
}
