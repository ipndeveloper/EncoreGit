using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;

namespace NetSteps.Commissions.Common
{
	/// <summary>
	/// Service to run commission jobs
	/// </summary>
	public interface ICommissionJobService
	{
		/// <summary>
		/// Gets the commission job status.
		/// </summary>
		/// <param name="jobName">Name of the job.</param>
		/// <returns></returns>
		CommissionJobStatus GetCommissionJobStatus(string jobName);
		
		/// <summary>
		/// Gets the commission jobs.
		/// </summary>
		/// <param name="disbursementFrequency">The disbursement frequency.</param>
		/// <returns></returns>
		IEnumerable<ICommissionJob> GetCommissionJobs(DisbursementFrequencyKind disbursementFrequency);

		/// <summary>
		/// Starts the commission job.
		/// </summary>
		/// <param name="jobName">Name of the job.</param>
		/// <returns></returns>
		bool StartCommissionJob(string jobName);

		/// <summary>
		/// Gets the commission run output.
		/// </summary>
		/// <param name="jobName">Name of the job.</param>
		/// <returns></returns>
		ICommissionRunOutput GetCommissionRunOutput(string jobName);

		/// <summary>
		/// Starts a system event.
		/// </summary>
		/// <param name="periodId">The period identifier.</param>
		/// <param name="systemEventApplicationId">The system event application identifier.</param>
		/// <returns></returns>
		int StartSystemEvent(int periodId, int systemEventApplicationId);

		/// <summary>
		/// Opens the commission period.
		/// </summary>
		/// <param name="periodId">The period identifier.</param>
		void OpenCommissionPeriod(int periodId);

		/// <summary>
		/// Inserts the system event log.
		/// </summary>
		/// <param name="systemEventId">The system event identifier.</param>
		/// <param name="systemEventApplicationId">The system event application identifier.</param>
		/// <param name="eventKindId">The event kind identifier.</param>
		/// <param name="message">The message.</param>
		void InsertSystemEventLog(int systemEventId, int systemEventApplicationId, int eventKindId, string  message);

		/// <summary>
		/// Closes the system events.
		/// </summary>
		/// <param name="systemEventApplicationId">The system event application identifier.</param>
		/// <returns></returns>
		bool CloseSystemEvents(int systemEventApplicationId);

		/// <summary>
		/// Gets the system events by period.
		/// </summary>
		/// <param name="periodId">The period identifier.</param>
		/// <param name="systemEventApplicationId">The system event application identifier.</param>
		/// <returns></returns>
		IEnumerable<ISystemEvent> GetSystemEventsByPeriod(int periodId, int systemEventApplicationId);

		/// <summary>
		/// Gets the system events.
		/// </summary>
		/// <param name="systemEventApplicationId">The system event application identifier.</param>
		/// <param name="endTime">The end time.</param>
		/// <returns></returns>
		IEnumerable<ISystemEvent> GetSystemEvents(int systemEventApplicationId, DateTime? endTime);

		/// <summary>
		/// Executes the commission run procedure.
		/// </summary>
		/// <param name="periodId">The period identifier.</param>
		/// <param name="sqlTimeOutSeconds">The SQL time out seconds.</param>
		/// <param name="includePeriodIdParameter">if set to <c>true</c> [include period identifier parameter].</param>
		/// <param name="commissionProcedureName">Name of the commission procedure.</param>
		void ExecuteCommissionRunProcedure(int periodId, int sqlTimeOutSeconds,bool includePeriodIdParameter, string commissionProcedureName);

		/// <summary>
		/// Ends the system event.
		/// </summary>
		/// <param name="systemEventId">The system event identifier.</param>
		/// <param name="completed">if set to <c>true</c> [completed].</param>
		void EndSystemEvent(int systemEventId, bool completed);

		/// <summary>
		/// Gets the system event settings.
		/// </summary>
		/// <returns></returns>
		ISystemEventSetting GetSystemEventSettings();

		/// <summary>
		/// Gets the system event log.
		/// </summary>
		/// <param name="systemEventId">The system event identifier.</param>
		/// <param name="previousSystemEventLogId">The previous system event log identifier.</param>
		/// <returns></returns>
		ISystemEventLog GetSystemEventLog(int systemEventId, int previousSystemEventLogId);

		/// <summary>
		/// Gets the commission run.
		/// </summary>
		/// <param name="planId">The plan identifier.</param>
		/// <param name="periodId">The period identifier.</param>
		/// <param name="runKind">Kind of the run.</param>
		/// <returns></returns>
		ICommissionRun GetCommissionRun(int planId, int periodId, CommissionRunKind runKind);

		/// <summary>
		/// Gets the commission run.
		/// </summary>
		/// <param name="planId">The plan identifier.</param>
		/// <param name="runKind">Kind of the run.</param>
		/// <returns></returns>
		ICommissionRun GetCommissionRun(int planId, CommissionRunKind runKind);

		/// <summary>
		/// Gets the commission run plan.
		/// </summary>
		/// <param name="runKind">Kind of the run.</param>
		/// <returns></returns>
		IEnumerable<ICommissionRunPlan> GetCommissionRunPlan(CommissionRunKind runKind);
	}

}
