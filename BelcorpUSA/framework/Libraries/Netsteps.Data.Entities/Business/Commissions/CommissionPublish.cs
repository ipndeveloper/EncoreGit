// -----------------------------------------------------------------------
// <copyright file="CommissionPublish.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommissionPublish : BaseCommissionRun
    {

        #region Constructor

        public CommissionPublish()
        {

        }

        #endregion

        #region Methods

        protected override void CleanupFailedCommissionRun(CommissionRun commissionRun)
        {
            base.OpenCommissionPeriod(commissionRun);
        }

        public CommissionRun ExecuteCommissionPublish(int planID, int periodID)
        {
            CommissionRun commissionRun = new CommissionRun();

            try
            {
                commissionRun = base.GetCommissionRun(planID, periodID, Constants.CommissionRunType.Publish);
                commissionRun.CommissionRunPlans = base.LoadByCommissionRunType(Constants.CommissionRunType.Prep);
                commissionRun.DefaultCommissionRunPlanID = base.GetDefaultCommissionRunPlanID(commissionRun.CommissionRunPlans);
                this.CommissionRunCanBePublished(commissionRun);

                if (commissionRun.CommissionRunCanBePublished && commissionRun.TotalCommissionRunSteps > 0)
                {
                    string startMessage = "Starting Commissions Publish for Period: " + commissionRun.PeriodID;

                    base.StartSystemEvent(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, startMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.SystemEventLog.SystemEventLogID;

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault();
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogID;

                    commissionRun.InProgress = true;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventID > 0)
                {
                    string errorMessage = "Commissions Publish Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                }
            }

            return commissionRun;
        }

        public CommissionRun ExecuteCommissionPublishStep(CommissionRun commissionRun)
        {
            try
            {
                int commissionRunStepNumber = commissionRun.CommissionRunStepNumber + 1;

                if (commissionRunStepNumber < commissionRun.TotalCommissionRunSteps)
                {
                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber + 1);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogID;

                    commissionRun.SystemEventLog = new SystemEventLog();
                    commissionRun.InProgress = true;
                }
                else
                {
                    string endMessage = "Commissions Publish Finished successfully for period: " + commissionRun.PeriodID;

                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, endMessage);
                    base.EndSystemEvent(commissionRun, true);

                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.SystemEventLog.SystemEventLogID;
                    commissionRun.InProgress = false;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventID > 0)
                {
                    string errorMessage = "Commissions Publish Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationID, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);

                    commissionRun.CurrentCommissionRunStep = new CommissionRunStep();
                    commissionRun.NextCommissionRunStep = new CommissionRunStep();
                }
            }

            return commissionRun;
        }

        public void CommissionRunCanBePublished(CommissionRun commissionRun)
        {
            List<SystemEvent> systemEvents = 
                base.GetSystemEventsByPeriod(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID)
                    .OrderByDescending(s => s.SystemEventID).ToList();

            SystemEvent systemEvent = systemEvents.FirstOrDefault();

            commissionRun.CommissionRunCanBePublished = systemEvent != null ? systemEvent.Completed : false;                       
        }

        #endregion

    }
}
