using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICommissionsUiRepository
	{
		List<CommissionsProcess> GetProcessHistory(int periodID, bool skipImport, bool skipSync);
		List<CommissionsProcess> GetManualProcesses(int planID);
		List<CommissionsPlan> GetPlans();
		List<CommissionsPeriod> GetPeriods(int PlanID);
		bool ToggleDisplayEarnings(int PeriodID, bool? value);
        //TODO: Commissions Refactor - GetPerformanceWidgetData
		//usp_get_performancelandingwidgets_Result GetPerformanceWidgetData(int accountID, int periodID);
		//TODO: GetPerformanceOverviewWidgetData
        //List<uspGetKPIsForAccount_Result> GetPerformanceOverviewWidgetData(int accountID, int periodID);
		//List<CommissionRunPlan> GetCommissionRunPlan(int commissionRunTypeID);
		//CommissionRun GetCommissionRun(int planID, int commissionRunTypeID);
		//int StartSystemEvent(int periodID, int systemEventApplicationID);
		//bool EndSystemEvent(int systemEventID, bool completed);
		bool OpenCommissionPeriod(int periodID);
		//bool CloseSystemEvents(int systemEventApplicationID);
		//bool InsertSystemEventLog(int systemEventID, int systemEventApplicationID, int systemEventLogTypeID, string eventMessage);
		bool ExecuteCommissionRunProcedure(int periodID, int sqlTimeOutSeconds, bool includePeriodIDParameter, string commissionProcedureName);
		//TODO: Commissions Refactor - GetSystemEventLog
        //SystemEventLog GetSystemEventLog(int systemEventID, int previousSystemEventLogID);
        //TODO: Commissions Refactor - GetSystemEventSettings
        //SystemEventSetting GetSystemEventSettings();
        //TODO: Commissions Refactor - GetSystemEvents
        //List<SystemEvent> GetSystemEvents(int systemEventApplicationID, DateTime? endTime);
        //TODO: Commissions Refactor - GetSystemEventsByPeriod
        //List<SystemEvent> GetSystemEventsByPeriod(int periodID, int systemEventApplicationID);

		/// <summary>
		/// Whether to display the Commissions -> Runner sub-tab.
		/// </summary>
		/// <returns>True or false as set; defaults to true if not set</returns>
		bool DisplayCommissionsRunnerSubTab();

		/// <summary>
		/// Whether to display the Commissions -> Publish sub-tab.
		/// </summary>
		/// <returns>True or false as set; defaults to true if not set</returns>
		bool DisplayCommissionsPublishSubTab();

        /// <summary>
        /// Gets the calculated commission value.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <returns></returns>
        decimal GetCalculatedCommissionValue(int accountID, string calculationType, System.DateTime timePeriod);

        /// <summary>
        /// Gets the commission config value.
        /// </summary>
        /// <param name="configCode">The config code.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <returns></returns>
        object GetCommissionConfigValue(string configCode, System.DateTime timePeriod);

		#region -- CommissionRun info --
		IEnumerable<CommissionRunType> GetCommissionRunTypes();
		CommissionRunType GetCommissionRunTypeByID(int commissionRunTypeID);
		CommissionJobStatus GetCommissionJobStatus(string jobName);
		bool StartCommissionJob(string jobName);
		IEnumerable<CommissionJob> GetCommissionJobs(int planID);
		RunOutput GetCommissionJobOutput(string jobName);

		#endregion
	}
}
