using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace nsCore.Areas.Admin.Controllers
{
    public class BlockingSubTypeController : Controller
    {
        //
        // GET: /Admin/BlockingSubType/

        public ActionResult Index()
        {

            var model = BlockingType.Get(new BlockingTypeSearchParameters()
            {
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                LanguageID = CoreContext.CurrentLanguageID
            });

            var vListTypeProcess = BlockingSubType.ListTypeProcess(new BlockingSubTypeSearchParameters()
            {              
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                LanguageID = CoreContext.CurrentLanguageID
            });

            ViewBag.BlockingType = new SelectList(model.AsEnumerable(), "AccountBlockingTypeID", "Name", 3);
            ViewBag.BlockingProcess = new SelectList(vListTypeProcess.AsEnumerable(), "AccountBlockingProcessID", "Description", 3);
            return View();
        }
    
        [OutputCache(CacheProfile = "DontCache")]
        public virtual JsonResult GetTypeProcess()
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                var vListTypeProcess = BlockingSubType.ListTypeProcess(new BlockingSubTypeSearchParameters()
                {
                 
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    LanguageID = CoreContext.CurrentLanguageID
                });


                return Json(vListTypeProcess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }



        [OutputCache(CacheProfile = "DontCache")]
        public virtual JsonResult GetTypeProcessBlockingSubTypeID(Int16 AccountBlockingSubTypeID)
        {
            try
            {
                var blockingProcess = BlockingSubType.GetAccountSubTypeProcess(new BlockingSubTypeSearchParameters()
                {
                   AccountBlockingSubTypeID=AccountBlockingSubTypeID,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    LanguageID = CoreContext.CurrentLanguageID
                });

                string cadBlockingProcess = "";

                foreach (var blockingSubTypeProcess in blockingProcess)
                {
                    cadBlockingProcess += blockingSubTypeProcess.AccountBlockingProcessID.ToString() + ",";
                }
                if (cadBlockingProcess.Length > 0) {
                    cadBlockingProcess = cadBlockingProcess.Substring(0, cadBlockingProcess.Length - 1);
                }
             


                return Json(cadBlockingProcess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                var blockingSubTypes = BlockingSubType.Get(new BlockingSubTypeSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    LanguageID = CoreContext.CurrentLanguageID
                });


                foreach (var blockingSubType in blockingSubTypes)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td><a href='javascript:void(0)' class='blockingTypeDetail' new-id='{0}' new-name='{1}' new-enabled='{2}' new-subtypeID='{4}'  >{3}</a></td>", blockingSubType.AccountBlockingTypeID, blockingSubType.Name, blockingSubType.Enabled, blockingSubType.Name,blockingSubType.AccountBlockingSubTypeID));
                    builder.Append(String.Format("<td>{0}</td>", blockingSubType.BlockingType));
                    builder.Append(String.Format("<td>{0}</td>", blockingSubType.Enabled ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive")));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = blockingSubTypes.TotalPages, page = blockingSubTypes.TotalCount == 0 ? "<tr><td colspan=\"2\">There are no periods</td></tr>" : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Save(string AccountBlockingSubTypeID, string AccountBlockingTypeID, string Name, int? Status,string ListaBlockAccess)
        {
            try
            {
                string[] valuesBlockID = ListaBlockAccess.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                // Se invocara al método de la capa lógica y se retorna un json de la siguiente forma
                var parameters = new BlockingSubTypeSearchParameters();

                parameters.AccountBlockingSubTypeID = Convert.ToInt16(AccountBlockingSubTypeID);// Nuevo tipo de bloqueo
                parameters.AccountBlockingTypeID = Convert.ToInt16(AccountBlockingTypeID); 
                parameters.LanguageID = CoreContext.CurrentLanguageID;
                parameters.Name = Name;
                parameters.Enabled = Convert.ToBoolean(Status);
                parameters.ListaBlockingProcess = ListaBlockAccess;

                Int16 newAccountBLockingSubTypeID = BlockingSubType.Save(parameters);
                return Json(new { result = true, menssage = Translation.GetTerm("SavedSuccessfully"), newID = newAccountBLockingSubTypeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, menssage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
