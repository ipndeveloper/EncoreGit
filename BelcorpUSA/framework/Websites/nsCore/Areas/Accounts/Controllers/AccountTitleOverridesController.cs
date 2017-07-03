using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Shared;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;
using nsCore.Areas.Accounts.Models.AccountTitleOverrides;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Accounts.Controllers
{
	public class AccountTitleOverridesController : BaseAccountsController
	{
		protected virtual string BuildResultRows(AccountTitleOverrideSearchParameters listContext, out IAccountTitleOverrideSearchResult accountTitleOverrides)
		{
			var builder = new StringBuilder();
			accountTitleOverrides = CommissionsService.SearchAccountTitleOverrides(listContext);
			var cssClass = "GridRow";
			if (accountTitleOverrides != null && accountTitleOverrides.Results != null && accountTitleOverrides.Results.Any())
			{
				foreach (var titleOverride in accountTitleOverrides.Results)
				{
					BuildRow(builder, cssClass, titleOverride);
					cssClass = cssClass == "GridRow" ? "GridRowAlt" : "GridRow";
				}
			}
			else
			{
				builder.Append("<tr>")
					.AppendCell(Translation.GetTerm("Commissions_NoData_OrServiceUnavailable", "No data was returned, either because none exists or the service is unavailable."), columnSpan: 9)
					.Append("</tr>");
			}
			return builder.ToString();
		}

		protected virtual void BuildRow(StringBuilder builder, string cssClass, IAccountTitleOverride titleOverride)
		{
			int periodMonth = titleOverride.Period.PeriodId % 100;
			int periodYear = (titleOverride.Period.PeriodId - periodMonth) / 100;

			var currentAccountTitle = CommissionsService.GetAccountTitle(CurrentAccount.AccountID, titleOverride.OverrideTitleKind.TitleKindId, titleOverride.Period.PeriodId);
			var currentAccountTitleId = currentAccountTitle == null ? 1 : currentAccountTitle.TitleId;
			var currentTitle = CommissionsService.GetTitle(currentAccountTitleId);
			var overriddingUser = NetSteps.Data.Entities.User.Load(titleOverride.UserId);

			builder.Append("<tr id=\"name").Append(titleOverride.AccountTitleOverrideId).Append("\" class=\"").Append(cssClass).Append("\">")
				   .AppendCheckBoxCell(cssClass: "alertSelector", value: titleOverride.AccountTitleOverrideId.ToString())
				   .AppendLinkCell("~/Accounts/AccountTitleOverrides/Edit/" + titleOverride.AccountTitleOverrideId, String.Format("{0}-{1:D2}", periodYear, periodMonth))
				   .AppendCell(Translation.GetTerm(currentTitle.TermName))
				   .AppendCell(Translation.GetTerm(titleOverride.OverrideTitle.TermName))
				   .AppendCell(Translation.GetTerm(titleOverride.OverrideTitleKind.TermName))
				   .AppendCell(Translation.GetTerm(titleOverride.OverrideReason.TermName, titleOverride.OverrideReason.Name))
				   .AppendCell(overriddingUser.Username)
				   .AppendCell(titleOverride.CreatedDateUTC.ToLocalTime().ToString("g"))
				   .AppendCell(titleOverride.UpdatedDateUTC.ToLocalTime().ToString("g"))
				   .Append("</tr>");
		}

		[FunctionFilter("Accounts-Title Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Index()
		{
			return View(new nsCore.Areas.Accounts.Models.AccountTitleOverrides.AccountTitleOverrideIndexModel());
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Title Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Save(int? overrideReasonID, int? titleID,
			int? titleTypeID, string notes, int? accountTitleOverrideID, int? periodID)
		{
			try
			{
				if (!titleID.HasValue
					|| !titleTypeID.HasValue
					|| String.IsNullOrEmpty(notes)
					|| !overrideReasonID.HasValue
					|| !accountTitleOverrideID.HasValue
					|| !periodID.HasValue)
				{
					return Json(new { result = false });
				}

				IAccountTitleOverride acctTitleOverride;

				if (accountTitleOverrideID.HasValue && accountTitleOverrideID.Value > 0)
				{
					acctTitleOverride = CommissionsService.GetAccountTitleOverride(accountTitleOverrideID.Value);
					if (acctTitleOverride == null)
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_AccountTitleOverRide_NotFound", "Account Title Override for Id {0} was not found."), accountTitleOverrideID.Value) });
					}
					if (!acctTitleOverride.IsEditable)
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_AccountTitleOverRide_NotEditable", "Account Title Override for Period {0}, Type {1} is no longer editable."), acctTitleOverride.Period.PeriodId, Translation.GetTerm(acctTitleOverride.OverrideTitleKind.TermName)) });
					}
				}
				else
				{
					acctTitleOverride = Create.New<IAccountTitleOverride>();
					acctTitleOverride.CreatedDateUTC = DateTime.Now.ApplicationNow().ToUniversalTime();
				}

				acctTitleOverride.Notes = notes;
				acctTitleOverride.Period = CommissionsService.GetPeriod((int)periodID);
				acctTitleOverride.AccountId = CoreContext.CurrentAccount.AccountID;
				acctTitleOverride.UserId = CoreContext.CurrentUser.UserID;
				acctTitleOverride.ApplicationSourceId = ApplicationContext.Instance.ApplicationID;
				acctTitleOverride.OverrideReason = CommissionsService.GetOverrideReasons().FirstOrDefault(x => x.OverrideReasonId == (int)overrideReasonID);
				acctTitleOverride.OverrideTitle = CommissionsService.GetTitle((int)titleID);
				acctTitleOverride.OverrideTitleKind = CommissionsService.GetTitleKind((int)titleTypeID);
				acctTitleOverride.UpdatedDateUTC = DateTime.Now.ApplicationNow().ToUniversalTime();

				if (acctTitleOverride.AccountTitleOverrideId > 0)
				{
					if (!CommissionsService.DeleteAccountTitleOverride(acctTitleOverride.AccountTitleOverrideId))
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_AccountTitleOverRide_UpdatedFailed", "Account Title Override for Period {0}, Type {1} failed to update."), acctTitleOverride.Period.PeriodId, Translation.GetTerm(acctTitleOverride.OverrideTitleKind.TermName)) });
					}
				}

				CommissionsService.AddAccountTitleOverride(acctTitleOverride);

				return Json(new { result = true });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpGet]
		[FunctionFilter("Accounts-Title Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Edit(int? id)
		{
			IAccountTitleOverride innerModel;
			if (id.HasValue)
			{
				innerModel = CommissionsService.GetAccountTitleOverride(id.Value);
			}
			else
			{
				innerModel = Create.New<IAccountTitleOverride>();
				innerModel.Period = CommissionsService.GetCurrentPeriod();
			}

			return View(new AccountTitleOverrideEditModel(innerModel));
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Title Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Get(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection, DateTime startDate, DateTime endDate, int? overrideReasonID)
		{
			var listContext = new AccountTitleOverrideSearchParameters()
			{
				AccountId = CoreContext.CurrentAccount.AccountID,
				OverrideReasonId = overrideReasonID,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection,
				StartDate = startDate,
				EndDate = endDate,
				PageIndex = page,
				PageSize = pageSize
			};

			IAccountTitleOverrideSearchResult accountTitleOverrides;
			var results = BuildResultRows(listContext, out accountTitleOverrides);
			var tpages = (accountTitleOverrides != null && accountTitleOverrides.Results != null) ? Math.Ceiling(accountTitleOverrides.Results.Count() / (double)pageSize) : 0;
			return Json(new { totalPages = tpages, page = results }, JsonRequestBehavior.AllowGet);
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Title Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Delete(List<int> items)
		{
			try
			{
				if (items != null)
				{
					foreach (var itemId in items)
					{
						var accountTitleOverride = CommissionsService.GetAccountTitleOverride(itemId);

						if (accountTitleOverride.IsEditable)
						{
							CommissionsService.DeleteAccountTitleOverride(itemId);
						}
					}
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

	}
}
