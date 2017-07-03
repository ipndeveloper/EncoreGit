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
    public class EditSponsorController : BaseAccountsController
    {

        //[FunctionFilter("Accounts-Edit Sponsor", "~/Accounts/Overview")]
        [FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
        public virtual ActionResult Index(string currentAccountID)
        {
            try
            {
                if (!string.IsNullOrEmpty(currentAccountID))
                {
                    CoreContext.CurrentAccount = Account.LoadForSessionByAccountNumber(currentAccountID.ToString());
                }

                var model = new EditSponsorModel();

                model.OpenPeriodID = Periods.GetOpenPeriodID();

                model.Account = AccountSponsorBusinessLogic.Instance.GetAccountInformationEditSponsor(new AccountSponsorSearchParameters()
                {
                    AccountID   = CurrentAccount.AccountID,
                    PeriodID    = model.OpenPeriodID,
                    LanguageID  = CurrentLanguageID
                });

                if (model.Account != null)
                {
                    if (model.Account.SponsorID != -1)
                    {
                        model.AccountSponsor = AccountSponsorBusinessLogic.Instance.GetAccountInformationEditSponsor(new AccountSponsorSearchParameters()
                        {
                            AccountID = model.Account.SponsorID,
                            PeriodID = model.OpenPeriodID,
                            LanguageID = CurrentLanguageID
                        });
                    }
                    else
                    {
                        model.AccountSponsor = new AccountSponsorSearchData();
                    }
                }
                else
                {
                    model.Account = new AccountSponsorSearchData ();
                    model.AccountSponsor = new AccountSponsorSearchData();
                }
              Dictionary<string,string> periodos  =Periods.GetThreeNextPeriods(model.OpenPeriodID);

                if (model.Account.AccountStatusID == 2)// terminado  [solo se muestra el periodo actual]
                {
                    model.AviablePeriods = new Dictionary<string, string>() { { model.OpenPeriodID.ToString(), periodos[model.OpenPeriodID.ToString()] } };
                }
                //else if (model.Account.AccountStatusID == 3)//BEGUN ENRROlLMENT SOLO  SE CARGA  7 PERIDOS A APRTIR DE LA REACTIVACION LOS PERIODOS 
                //{
                //    Periods.GetThreeNextPeriodsReactiveAccount(model.OpenPeriodID, model.Account.AccountID);
                //}
                else {
                    // se muestra todos loes periofoss 
                    model.AviablePeriods = periodos;
                }
 

                
                                        
 
                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public ActionResult SearchSponsor(string query)
        {
            try
            {
                Dictionary<int, string> SponsorList = AccountCache.GetAccountSearchByTextResults(query).Where(x => x.Key != CurrentAccount.AccountID).ToDictionary( x => x.Key, x => x.Value );
                return Json(SponsorList.ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult GetSponsorAditionalValues(int SponsorID, int OpenPeriodID)
        {
            string actionResult = string.Empty;

            try
            {
                AccountSponsorSearchData Sponsor =  AccountSponsorBusinessLogic.Instance.GetAccountInformationEditSponsor(new AccountSponsorSearchParameters()
                {
                    AccountID = SponsorID,
                    PeriodID = OpenPeriodID
                });

                actionResult = JsonConvert.SerializeObject(Sponsor);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

            return Json(actionResult);
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetUpdateLog(int page, int pageSize)
        {
            try
            {
                page++;

                StringBuilder builder = new StringBuilder();
                var Log = AccountSponsorBusinessLogic.Instance.GetUpdateLogEditSponsor(new AccountSponsorLogSearchParameters()
                {
                    AccountID = CurrentAccount.AccountID,
                    PageNumber = page,
                    PageSize = pageSize
                });

                if (Log.Count > 0)
                {
                    int count = 0;
                    foreach (AccountSponsorLogSearchData entry in Log)
                    {
                        AppendAccountRow(builder, entry);
                        ++count;
                    }

                    int TotalPages = ((Log.TotalCount - 1) / pageSize) + 1;
                    return Json(new { totalPages = TotalPages, page = builder.ToString() });
                }
                else
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual void AppendAccountRow(StringBuilder builder, AccountSponsorLogSearchData entry)
        {
            builder.Append("<tr>")
                .AppendCell(entry.OldSponsorID.ToString())
                .AppendCell(entry.OldSponsorFirstName + " " + entry.OldSponsorLastName)
                .AppendCell(entry.NewSponsorID.ToString())
                .AppendCell(entry.NewSponsorFirstName + " " + entry.NewSponsorLastName)
                .AppendCell(entry.CampainStart)
                .AppendCell(entry.UpdateDate.ToString())
                // Inicio 06042017 --> comentado por IPN :  la funcionalidad solo formatea fecha para formato brazil
                // .AppendCell(String.Format("{0:dd/MM/yyyy}", entry.UpdateDate))
                // fin 06042017 --> 
                .AppendCell(entry.UpdateUser)
                .Append("</tr>");
        }

        public ActionResult UpdateSponsorInformation(AccountSponsorSearchParameters parameters)
        {
            string actionResult = string.Empty;

            try
            {
                if (CurrentAccount.AccountID == parameters.NewSponsorID) return Json(new { result = false, message = Translation.GetTerm("SelfSponsor", "The Sponsor can't be the same as the Consultant.") });
                parameters.AccountID = CurrentAccount.AccountID;
                parameters.LogParameters.CreatedUserID = CoreContext.CurrentUser.UserID;
                actionResult = AccountSponsorBusinessLogic.Instance.UpdateSponsorInformation(parameters);
                if (actionResult == string.Empty)
                {
                    AccountExtensions.UpdateAccountStatusByReEntryRules(CurrentAccount.AccountID);
                    return Json(new { result = true, message = Translation.GetTerm("SponsorUpdated", "Sponsor Actualizado.") });
                }
                else return Json(new { result = false, message = actionResult });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult TerminateConsultant()
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                message = AccountSponsorBusinessLogic.Instance.TerminateConsultant(CurrentAccount.AccountID);

                if (string.IsNullOrEmpty(message)){
                    result = true;
                    message = Translation.GetTerm("TerminateConsultantSuccess", "The consultant will be terminated at the end of the period.");
                }
            }
            catch (Exception ex)
            {
                message = Translation.GetTerm("TerminateConsultantError", "An error ocurred while terminating consultant") + ": " + ex.Message;
            }

            return Json(new { result = result, message = message });
        }
    }
}
