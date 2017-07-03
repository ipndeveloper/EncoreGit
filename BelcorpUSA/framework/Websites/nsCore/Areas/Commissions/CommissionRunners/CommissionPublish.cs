using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Commissions.CommissionRunners
{
    public class CommissionPublish : BaseCommissionRun
    {

        #region Constructor

        public CommissionPublish()
        {

        }

        #endregion

        #region Methods

        protected override void CleanupFailedCommissionRun(ICommissionRun commissionRun)
        {
            base.OpenCommissionPeriod(commissionRun);
        }

        public ICommissionRun ExecuteCommissionPublish(int planID, int periodID)
        {
            var commissionRun = Create.New<ICommissionRun>();

            try
            {
                commissionRun = base.GetCommissionRun(planID, periodID, CommissionRunKind.Publish);
                commissionRun.CommissionRunPlans = base.LoadByCommissionRunType(CommissionRunKind.Prep);
                commissionRun.DefaultCommissionRunPlan = base.GetDefaultCommissionRunPlan(commissionRun.CommissionRunPlans);
                this.CommissionRunCanBePublished(commissionRun);

                if (commissionRun.CommissionRunCanBePublished && commissionRun.TotalCommissionRunSteps > 0)
                {
                    string startMessage = "Starting Commissions Publish for Period: " + commissionRun.PeriodId;

                    base.StartSystemEvent(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, startMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.SystemEventLog.SystemEventLogId;

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault();
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogId;

                    commissionRun.InProgress = true;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventId > 0)
                {
                    string errorMessage = "Commissions Publish Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                }
            }

            return commissionRun;
        }

        public ICommissionRun ExecuteCommissionPublishStep(ICommissionRun commissionRun)
        {
            try
            {
                int commissionRunStepNumber = commissionRun.CommissionRunStepNumber + 1;

                if (commissionRunStepNumber < commissionRun.TotalCommissionRunSteps)
                {
                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber + 1);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogId;

                    commissionRun.SystemEventLog = Create.New<ISystemEventLog>();
                    commissionRun.InProgress = true;
                }
                else
                {
                    string endMessage = "Commissions Publish Finished successfully for period: " + commissionRun.PeriodId;

                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, endMessage);
                    base.EndSystemEvent(commissionRun, true);

                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.SystemEventLog.SystemEventLogId;
                    commissionRun.InProgress = false;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventId > 0)
                {
                    string errorMessage = "Commissions Publish Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PublishSystemEventApplicationId, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);

                    commissionRun.CurrentCommissionRunStep = Create.New<ICommissionRunStep>();
                    commissionRun.NextCommissionRunStep = Create.New<ICommissionRunStep>();
                }
            }

            return commissionRun;
        }

        public void CommissionRunCanBePublished(ICommissionRun commissionRun)
        {
            var systemEvents =
                base.GetSystemEventsByPeriod(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId)
                    .OrderByDescending(s => s.SystemEventId).ToList();

            var systemEvent = systemEvents.FirstOrDefault();

            commissionRun.CommissionRunCanBePublished = systemEvent != null ? systemEvent.Completed : false;
        }

        #endregion

    }
}