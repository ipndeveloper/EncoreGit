using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Admin.Controllers
{
    public class BlockingTypeController : Controller
    {
        //
        // GET: /Admin/BlockingType/

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                var blockingType = BlockingType.Get(new BlockingTypeSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    LanguageID = CoreContext.CurrentLanguageID
                });
                foreach (var blockingTypes in blockingType)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td><a href='javascript:void(0)' class='blockingTypeDetail' new-id='{0}' new-name='{1}' new-enabled='{2}' >{3}</a></td>", blockingTypes.AccountBlockingTypeID, blockingTypes.Name, blockingTypes.Enabled, blockingTypes.Name));                   
                    builder.Append(String.Format("<td>{0}</td>", blockingTypes.Enabled ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive")));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = blockingType.TotalPages, page = blockingType.TotalCount == 0 ? "<tr><td colspan=\"2\">There are no History Blocking</td></tr>" : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        // Grabar 
        public ActionResult Save(string AccountBlockingTypeID, string Name, int? Status)
        {
            try
            {
                // Se invocara al método de la capa lógica y se retorna un json de la siguiente forma
                var parameters = new BlockingTypeSearchParameters();

                parameters.AccountBlockingTypeID = Convert.ToInt16(AccountBlockingTypeID); // 0 - Nuevo tipo de bloqueo
                parameters.LanguageID = CoreContext.CurrentLanguageID;
                parameters.Name = Name;
                parameters.Enabled = Convert.ToBoolean(Status);

                Int16 newAccountBLockingTypeID = BlockingType.Save(parameters);
                return Json(new { result = true, menssage = Translation.GetTerm("SavedSuccessfully"), newID = newAccountBLockingTypeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, menssage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        // List
        public ActionResult DatailBlockingType(int Id)
        {
            //Se invocara al método de la capa lógica y se retorna un json de la siguiente forma
            //BlockingType.(Id);
            return Json(new { result = true, menssage = "" });
        }

    }
}
