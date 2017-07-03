using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities.Cache;
using nsCore.Areas.Commissions.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Commissions.Controllers
{
	public class JobController : BaseCommissionController
	{
        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public ActionResult Index(int id)
		{
			//Get the plan type for Monthly.
			//Get the jobs available for the montly plan type
			//Check to see if each job is runnable.
			var monthlyPlan = _commissionsService.GetCommissionPlan(id);

			CommissionPlanProfileModel model = new CommissionPlanProfileModel();

			if (monthlyPlan != null)
			{
				model.Plan = monthlyPlan;
				model.PlanID = (int)monthlyPlan.DisbursementFrequency;
				model.Jobs = GetJobsForPlan(model.PlanID);
			}

			return View(model);
		}

		public bool IsJobRunning(string jobName)
		{
			return _commissionJobService.GetCommissionJobStatus(jobName) == CommissionJobStatus.Running;
		}

		public bool StartJob(string jobName)
		{
			//check to see if the job is running already.  If it is running, say NO, NO WE WILL NOT LET YOU START!
			var jobRunning = _commissionJobService.GetCommissionJobStatus(jobName);
			return jobRunning == CommissionJobStatus.Running ? false : _commissionJobService.StartCommissionJob(jobName);
		}

		public ActionResult GetJobOutput(string jobName)
		{
			var status = _commissionJobService.GetCommissionRunOutput(jobName);
			return View("JobTypeOutput", new JobOutputModel() { JobStatuses = status.Results });
		}

		/// <summary>
		/// Retrieves the available jobs for the plan selected.
		/// A plan is Montly, Weekly, Bi-Monthly, etc
		/// </summary>
		/// <param name="planID"></param>
		/// <returns></returns>
		public IEnumerable<JobDetailsModel> GetJobsForPlan(int planID)
		{
			List<JobDetailsModel> details = new List<JobDetailsModel>();
			var jobs = _commissionJobService.GetCommissionJobs((DisbursementFrequencyKind)planID);
			foreach (var job in jobs)
			{
				bool isJobRunnable = _commissionJobService.GetCommissionJobStatus(job.JobName) == CommissionJobStatus.NotRunning;

				JobDetailsModel detail = new JobDetailsModel()
				{
					CommissionRunTypeID = (int)job.CommissionRunType,
                    CommissionRunType = job.CommissionRunType,
					IsJobRunnable = isJobRunnable,
					JobDisplayName = job.JobDisplayName,
					JobName = job.JobName
				};

				var currentOutput = _commissionJobService.GetCommissionRunOutput(job.JobName).Results;
				detail.JobOutput = new JobOutputModel() { JobStatuses = currentOutput };

				details.Add(detail);
			}

			return details;
		}
	}
}
