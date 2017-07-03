using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Web.Mvc.Helpers;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace nsCore.Areas.Accounts.Controllers
{
    public class AccountBlockingController : BaseAccountsController
    {
        //fsv
        // GET: /Accounts/AccountBlocking/

        public ActionResult Index()
        {
            var account = CoreContext.CurrentAccount;

            var model = BlockingType.GetAccountBlockingHistory(new BlockingTypeSearchParameters()
            {
                AccountID = account.AccountID,
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                LanguageID = CoreContext.CurrentLanguageID
            });

            AccountBlockingSearchData varBlocking = BlockingType.GetAccountIsLocked(new BlockingTypeSearchParameters()
            {
                AccountID = account.AccountID,
                LanguageID = CoreContext.CurrentLanguageID
            });
            Session["IsLocked"] = varBlocking.Description;

            var varStatus = new BlockingTypeSearchData();
            var ListaStatus = new List<BlockingTypeSearchData>();
            if (!varBlocking.IsLocked)
            {
                varStatus.StatusName = Translation.GetTerm("blocking", "Blocking"); ;
                varStatus.StatusID = 1;
                ListaStatus.Add(varStatus);
            }
            else
            {
                varStatus.StatusID = 0;
                varStatus.StatusName = Translation.GetTerm("unblocking", "Unblocking"); ;
                ListaStatus.Add(varStatus);
            }


            var mBlockingType = BlockingType.Get(new BlockingTypeSearchParameters()
            {
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                LanguageID = CoreContext.CurrentLanguageID,
                Enabled = true
            });

            ViewBag.BlockingType = new SelectList(mBlockingType.AsEnumerable(), "AccountBlockingTypeID", "Name", 3);
            ViewBag.ListaStatus = new SelectList(ListaStatus.AsEnumerable(), "StatusID", "StatusName", 3);
            ViewBag.IsBlocking = varBlocking.IsLocked;
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                var account = CoreContext.CurrentAccount;

                var blockingHistoryType = BlockingType.GetAccountBlockingHistory(new BlockingTypeSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    AccountID = account.AccountID,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    LanguageID = CoreContext.CurrentLanguageID
                });
                foreach (var blockingTypes in blockingHistoryType)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.DateCreatedUTC));
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.BlockTypeName));
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.BlockSubTypeName));
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.Reasons));
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.UserName));
                    if (blockingTypes.StatusName == "Blocking")
                    {
                        builder.Append(String.Format("<td style='color:red'>{0}</td>", blockingTypes.StatusName));
                    }
                    else { builder.Append(String.Format("<td style='color:Green'>{0}</td>", blockingTypes.StatusName)); }

                    // builder.Append(String.Format("<td><a href='javascript:void(0)' class='blockingTypeDetail' new-id='{0}' new-name='{1}' new-enabled='{2}' >{3}</a></td>", blockingTypes.AccountBlockingTypeID, blockingTypes.Name, blockingTypes.Enabled, blockingTypes.Name));
                    // builder.Append(String.Format("<td>{0}</td>", blockingTypes.Enabled ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive")));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = blockingHistoryType.TotalPages, page = blockingHistoryType.TotalCount == 0 ? "<tr><td colspan=\"2\">There are no periods</td></tr>" : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Save(string AccountBlockingTypeID, string AccountBlockingSubTypeID, string Reasons, bool Status)
        {
            try
            {
                // Se invocara al método de la capa lógica y se retorna un json de la siguiente forma
                var parameters = new BlockingTypeSearchParameters();

                var account = CoreContext.CurrentAccount;

                parameters.AccountID = account.AccountID;
                parameters.AccountBlockingTypeID = string.IsNullOrEmpty(AccountBlockingTypeID) ?  (short?)null: Convert.ToInt16(AccountBlockingTypeID);
                parameters.AccountBlockingSubTypeID = string.IsNullOrEmpty(AccountBlockingSubTypeID)?(short?)null:Convert.ToInt16(AccountBlockingSubTypeID);
                parameters.Reasons = Reasons;
                parameters.CreateByUserID = CoreContext.CurrentUser.UserID;
                parameters.DateCreatedUTC = DateTime.UtcNow;
                parameters.IsLocked = Status;
               
                int newAccountBLockingHistoryID = BlockingType.SaveAccountBlockingHistory(parameters);
                return Json(new { result = true, menssage = Translation.GetTerm("SavedSuccessfully"), newID = newAccountBLockingHistoryID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, menssage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBlokingSubTypeList(short intBlockingTypeID)
        {

            var mBlockingSubType = BlockingSubType.Get(new BlockingSubTypeSearchParameters()
            {
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                LanguageID = CoreContext.CurrentLanguageID,
                Enabled = true,
                AccountBlockingTypeID = intBlockingTypeID
            });

            //ViewBag.BlockingSubType = new SelectList(mBlockingSubType.AsEnumerable(), "AccountBlockingSubTypeID", "Name", 3);                      

            return Json(mBlockingSubType, JsonRequestBehavior.AllowGet);
        }
    }
}
