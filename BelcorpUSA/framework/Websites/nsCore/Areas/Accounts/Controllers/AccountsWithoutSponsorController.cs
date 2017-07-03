using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.Extensions;
using Newtonsoft.Json;
using System.Text;
using NetSteps.Web.Extensions;

namespace nsCore.Areas.Accounts.Controllers
{
    public class AccountsWithoutSponsorController : BaseAccountsController
    {

        [FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
        public virtual ActionResult Index()
        {
            try
            {
                var ActivePlan = TemporalMatrixEstensions.LkpSystemConfigValue("ActivePlan");
                int ActivePlanID = ActivePlan == string.Empty ? -1 : Int32.Parse(ActivePlan);

                var periods = Periods.Search().FindAll(x => x.PlanID == ActivePlanID);

                var model = new AccountsWithoutSponsorModel();
                model.SponsorIDValidation = OrderExtensions.GeneralParameterVal(56, "BEL") == string.Empty ? false : true;
                model.AviablePeriods = periods == null ? new Dictionary<int, string>() : periods.OrderByDescending(x => x.PeriodID).ToDictionary(p => p.PeriodID, p => p.PeriodID.ToString());
                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetAccounts(int page, int pageSize, int? accountId, int periodID)
        {
            try
            {
                page++;

                StringBuilder builder = new StringBuilder();
                var Accounts = new AccountBusinessLogic().GetAccountsWithoutSponsor(new AccountWithoutSponsorSearchParameters()
                {
                    AccountID = accountId ?? 0,
                    PeriodID = periodID,
                    PageNumber = page,
                    PageSize = pageSize
                });

                if (Accounts.Count > 0)
                {
                    int count = 0;
                    foreach (AccountWithoutSponsorSearchData entry in Accounts)
                    {
                        entry.AccountStatus = Translation.GetTerm(entry.AccountStatusTerm, string.Empty);
                        AppendAccountRow(builder, entry);
                        ++count;
                    }

                    int TotalPages = ((Accounts.TotalCount - 1) / pageSize) + 1;
                    return Json(new { totalPages = TotalPages, page = builder.ToString() });
                }
                else
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"10\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual void AppendAccountRow(StringBuilder builder, AccountWithoutSponsorSearchData entry)
        {
                builder.Append("<tr>")
                .AppendLinkCell("/Accounts/EditSponsor/Index?currentAccountID=" + entry.AccountID.ToString(), entry.AccountID.ToString())
                .AppendCell(entry.Name)
                .AppendCell(entry.AccountStatus)
                .AppendCell(entry.PeriodID)
                .AppendCell(entry.City)
                .AppendCell(entry.State)
                .AppendCell(entry.Email)
                .AppendCell(entry.Address)
                .AppendCell(entry.Telephone1)
                .AppendCell(entry.Telephone2)
                .Append("</tr>");
        }

    }
}
