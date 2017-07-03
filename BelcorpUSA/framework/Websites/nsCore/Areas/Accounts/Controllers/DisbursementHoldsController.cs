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
using nsCore.Areas.Accounts.Models.DisbursementHolds;

namespace nsCore.Areas.Accounts.Controllers
{
	public class DisbursementHoldsController : BaseAccountsController
	{
		protected virtual string BuildResultRows(DisbursementHoldSearchParameters listContext, out IDisbursementHoldSearchResult checkHolds)
		{
			var builder = new StringBuilder();
			checkHolds = CommissionsService.SearchDisbursementHolds(listContext);
			var cssClass = "GridRow";
              
			foreach (var calcOverride in checkHolds.Results)
			{
				BuildRow(builder, cssClass, calcOverride);
				cssClass = cssClass == "GridRow" ? "GridRowAlt" : "GridRow";
			}
			return builder.ToString();
		}

		protected virtual void BuildRow(StringBuilder builder, string cssClass, IDisbursementHold checkHold)
		{
			var user = NetSteps.Data.Entities.User.Load(checkHold.UserId);

			builder.Append("<tr id=\"name").Append(checkHold.DisbursementHoldId).Append("\" class=\"").Append(cssClass).Append("\">")
				.AppendLinkCell("~/Accounts/DisbursementHolds/Edit/" + checkHold.DisbursementHoldId, checkHold.Reason.Name)
				   .AppendCell(checkHold.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
				   .AppendCell(checkHold.HoldUntil != null ? checkHold.HoldUntil.Value.ToString("g") : string.Empty)
				   .AppendCell(user.Username)

                   .AppendCell(checkHold.CreatedDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                   .AppendCell(checkHold.UpdatedDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                   //.AppendCell(checkHold.CreatedDate.ToString("g"))
                   //.AppendCell(checkHold.UpdatedDate.ToString("g"))
				   .Append("</tr>");
		}

		[FunctionFilter("Accounts-Disbursement Holds", "~/Accounts/Overview")]
		public virtual ActionResult Index()
		{
			return View(new DisbursementHoldsIndexModel());
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Disbursement Holds", "~/Accounts/Overview")]
		public virtual ActionResult Save(int? overrideReasonID, DateTime? holdUntil,
			DateTime? startDate, string note, int? disbursementHoldId)
		{
			try
			{
				if (!holdUntil.HasValue || !startDate.HasValue || String.IsNullOrEmpty(note) ||
					!overrideReasonID.HasValue || !disbursementHoldId.HasValue)
					return Json(new { result = false }, JsonRequestBehavior.AllowGet);

				IDisbursementHold disbursementHold;

				if (disbursementHoldId.HasValue && disbursementHoldId.Value > 0)
				{
					disbursementHold = CommissionsService.GetDisbursementHold(disbursementHoldId.Value);
				}
				else
				{
					disbursementHold = Create.New<IDisbursementHold>();
					disbursementHold.CreatedDate = DateTime.Now.ApplicationNow();
				}

				disbursementHold.Notes = note;
				disbursementHold.AccountId = CoreContext.CurrentAccount.AccountID;
				disbursementHold.UserId = CoreContext.CurrentUser.UserID;
				disbursementHold.ApplicationSourceId = ApplicationContext.Instance.ApplicationID;
				disbursementHold.Reason = CommissionsService.GetOverrideReasons().FirstOrDefault(x => x.OverrideReasonId == (int)overrideReasonID);
				disbursementHold.HoldUntil = holdUntil.IsNullOrEmpty() ? (DateTime?)null : holdUntil;
				disbursementHold.StartDate = (DateTime)startDate;
				disbursementHold.UpdatedDate = DateTime.Now.ApplicationNow();

				CommissionsService.AddDisbursementHold(disbursementHold);

				return Json(new { result = true });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpGet]
		[FunctionFilter("Accounts-Disbursement Holds", "~/Accounts/Overview")]
		public virtual ActionResult Edit(int? id)
		{
			IDisbursementHold disbursementHold;
			if (id.HasValue)
			{
				disbursementHold = CommissionsService.GetDisbursementHold(id.Value);
			}
			else
			{
				disbursementHold = Create.New<IDisbursementHold>();
				disbursementHold.AccountId = CurrentAccount.AccountID;
				disbursementHold.ApplicationSourceId = ApplicationContext.Instance.ApplicationID;
				disbursementHold.CreatedDate = DateTime.Now.ApplicationNow();
				disbursementHold.HoldUntil = disbursementHold.CreatedDate.AddMonths(3);
				disbursementHold.StartDate = disbursementHold.CreatedDate;
				disbursementHold.UserId = ApplicationContext.Instance.CurrentUserID;
			}
			return View(new DisbursementHoldsEditModel(disbursementHold));
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Disbursement Holds", "~/Accounts/Overview")]
		public virtual ActionResult Get(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection, DateTime startDate, DateTime endDate, int? overrideReasonID)
		{
			var listContext = new DisbursementHoldSearchParameters
			{
				AccountId = CoreContext.CurrentAccount.AccountID,
				ReasonId = overrideReasonID,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection,
				StartDate = startDate,
				EndDate = endDate,
				PageIndex = page,
				PageSize = pageSize
			};

			IDisbursementHoldSearchResult disbursementHolds;
			var results = BuildResultRows(listContext, out disbursementHolds);
			return Json(new { totalPages = Math.Ceiling(disbursementHolds.Results.Count() / (double)pageSize), page = results.ToString() }, JsonRequestBehavior.AllowGet);
		}
	}
}
