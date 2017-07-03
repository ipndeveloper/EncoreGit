using System.Data;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using NetSteps.Foundation.Common;
using System.Globalization;

namespace NetSteps.Commissions.Service
{
	public class CommissionsJobService : ICommissionJobService
	{
		private IConnectionProvider _commissionsConnectionProvider;
		protected IConnectionProvider CommissionsConnectionProvider
		{
			get
			{
				if(_commissionsConnectionProvider == null)
				{
					_commissionsConnectionProvider = Create.NewNamed<IConnectionProvider>(CommissionsConstants.ConnectionStringNames.Commissions);
				}
				return _commissionsConnectionProvider;
			}
		}

		private IConnectionProvider _commissionsPrepConnectionProvider;
		protected IConnectionProvider CommissionsPrepConnectionProvider
		{
			get
			{
				if (_commissionsPrepConnectionProvider == null)
				{
					_commissionsPrepConnectionProvider = Create.NewNamed<IConnectionProvider>(CommissionsConstants.ConnectionStringNames.CommissionsPrep);
				}
				return _commissionsPrepConnectionProvider;
			}
		}                

		public bool CloseSystemEvents(int systemEventApplicationId)
		{
			var systemEvents = GetSystemEvents(systemEventApplicationId, null);
			foreach (var systemEvent in systemEvents)
			{
				EndSystemEvent(systemEvent.SystemEventId, false);
			}

			return true;
		}

		public void EndSystemEvent(int systemEventId, bool completed)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspEndSystemEvent]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventID", systemEventId));
					command.Parameters.Add(new SqlParameter("@Completed", completed));

					connection.Open();

					command.ExecuteNonQuery();

					connection.Close();
				}
			}
		}

		public void ExecuteCommissionRunProcedure(int periodId, int sqlTimeOutSeconds, bool includePeriodIdParameter, string commissionProcedureName)
		{
			using (var connection = CommissionsPrepConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand(commissionProcedureName, connection))
				{
					command.CommandTimeout = sqlTimeOutSeconds;

					if (includePeriodIdParameter)
					{
						command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
					}

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		public CommissionJobStatus GetCommissionJobStatus(string jobName)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspIsJobRunning]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@JobName", jobName));
					command.Parameters.Add(new SqlParameter("@Return", SqlDbType.Int, 4) { Direction = ParameterDirection.Output });

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					var result = command.Parameters["@Return"].Value.ToString();
					if (result == "0" || result == "1")
					{
						result = result == "0" ? "false" : "true";
					}

					var returnValue = Convert.ToBoolean(result);
					return returnValue ? CommissionJobStatus.Running : CommissionJobStatus.NotRunning;
				}
			}
		}

		public IEnumerable<ICommissionJob> GetCommissionJobs(DisbursementFrequencyKind disbursementFrequency)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspCommissionRunGetJobs]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@planID", "1"));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						var jobs = new List<ICommissionJob>();
						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							var job = Create.New<ICommissionJob>();
							job.CommissionRunId = row["CommissionRunID"] != DBNull.Value ? (int)row["CommissionRunID"] : 0;
							//job.PlanID = row["PlanID"] != DBNull.Value ? (int)row["PlanID"] : 0;
							job.CommissionRunType = row["CommissionRunTypeID"] != DBNull.Value ? (CommissionRunKind)row["CommissionRunTypeID"] : 0;
							job.JobName = row["JobName"] != DBNull.Value ? row["JobName"].ToString() : String.Empty;
							job.JobDisplayName = row["JobDisplayName"] != DBNull.Value ? row["JobDisplayName"].ToString() : String.Empty;
							jobs.Add(job);
						}

						return jobs;
					}
				}
			}
		}

		public ICommissionRun GetCommissionRun(int planId, CommissionRunKind runKind)
		{
			var run = Create.New<ICommissionRun>();

			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspGetCommissionRunWithSteps]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@PlanID", planId));
					command.Parameters.Add(new SqlParameter("@CommissionRunTypeID", (int)runKind));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							run.CommissionRunId = row["CommissionRunID"] != DBNull.Value ? (int)row["CommissionRunID"] : 0;
							run.PlanId = row["PlanID"] != DBNull.Value ? (int)row["PlanID"] : 0;
							run.CommissionRunKindId = row["CommissionRunTypeID"] != DBNull.Value ? (int)row["CommissionRunTypeID"] : 0;
							run.CommissionRunName = row["CommissionRunName"] != DBNull.Value ? (string)row["CommissionRunName"] : string.Empty;
							run.Enabled = row["Enabled"] != DBNull.Value ? (bool)row["Enabled"] : false;
							break;
						}

						var commissionRunStepNumber = 1;

						foreach (DataRow row in dataSet.Tables[1].Rows)
						{
							var step = Create.New<ICommissionRunStep>();
							step.CommissionRunStepId = row["CommissionRunStepID"] != DBNull.Value ? (int)row["CommissionRunStepID"] : 0;
							step.CommissionRunId = row["CommissionRunID"] != DBNull.Value ? (int)row["CommissionRunID"] : 0;
							step.DisplayMessage = row["DisplayMessage"] != DBNull.Value ? (string)row["DisplayMessage"] : string.Empty;
							step.SQLTimeOutSeconds = row["SQLTimeOutSeconds"] != DBNull.Value ? (int)row["SQLTimeOutSeconds"] : 0;
							step.SortOrder = row["SortOrder"] != DBNull.Value ? (int)row["SortOrder"] : 0;
							step.Enabled = row["Enabled"] != DBNull.Value ? (bool)row["Enabled"] : false;
							step.CommissionRunProcedureId = row["CommissionRunProcedureID"] != DBNull.Value ? (int)row["CommissionRunProcedureID"] : 0;
							step.CommissionProcedureName = row["CommissionProcedureName"] != DBNull.Value ? (string)row["CommissionProcedureName"] : string.Empty;
							step.IncludePeriodIdParameter = row["IncludePeriodIDParameter"] != DBNull.Value ? (bool)row["IncludePeriodIDParameter"] : false;
							step.CommissionRunStepNumber = commissionRunStepNumber;
							commissionRunStepNumber++;

							run.CommissionRunSteps.Add(step);
						}

						foreach (DataRow row in dataSet.Tables[2].Rows)
						{
							var systemEventSetting = Create.New<ISystemEventSetting>();
							systemEventSetting.PrepSystemEventApplicationId = row["PrepSystemEventApplicationID"] != DBNull.Value ? (int)row["PrepSystemEventApplicationID"] : 0;
							systemEventSetting.PublishSystemEventApplicationId = row["PublishSystemEventApplicationID"] != DBNull.Value ? (int)row["PublishSystemEventApplicationID"] : 0;
							systemEventSetting.SystemEventLogErrorTypeId = row["SystemEventLogErrorTypeID"] != DBNull.Value ? (int)row["SystemEventLogErrorTypeID"] : 0;
							systemEventSetting.SystemEventLogNoticeTypeId = row["SystemEventLogNoticeTypeID"] != DBNull.Value ? (int)row["SystemEventLogNoticeTypeID"] : 0;

							run.SystemEventSetting = systemEventSetting;
							break;
						}
					}
				}
			}

			return run;
		}

		public ICommissionRun GetCommissionRun(int planId, int periodId, CommissionRunKind runKind)
		{
			var commissionRun = GetCommissionRun(planId, runKind);
			commissionRun.CommissionRunKind = runKind;
			commissionRun.PeriodId = periodId;

			return commissionRun;
		}

		public ICommissionRunOutput GetCommissionRunOutput(string jobName)
		{
			var runOutput = Create.New<ICommissionRunOutput>();
			runOutput.Results = new List<string>();

			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspCommissionRunOutput]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@jobName", jobName));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							var result = row[0] != DBNull.Value ? row[0].ToString() : String.Empty;
							runOutput.Results.Add(result);
						}
					}
				}
			}

			return runOutput;
		}

		public IEnumerable<ICommissionRunPlan> GetCommissionRunPlan(CommissionRunKind runKind)
		{
			var commissionRunPlans = new List<ICommissionRunPlan>();
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspGetCommissionRunPlanByCommissionRunTypeID]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@CommissionRunTypeID", (int)runKind));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							var plan = Create.New<ICommissionRunPlan>();

							plan.PlanId = row["PlanID"] != DBNull.Value ? (int)row["PlanID"] : 0;
							plan.PlanCode = row["PlanCode"] != DBNull.Value ? (string)row["PlanCode"] : string.Empty;
							plan.Name = row["Name"] != DBNull.Value ? (string)row["Name"] : string.Empty;
							plan.Enabled = row["Enabled"] != DBNull.Value ? (bool)row["Enabled"] : false;
							plan.DefaultPlan = row["DefaultPlan"] != DBNull.Value ? (bool)row["DefaultPlan"] : false;
							plan.TermName = row["TermName"] != DBNull.Value ? (string)row["TermName"] : string.Empty;
							plan.RunKind = row["CommissionRunTypeID"] != DBNull.Value ? (CommissionRunKind)row["CommissionRunTypeID"] : 0;
							plan.RunTypeName = row["CommissionRunTypeName"] != DBNull.Value ? (string)row["CommissionRunTypeName"] : string.Empty;
							plan.RunName = row["CommissionRunName"] != DBNull.Value ? (string)row["CommissionRunName"] : string.Empty;

							commissionRunPlans.Add(plan);
						}
					}
				}
			}

			return commissionRunPlans;
		}

		public ISystemEventLog GetSystemEventLog(int systemEventId, int previousSystemEventLogId)
		{
			var systemEventLog = Create.New<ISystemEventLog>();

			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[UspGetNextSystemEventLog]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventID", systemEventId));
					command.Parameters.Add(new SqlParameter("@SystemEventLogID", previousSystemEventLogId));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							systemEventLog.SystemEventLogId = row["SystemEventLogID"] != DBNull.Value ? (int)row["SystemEventLogID"] : 0;
							systemEventLog.SystemEventApplicationId = row["SystemEventApplicationID"] != DBNull.Value ? (int)row["SystemEventApplicationID"] : 0;
							systemEventLog.SystemEventLogTypeId = row["SystemEventLogTypeID"] != DBNull.Value ? (int)row["SystemEventLogTypeID"] : 0;
							systemEventLog.EventMessage = row["EventMessage"] != DBNull.Value ? (string)row["EventMessage"] : string.Empty;
							systemEventLog.CreatedDate = row["CreatedDate"] != DBNull.Value ? (DateTime)row["CreatedDate"] : DateTime.MinValue;
							break;
						}
					}
				}
			}

			return systemEventLog;
		}

		public ISystemEventSetting GetSystemEventSettings()
		{
			var systemEventSetting = Create.New<ISystemEventSetting>();

			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspGetSystemEventSettings]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							systemEventSetting.PrepSystemEventApplicationId = row["PrepSystemEventApplicationID"] != DBNull.Value ? (int)row["PrepSystemEventApplicationID"] : 0;
							systemEventSetting.PublishSystemEventApplicationId = row["PublishSystemEventApplicationID"] != DBNull.Value ? (int)row["PublishSystemEventApplicationID"] : 0;
							systemEventSetting.SystemEventLogErrorTypeId = row["SystemEventLogErrorTypeID"] != DBNull.Value ? (int)row["SystemEventLogErrorTypeID"] : 0;
							systemEventSetting.SystemEventLogNoticeTypeId = row["SystemEventLogNoticeTypeID"] != DBNull.Value ? (int)row["SystemEventLogNoticeTypeID"] : 0;
							break;
						}
					}
				}
			}

			return systemEventSetting;
		}

		public IEnumerable<ISystemEvent> GetSystemEvents(int systemEventApplicationId, DateTime? endTime)
		{
			var systemEvents = new List<ISystemEvent>();
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspGetSystemEvents]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventApplicationID", systemEventApplicationId));
					command.Parameters.Add(new SqlParameter("@EndTime", endTime));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							var systemEvent = Create.New<ISystemEvent>();

							systemEvent.SystemEventId = row["SystemEventID"] != DBNull.Value ? (int)row["SystemEventID"] : 0;
							systemEvent.SystemEventApplicationId = row["SystemEventApplicationID"] != DBNull.Value ? (int)row["SystemEventApplicationID"] : 0;
							systemEvent.StartTime = row["StartTime"] != DBNull.Value ? (DateTime)row["StartTime"] : DateTime.MinValue;
							systemEvent.EndTime = row["EndTime"] != DBNull.Value ? (DateTime)row["EndTime"] : DateTime.MinValue;
							systemEvent.Duration = row["Duration"] != DBNull.Value ? (int)row["Duration"] : 0;
							systemEvent.CreatedDate = row["CreatedDate"] != DBNull.Value ? (DateTime)row["CreatedDate"] : DateTime.MinValue;
							systemEvent.PeriodId = row["PeriodID"] != DBNull.Value ? (int)row["PeriodID"] : 0;
							systemEvent.Completed = row["Completed"] != DBNull.Value ? (bool)row["Completed"] : false;

							systemEvents.Add(systemEvent);
						}
					}
				}
			}

			return systemEvents;
		}

		public IEnumerable<ISystemEvent> GetSystemEventsByPeriod(int periodId, int systemEventApplicationId)
		{
			var systemEvents = new List<ISystemEvent>();
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspGetSystemEventsByPeriod]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventApplicationID", systemEventApplicationId));
					command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					using (var sqlDataAdapter = new SqlDataAdapter(command))
					{
						var dataSet = new DataSet();
						sqlDataAdapter.Fill(dataSet);

						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							var systemEvent = Create.New<ISystemEvent>();

							systemEvent.SystemEventId = row["SystemEventID"] != DBNull.Value ? (int)row["SystemEventID"] : 0;
							systemEvent.SystemEventApplicationId = row["SystemEventApplicationID"] != DBNull.Value ? (int)row["SystemEventApplicationID"] : 0;
							systemEvent.StartTime = row["StartTime"] != DBNull.Value ? (DateTime)row["StartTime"] : DateTime.MinValue;
							systemEvent.EndTime = row["EndTime"] != DBNull.Value ? (DateTime)row["EndTime"] : DateTime.MinValue;
							systemEvent.Duration = row["Duration"] != DBNull.Value ? (int)row["Duration"] : 0;
							systemEvent.CreatedDate = row["CreatedDate"] != DBNull.Value ? (DateTime)row["CreatedDate"] : DateTime.MinValue;
							systemEvent.PeriodId = row["PeriodID"] != DBNull.Value ? (int)row["PeriodID"] : 0;
							systemEvent.Completed = row["Completed"] != DBNull.Value ? (bool)row["Completed"] : false;

							systemEvents.Add(systemEvent);
						}
					}
				}
			}

			return systemEvents;
		}

		public void InsertSystemEventLog(int systemEventId, int systemEventApplicationId, int eventKindId, string message)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[UspInsertSystemEventLog]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventID", systemEventId));
					command.Parameters.Add(new SqlParameter("@SystemEventApplicationID", systemEventApplicationId));
					command.Parameters.Add(new SqlParameter("@SystemEventLogTypeID", eventKindId));
					command.Parameters.Add(new SqlParameter("@EventMessage", message));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		public void OpenCommissionPeriod(int periodId)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspPublishOpenPeriod]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		public bool StartCommissionJob(string jobName)
		{
			try
			{
				using (var connection = CommissionsConnectionProvider.GetConnection())
				{
					using (var command = new SqlCommand("[dbo].[uspCommissionRunStartJob]", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.Add(new SqlParameter("@JobName", jobName));

						connection.Open();
						command.ExecuteNonQuery();
						connection.Close();

						return true;
					}
				}
			}
			catch (SqlException excp)
			{
				//This should be thrown if the job is already started
				//Check for exception text running just in case;
				if (excp.InnerException == null && !String.IsNullOrWhiteSpace(excp.Message) &&
					(excp.Message.Contains("job is already running") || excp.Message.Contains("job already has a pending request")))
				{
					return false;
				}

				//If the error was NOT due to job already running, throw the exception.
				throw;
			}
		}

		public int StartSystemEvent(int periodId, int systemEventApplicationId)
		{
			using (var connection = CommissionsConnectionProvider.GetConnection())
			{
				using (var command = new SqlCommand("[dbo].[uspStartSystemEvent]", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@SystemEventApplicationID", systemEventApplicationId));
					command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
					command.Parameters.Add(new SqlParameter("@SystemEventID", SqlDbType.Int, 4) { Direction = ParameterDirection.Output });

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					var value = command.Parameters["@SystemEventID"].Value;
					return value.ToString().Length == 0 ? 0 : Int32.Parse(value.ToString(), CultureInfo.InvariantCulture);
				}
			}
		}
	}
}
