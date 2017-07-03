using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Serialization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;

namespace DistributorBackOffice.Controllers
{
	using NetSteps.Common.Configuration;

	/// <summary>
	/// Author: John Egbert
	/// Description: Base Class for AccountReports; T is the report data/parameters that gets serialized back to DB. 
	/// Created: 11-22-2010
	/// </summary>
	public abstract class AccountReportController<T> : BaseAccountsController
		where T : class, IPrimaryKey<int>, new()
	{
		#region Properties
		public virtual List<AccountReport> CurrentAccountReports
		{
			get
			{
				var currentAccountReports = Session["CurrentAccountReports"] as List<AccountReport>;
				if (currentAccountReports == null && CurrentAccount != null && CurrentAccount.AccountID > 0)
				{
					currentAccountReports = Account.LoadAccountReports(CurrentAccount.AccountID);
					Session["CurrentAccountReports"] = currentAccountReports;
				}

				return currentAccountReports;
			}
			set
			{
				Session["CurrentAccountReports"] = value;
			}
		}

		public virtual List<AccountReport> CorporateAccountReports
		{
			get
			{
				var corporateAccountReports = Session["CorporateAccountReports"] as List<AccountReport>;
				if (corporateAccountReports == null)
				{
					corporateAccountReports = Account.LoadCorporateReports();
					Session["CorporateAccountReports"] = corporateAccountReports;
				}

				return corporateAccountReports;
			}
			set
			{
				Session["CorporateAccountReports"] = value;
			}
		}

		public virtual T CurrentReportParameters
		{
			get
			{
				T downlineReportParameters = Session["CurrentReportParameters"] as T;
				if (downlineReportParameters == null)
				{
					downlineReportParameters = GetDefaultReportParameters();
				}

				return downlineReportParameters;
			}
			set
			{
				Session["CurrentReportParameters"] = value;
			}
		}

		public abstract Constants.AccountReportType AccountReportType { get; }
		#endregion

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Reports", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveReport(string reportName)
		{
			try
			{
				if (CurrentReportParameters != null && CurrentAccount != null)
				{
					var userIsCorporateAccount = CurrentAccount.AccountID == ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateAccountID);
					var accountReportList = userIsCorporateAccount ? CorporateAccountReports : CurrentAccountReports;

					int count = accountReportList.FindAll(x => x.Name.ToLower() == reportName.ToLower() || x.Name.ToLower().StartsWith(reportName.ToLower())).Count;

					AccountReport accountReport = null;
					if (CurrentReportParameters.PrimaryKey > 0)
					{
						accountReport =
							accountReportList.FirstOrDefault(
								r => r.AccountReportID == CurrentReportParameters.PrimaryKey);
						if (accountReport == null)
						{
							accountReport = AccountReport.Load(CurrentReportParameters.PrimaryKey);
							if (accountReport != null)
							{
								var newReport = new AccountReport
								{
									AccountID = CurrentAccount.AccountID,
									AccountReportTypeID = accountReport.AccountReportTypeID,
									DateCreated = DateTime.Now,
									Data =
										BinarySerializationHelper.Serialize(CurrentReportParameters),
									Name = reportName,
									IsCorporate = userIsCorporateAccount
								};
								accountReportList.Add(newReport);
								newReport.Save();

								return Json(new { result = true, accountReportID = newReport.AccountReportID });
							}
						}
						if (count > 1)
						{
							reportName += count;
						}
					}
					else
					{
						if (count >= 1)
						{
							reportName += count;
						}
						accountReport = new AccountReport();
						accountReportList.Add(accountReport);
					}

					accountReport.StartTracking();
					accountReport.AccountReportTypeID = AccountReportType.ToShort();
					accountReport.AccountID = CurrentAccount.AccountID;
					accountReport.Name = reportName;
					accountReport.Data = BinarySerializationHelper.Serialize(CurrentReportParameters);
					accountReport.DateCreated = DateTime.Now;
					accountReport.IsCorporate = userIsCorporateAccount;
					accountReport.Save();

					if (userIsCorporateAccount)
					{
						CorporateAccountReports = accountReportList;
					}
					else
					{
						CurrentAccountReports = accountReportList;
					}

					return Json(new { result = true, accountReportID = accountReport.AccountReportID });
				}
				return Json(new { result = false, message = Translation.GetTerm("YourSessionHasTimedOutPleaseRefreshthePage", "Your session has timed out.  Please refresh the page") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Reports", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult DeleteReport(int accountReportID)
		{
			try
			{
				if (CurrentAccount != null)
				{
					// Check that the report belongs to the current account to be able to delete it. - JHE
					AccountReport accountReport = null;
					if (accountReportID > 0)
						accountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);

					if (accountReport != null)
					{
						accountReport.Delete();
						CurrentAccountReports.Remove(accountReport);
					}

					CurrentAccountReports = CurrentAccountReports; // Save back to session - JHE

					return Json(new { result = true });
				}
				return Json(new { result = false, message = Translation.GetTerm("YourSessionHasTimedOutPleaseRefreshthePage", "Your session has timed out.  Please refresh the page") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Override this to default ReportParameters for new reports. - JHE
		/// </summary>
		/// <returns></returns>
		protected virtual T GetDefaultReportParameters()
		{
			return new T();
		}
	}
}