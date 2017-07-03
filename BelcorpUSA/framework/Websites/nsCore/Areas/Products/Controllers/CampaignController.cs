using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Products.Controllers
{
    public class CampaignController : Controller
    {
        //
        // GET: /Products/Campaign/
        /// <summary>
        /// Controlador, encargado de administrar las creación de campañas
        /// Create by FHP - 16/07/2015 
        /// </summary>
        /// <returns>Vista con los modelos establecidos</returns>
        public ActionResult Index()
        {

            //TempData["GetCampaign"] = from x in Periods.GetPeriods()
            //                          orderby x.Value
            //                          select new SelectListItem()
            //                          {
            //                              Text = x.Key,
            //                              Value = x.Value
            //                          };


            TempData["GetCampaign"] = from x in  Periods.GetNextPeriodsByAccountType(Constants.AccountType.Testing.GetHashCode(), 2, 0, false)
                                      orderby x.Value
                                      select new SelectListItem()
                                      {
                                          Text = x.Key.ToString(),
                                          Value = x.Key.ToString()
                                      };
            return View();
        }

        public ActionResult Information()
        {
            TempData["Periods"] = PeriodExtensions.GetPreviousNextPeriods();
            return View();
        }

        public ActionResult GetCatalogByCampaign(int campaignID)
        {
            var getCatalog = Catalog.GetCatalogByCampaignID(campaignID);
            return Json(getCatalog, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateCampaign(int catalogID)
        {
            List<TemporalMatrixSearchData> objEntity = Catalog.GetTemporalMatrix();
            foreach (var item in objEntity)
            {
                Campaign.ProductWithCatalog(item.ProductID, catalogID, item.Type, item.ProductName);
                Campaign.ProductWithMaterial(item.ProductID, catalogID);
                Campaign.ProductPrice(item.ProductID, catalogID);
            }
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }







    }
}
