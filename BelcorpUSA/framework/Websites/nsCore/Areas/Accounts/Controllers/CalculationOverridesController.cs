using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;
using nsCore.Areas.Accounts.Models.CalculationOverrides;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Accounts.Controllers
{
	public class CalculationOverridesController : BaseAccountsController
	{
		protected virtual string BuildResultRows(CalculationOverrideSearchParameters listContext, out ICalculationOverrideSearchResult calculationOverrides)
		{
			var builder = new StringBuilder();
			calculationOverrides = CommissionsService.SearchCalculationOverrides(listContext);
			var cssClass = "GridRow";

			if (calculationOverrides != null && calculationOverrides.Results != null && calculationOverrides.Results.Any())
			{
				foreach (var calcOverride in calculationOverrides.Results)
				{
					BuildRow(builder, cssClass, calcOverride);
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

		protected virtual void BuildRow(StringBuilder builder, string cssClass, ICalculationOverride calcOverride)
		{
			int periodMonth = calcOverride.Period.PeriodId % 100;
			int periodYear = (calcOverride.Period.PeriodId - periodMonth) / 100;

			builder.Append("<tr id=\"name").Append(calcOverride.CalculationOverrideId).Append("\" class=\"").Append(cssClass).Append("\">")
				.AppendCheckBoxCell(cssClass: "alertSelector", value: calcOverride.CalculationOverrideId.ToString())
				.AppendLinkCell("~/Accounts/CalculationOverrides/Edit/" + calcOverride.CalculationOverrideId
					, String.Format("{0}-{1:D2}", periodYear, periodMonth))
				.AppendCell(calcOverride.CalculationKind.Name)
				.AppendCell(calcOverride.OverrideKind.Description)
                .AppendCell(calcOverride.NewValue.ToDouble().ToString(CoreContext.CurrentCultureInfo))
                //.AppendCell(calcOverride.NewValue.ToDouble().TruncateDoubleInsertCommas(2))
				.AppendCell(calcOverride.OverrideReason.Name)
				.AppendCell(NetSteps.Data.Entities.User.Load(calcOverride.UserId).Username)
				.AppendCell(calcOverride.CreatedDateUTC.ToLocalTime().ToString("g"))
				.AppendCell(calcOverride.UpdatedDateUTC.ToLocalTime().ToString("g"))
				.Append("</tr>");
		}

		[FunctionFilter("Accounts-Calculation Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Index()
		{
			return View(new CalculactionOverridesIndexModel());
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Calculation Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Get(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection, DateTime startDate, DateTime endDate, int? overrideReasonID)
		{
			var listContext = new CalculationOverrideSearchParameters()
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

			ICalculationOverrideSearchResult calculationOverrides;
			var results = BuildResultRows(listContext, out calculationOverrides);
			var tpages = (calculationOverrides != null && calculationOverrides.Results != null) ? Math.Ceiling(calculationOverrides.Results.Count() / (double)pageSize) : 0;
			return Json(new { totalPages = tpages, page = results.ToString() }, JsonRequestBehavior.AllowGet);
		}

		[FunctionFilter("Accounts-Calculation Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Edit(int? id)
		{
			ICalculationOverride calculationOverride;
			if (id.HasValue)
			{
				calculationOverride = CommissionsService.GetCalculationOverride(id.Value) ?? Create.New<ICalculationOverride>();
			}
			else
			{
				calculationOverride = Create.New<ICalculationOverride>();
				calculationOverride.AccountId = CurrentAccount.AccountID;
				calculationOverride.ApplicationSourceId = ApplicationContext.Instance.ApplicationID;
				calculationOverride.CreatedDateUTC = DateTime.UtcNow;
				calculationOverride.Period = CommissionsService.GetCurrentPeriod();
			}
			var model = new CalculationOverridesEditModel(calculationOverride);
			return View(model);
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Calculation Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Save(int? calculationTypeID, int? overrideTypeID, decimal? calculationValue, int? overrideReasonID, string note, int? periodID, int? calculationOverrideID)
		{
			try
			{
				if (!calculationTypeID.HasValue
					|| !calculationValue.HasValue
					|| String.IsNullOrEmpty(note)
					|| !overrideTypeID.HasValue
					|| !overrideReasonID.HasValue
					|| !periodID.HasValue)
				{
					return Json(new { result = false }, JsonRequestBehavior.AllowGet);
				}
				ICalculationOverride calcOverride;

				if (calculationOverrideID.HasValue && calculationOverrideID.Value > 0)
				{
					calcOverride = CommissionsService.GetCalculationOverride(calculationOverrideID.Value);
					if (calcOverride == null)
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_CalculationOverRide_NotFound", "Calculation Override for Id {0} was not found."), calculationOverrideID.Value) });
					}
					if (!calcOverride.IsEditable)
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_CalculationOverRide_NotEditable", "Calculation Override for Period {0}, Type {1} is no longer editable."), calcOverride.Period.PeriodId, Translation.GetTerm(calcOverride.CalculationKind.TermName)) });
					}
				}
				else
				{
					calcOverride = Create.New<ICalculationOverride>();
					calcOverride.CreatedDateUTC = DateTime.Now.ApplicationNow().ToUniversalTime();
				}

				calcOverride.Notes = note;
				calcOverride.AccountId = CoreContext.CurrentAccount.AccountID;
				calcOverride.UserId = CoreContext.CurrentUser.UserID;
				calcOverride.ApplicationSourceId = NetSteps.Data.Entities.ApplicationContext.Instance.ApplicationID;
				calcOverride.OverrideReason = CommissionsService.GetOverrideReasons().FirstOrDefault(x => x.OverrideReasonId == (int)overrideReasonID);
				calcOverride.OverrideKind = CommissionsService.GetOverrideKinds().FirstOrDefault(x => x.OverrideKindId == (int)overrideTypeID);
				calcOverride.CalculationKind = CommissionsService.GetCalculationKinds().FirstOrDefault(x => x.CalculationKindId == (int)calculationTypeID);
				calcOverride.NewValue = (decimal)calculationValue;
				calcOverride.Period = CommissionsService.GetPeriod(periodID.Value);
				calcOverride.UpdatedDateUTC = DateTime.Now.ApplicationNow().ToUniversalTime();

				if (calcOverride.CalculationOverrideId > 0)
				{
					if (!CommissionsService.DeleteCalculationOverride(calcOverride.CalculationOverrideId))
					{
						return Json(new { result = false, message = String.Format(Translation.GetTerm("GMP_CalculationOverRide_UpdateFailed", "Calculation Override for Period {0}, Type {1} failed to update."), calcOverride.Period.PeriodId, Translation.GetTerm(calcOverride.CalculationKind.TermName)) });
					}
				}

				CommissionsService.AddCalculationOverride(calcOverride);

				return Json(new { result = true });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Calculation Overrides", "~/Accounts/Overview")]
		public virtual ActionResult Delete(List<int> items)
		{
			try
			{
				if (items != null)
				{
					foreach (var itemId in items)
					{
						var calculationOverride = CommissionsService.GetCalculationOverride(itemId);

						if (calculationOverride.IsEditable)
						{
							CommissionsService.DeleteCalculationOverride(itemId);
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
