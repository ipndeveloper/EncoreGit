using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Commissions.Models;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Commissions.CommissionRunners;

namespace nsCore.Areas.Commissions.Controllers
{
	public class RunnerController : BaseCommissionController
    {
        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

        [FunctionFilter("Commissions", "~/Accounts")]
        public virtual ActionResult Index()
        {
            var commissionPrep = new CommissionPrep();

            var systemEventSetting = commissionPrep.GetSystemEventSettings();
            bool commissionRunInProgress =
                commissionPrep.IsCommissionRunInProgress(systemEventSetting.PrepSystemEventApplicationId) ||
                commissionPrep.IsCommissionRunInProgress(systemEventSetting.PublishSystemEventApplicationId); 

            var commissionRunPlans = commissionPrep.LoadByCommissionRunType(CommissionRunKind.Prep);
            var defaultCommissionRunPlan = commissionPrep.GetDefaultCommissionRunPlan(commissionRunPlans);

            var openPeriods = _commissionsService.GetOpenPeriods(defaultCommissionRunPlan);

            var model = new RunnerModel(commissionRunPlans, openPeriods, commissionRunInProgress, systemEventSetting.PrepSystemEventApplicationId);

            return View(model);
        }

        [HttpPost]
        [FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
        public virtual ActionResult GetPeriodForPlan(int planID)
        {
            var openPeriods = _commissionsService.GetOpenPeriods((DisbursementFrequencyKind)planID);
            var periods = openPeriods.Select(o => new 
            { 
                PeriodID = o.PeriodId, 
                Value = planID == (int)DisbursementFrequencyKind.Monthly ? o.EndDateUTC.ToLocalTime().ToString("yyyy-MM") : o.PeriodId.ToString()
            });                       

            return Json(new { result = true, periods = periods });
        }

        [HttpPost]
        [FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
        public virtual ActionResult ExecuteCommissionRun(int planID, int periodID)
        {
            var commissionPrep = new CommissionPrep();
            var commissionRun = commissionPrep.ExecuteCommissionRun(planID, periodID);
            commissionRun.OpenPeriods = _commissionsService.GetOpenPeriods((DisbursementFrequencyKind)commissionRun.PlanId);

            CoreContext.CurrentCommissionRun = commissionRun;

            return Json( commissionRun.ToJson() );                
        }

        [HttpPost]
        [FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
        public virtual ActionResult ExecuteCommissionRunStep()
        {
            var commissionPrep = new CommissionPrep();
            var commissionRun = commissionPrep.ExecuteCommissionRunStep(CoreContext.CurrentCommissionRun);

            commissionRun.OpenPeriods = _commissionsService.GetOpenPeriods((DisbursementFrequencyKind)commissionRun.PlanId);

            if (commissionRun.InProgress)
            {
                CoreContext.CurrentCommissionRun = commissionRun;
            }
            else
            {
                CoreContext.CurrentCommissionRun = null;
            }

            return Json( commissionRun.ToJson() );               
        }

        [HttpPost]
        [FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
        public virtual ActionResult CloseSystemEvents(int systemEventApplicationID)
        {
            CommissionPrep commissionPrep = new CommissionPrep();
            bool result = commissionPrep.CloseSystemEvents(systemEventApplicationID);

            return Json(new { result = result });
        }
    }
}
