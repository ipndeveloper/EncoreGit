using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Commissions.CommissionRunners
{
    public class CommissionPrep : BaseCommissionRun
    {

        #region Constructor

        public CommissionPrep()
        {

        }

        #endregion

        #region Public Methods

        public ICommissionRun ExecuteCommissionRunStep(ICommissionRun commissionRun)
        {
            try
            {
                int commissionRunStepNumber = commissionRun.CommissionRunStepNumber + 1;

                if (commissionRunStepNumber < commissionRun.TotalCommissionRunSteps)
                {
                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber + 1);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogId;

                    commissionRun.SystemEventLog = Create.New<ISystemEventLog>();
                    commissionRun.InProgress = true;
                }
                else
                {
                    string endMessage = "Commissions Run completed successfully for Period: " + commissionRun.PeriodId;

                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, endMessage);
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
                    string errorMessage = "Prep Commission Run Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);

                    commissionRun.CurrentCommissionRunStep = Create.New<ICommissionRunStep>();
                    commissionRun.NextCommissionRunStep = Create.New<ICommissionRunStep>();
                }
            }

            return commissionRun;
        }

        public ICommissionRun ExecuteCommissionRun(int planID, int periodID)
        {
            var commissionRun = Create.New<ICommissionRun>();

            try
            {
                commissionRun = base.GetCommissionRun(planID, periodID, CommissionRunKind.Prep);
                commissionRun.CommissionRunPlans = base.LoadByCommissionRunType(CommissionRunKind.Prep);
                commissionRun.DefaultCommissionRunPlan = base.GetDefaultCommissionRunPlan(commissionRun.CommissionRunPlans);

                if (commissionRun.TotalCommissionRunSteps > 0)
                {
                    string startMessage = "Starting Commissions Run for Period: " + commissionRun.PeriodId;

                    base.StartSystemEvent(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, startMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.SystemEventLog.SystemEventLogId;

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault();
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                    commissionRun.LastSystemEventLogId = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogId;

                    commissionRun.InProgress = true;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventId > 0)
                {
                    string errorMessage = "Prep Commission Run Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationId, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventId, commissionRun.LastSystemEventLogId);
                }
            }

            return commissionRun;
        }

        #endregion

    }
}