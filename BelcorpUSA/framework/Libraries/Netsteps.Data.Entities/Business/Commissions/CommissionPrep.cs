// -----------------------------------------------------------------------
// <copyright file="CommissionsPrep.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;
    using System.Linq;

    public class CommissionPrep : BaseCommissionRun
    {
      
        #region Constructor

        public CommissionPrep()
        {
            
        }        

        #endregion        

        #region Public Methods

        public CommissionRun ExecuteCommissionRunStep(CommissionRun commissionRun)
        {
            try
            {
                int commissionRunStepNumber = commissionRun.CommissionRunStepNumber + 1;

                if (commissionRunStepNumber < commissionRun.TotalCommissionRunSteps)
                {
                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault(crs => crs.CommissionRunStepNumber == commissionRunStepNumber + 1);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogID;

                    commissionRun.SystemEventLog = new SystemEventLog();
                    commissionRun.InProgress = true;
                }
                else
                {
                    string endMessage = "Commissions Run completed successfully for Period: " + commissionRun.PeriodID;

                    base.ExecuteCurrentCommissionRunStep(commissionRun, commissionRunStepNumber);

                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, endMessage);
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
                    string errorMessage = "Prep Commission Run Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);

                    commissionRun.CurrentCommissionRunStep = new CommissionRunStep();
                    commissionRun.NextCommissionRunStep = new CommissionRunStep();
                }
            }

            return commissionRun;
        }

        public CommissionRun ExecuteCommissionRun(int planID, int periodID)
        {
            CommissionRun commissionRun = new CommissionRun();

            try
            {
                commissionRun = base.GetCommissionRun(planID, periodID, Constants.CommissionRunType.Prep);
                commissionRun.CommissionRunPlans = base.LoadByCommissionRunType(Constants.CommissionRunType.Prep);
                commissionRun.DefaultCommissionRunPlanID = base.GetDefaultCommissionRunPlanID(commissionRun.CommissionRunPlans);

                if (commissionRun.TotalCommissionRunSteps > 0)
                {
                    string startMessage = "Starting Commissions Run for Period: " + commissionRun.PeriodID;

                    base.StartSystemEvent(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID);
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, startMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.SystemEventLog.SystemEventLogID;

                    commissionRun.NextCommissionRunStep = commissionRun.CommissionRunSteps.FirstOrDefault();
                    base.InsertSystemEventLog(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, commissionRun.NextCommissionRunStep.DisplayMessage);
                    commissionRun.NextCommissionRunStep.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                    commissionRun.LastSystemEventLogID = commissionRun.NextCommissionRunStep.SystemEventLog.SystemEventLogID;

                    commissionRun.InProgress = true;
                }
            }
            catch (Exception ex)
            {
                if (commissionRun.SystemEventID > 0)
                {
                    string errorMessage = "Prep Commission Run Failed: " + ex.Message;

                    base.HandleFailedCommissionRun(commissionRun, commissionRun.SystemEventSetting.PrepSystemEventApplicationID, errorMessage);
                    commissionRun.SystemEventLog = base.GetSystemEventLog(commissionRun.SystemEventID, commissionRun.LastSystemEventLogID);
                }
            }

            return commissionRun;
        }        

        #endregion        
        
    }
}