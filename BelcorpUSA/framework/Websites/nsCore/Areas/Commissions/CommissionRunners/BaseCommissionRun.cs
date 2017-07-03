using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Commissions.CommissionRunners
{
    public class BaseCommissionRun
    {

        #region Members

        private ICommissionsService _commissionsService;
        private ICommissionJobService _commissionJobService;

        #endregion

        #region Constructor

        public BaseCommissionRun()
            : this(null)
        {
        }
        public BaseCommissionRun(ICommissionsService commissionsService)
        {
            _commissionsService = commissionsService ?? Create.New<ICommissionsService>();
            _commissionJobService = Create.New<ICommissionJobService>();
        }

        #endregion

        #region Public Methods

        public bool CloseSystemEvents(int systemEventApplicationID)
        {
            return _commissionJobService.CloseSystemEvents(systemEventApplicationID);
        }

        public DisbursementFrequencyKind GetDefaultCommissionRunPlan(IEnumerable<ICommissionRunPlan> commissionRunPlans)
        {
            var defaultCommissionRunPlanID = DisbursementFrequencyKind.Monthly;

            var defaultCommissionRunPlan = commissionRunPlans.FirstOrDefault(c => c.DefaultPlan == true);

            if (defaultCommissionRunPlan != null)
            {
                defaultCommissionRunPlanID = (DisbursementFrequencyKind)defaultCommissionRunPlan.PlanId;
            }
            else if (commissionRunPlans.Count() > 0)
            {
                defaultCommissionRunPlanID = (DisbursementFrequencyKind)commissionRunPlans.First().PlanId;
            }

            return defaultCommissionRunPlanID;
        }

        public IEnumerable<ISystemEvent> GetSystemEvents(int systemEventApplicationID, DateTime? endTime)
        {
            return _commissionJobService.GetSystemEvents(systemEventApplicationID, endTime);
        }

        public ISystemEventSetting GetSystemEventSettings()
        {
            return _commissionJobService.GetSystemEventSettings();
        }

        public bool IsCommissionRunInProgress(int systemEventApplicationID)
        {
            var systemEvents = this.GetSystemEvents(systemEventApplicationID, null);

            return systemEvents.Count() > 0;
        }

        public IEnumerable<ICommissionRunPlan> LoadByCommissionRunType(CommissionRunKind commissionRunType)
        {
            return _commissionJobService.GetCommissionRunPlan(commissionRunType);
        }

        #endregion

        #region Protected Methods

        protected virtual void CleanupFailedCommissionRun(ICommissionRun commissionRun)
        {
        }

        protected void EndSystemEvent(ICommissionRun commissionRun, bool completed)
        {
            _commissionJobService.EndSystemEvent(commissionRun.SystemEventId, completed);
        }

        protected void ExecuteCommissionRunProcedure(ICommissionRun commissionRun)
        {
            _commissionJobService.ExecuteCommissionRunProcedure(
                commissionRun.PeriodId,
                commissionRun.CurrentCommissionRunStep.SQLTimeOutSeconds,
                commissionRun.CurrentCommissionRunStep.IncludePeriodIdParameter,
                commissionRun.CurrentCommissionRunStep.CommissionProcedureName);
        }

        protected void ExecuteCurrentCommissionRunStep(ICommissionRun commissionRun, int commissionRunStepNumber)
        {
            commissionRun.CurrentCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber);
            this.ExecuteCommissionRunProcedure(commissionRun);
        }

        protected ICommissionRun GetCommissionRun(int planID, int periodID, CommissionRunKind commissionRunKind)
        {
            return _commissionJobService.GetCommissionRun(planID, periodID, commissionRunKind);
        }

        protected IEnumerable<ISystemEvent> GetSystemEventsByPeriod(ICommissionRun commissionRun, int systemEventApplicationID)
        {
            return _commissionJobService.GetSystemEventsByPeriod(commissionRun.PeriodId, systemEventApplicationID);
        }

        protected ISystemEventLog GetSystemEventLog(int systemEventID, int previousSystemEventLogID)
        {
            return _commissionJobService.GetSystemEventLog(systemEventID, previousSystemEventLogID);
        }

        protected void HandleFailedCommissionRun(ICommissionRun commissionRun, int systemEventApplicationID, string errorMessage)
        {
            commissionRun.InProgress = false;

            _commissionJobService.InsertSystemEventLog(
                commissionRun.SystemEventId,
                systemEventApplicationID,
                commissionRun.SystemEventSetting.SystemEventLogErrorTypeId,
                errorMessage);

            this.EndSystemEvent(commissionRun, false);
            CleanupFailedCommissionRun(commissionRun);
        }

        protected void InsertSystemEventLog(ICommissionRun commissionRun, int systemEventApplicationID, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this._commissionJobService.InsertSystemEventLog(
                    commissionRun.SystemEventId,
                    systemEventApplicationID,
                    commissionRun.SystemEventSetting.SystemEventLogNoticeTypeId,
                    message);
            }
        }

        protected void OpenCommissionPeriod(ICommissionRun commissionRun)
        {
            _commissionJobService.OpenCommissionPeriod(commissionRun.PeriodId);
        }

        protected void StartSystemEvent(ICommissionRun commissionRun, int systemEventApplicationID)
        {
            commissionRun.SystemEventId = _commissionJobService.StartSystemEvent(commissionRun.PeriodId, systemEventApplicationID);
        }

        #endregion

    }
}