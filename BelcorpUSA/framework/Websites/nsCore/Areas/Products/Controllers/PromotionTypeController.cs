using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Globalization;
using Newtonsoft.Json;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Products.Controllers
{
    public class PromotionTypeController : Controller
    {
        public ActionResult Index()
        {
            var data = PromoPromotionTypeConfigurationsLogic.Instance.ListAll();
            var obj = (from r in data
                       where r.Active == true
                       select r).FirstOrDefault();
            if (obj != null)
                TempData["PromotionTypeID"] = obj.PromotionTypeID.ToString();
            else
                TempData["PromotionTypeID"] = "";

            var data2 = PromoPromotionTypeConfigurationsPerOrderLogic.Instance.GetBA();
            if (data2 == true)
                TempData["BA"] = "1";
            else
                TempData["BA"] = "0";

            return View();
        }

        [HttpPost]
        public string ListPromotionTypes()
        {
            try
            {
                var data = PromoPromotionTypesLogic.Instance.ListPromotionTypes();
                var newListData = new List<PromoPromotionTypes>();

                foreach (var item in data)
                {
                    var newData = new PromoPromotionTypes();
                    newData.PromotionTypeID = item.PromotionTypeID;
                    newData.TermName = Translation.GetTerm(item.TermName, "");
                    newListData.Add(newData);
                }

                var json = JsonConvert.SerializeObject(newListData);
                return json;
            }
            catch (Exception) { return "{'result' : false}"; }
        }

        [HttpGet]
        public virtual ActionResult ListPromotions(string query)
        {
            try
            {
                var data = PromoPromotionLogic.Instance.ListPromotions(query).Where(donde => donde.Description.ToLower().ToString().Contains(query.ToLower()));
                var newListData = new List<NetSteps.Data.Entities.Business.PromoPromotion>();

                foreach (var item in data)
                {
                    var newData = new NetSteps.Data.Entities.Business.PromoPromotion();
                    newData.PromotionID = item.PromotionID;
                    newData.Description = item.Description;
                    newData.StartDate = item.StartDate;
                    newData.EndDate = item.EndDate;
                    newData.Status = Translation.GetTerm(item.Status, "");

                    newListData.Add(newData);
                }
                return Json(newListData.Select(p => new { text = p.Description, PromotionTypeConfigurationPerPromotionID = 0, PromotionID = p.PromotionID, Description = p.Description, starDate = p.StartDate.ToShortDateString(), endDate = p.EndDate.ToShortDateString(), status = p.Status }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpGet]
        public virtual ActionResult ListPromotionsByPromotionTypeConfigurationPerPromotions()
        {
            try
            {
                var data = PromoPromotionLogic.Instance.ListPromotionsByPromotionTypeConfigurationPerPromotions();
                var newListData = new List<NetSteps.Data.Entities.Business.PromoPromotion>();

                foreach (var item in data)
                {
                    var newData = new NetSteps.Data.Entities.Business.PromoPromotion();
                    newData.PromotionTypeConfigurationPerPromotionID = item.PromotionTypeConfigurationPerPromotionID;
                    newData.PromotionID = item.PromotionID;
                    newData.Description = item.Description;
                    newData.StartDate = item.StartDate;
                    newData.EndDate = item.EndDate;
                    newData.Status = Translation.GetTerm(item.Status, "");

                    newListData.Add(newData);
                }
                return Json(newListData.Select(p => new { text = p.Description, PromotionTypeConfigurationPerPromotionID = p.PromotionTypeConfigurationPerPromotionID, PromotionID = p.PromotionID, Description = p.Description, starDate = p.StartDate.ToShortDateString(), endDate = p.EndDate.ToShortDateString(), status = p.Status }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult SavePromotionConfiguration(string PromoType, string NewPromotions, string PromotionsToDelete, string Ba, string newConfiguration)
        {
            try
            {
                PromoType = PromoPromotionTypeConfigurationsLogic.Instance.GetPromocionTypeDescuentoAcumulativo().ToString();
                var dataNew = GetListPromotion(NewPromotions);
                var dataToDelete = GetListPromotionPerPromotions(PromotionsToDelete);
                var ba = (Ba.ToUpper().Equals("TRUE") ? true : false);
                var configuration = (newConfiguration.ToUpper().Equals("TRUE") ? true : false);

                var result = PromoPromotionTypeConfigurationsLogic.Instance.SavePromotionConfiguration(PromoType, dataNew, dataToDelete, ba, configuration);

                if (result)
                    return Json(new { result = true });
                else
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(null, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private List<PromoPromotion> GetListPromotion(string promotion)
        {
            var datos = JsonConvert.DeserializeObject<List<string>>(promotion);
            var list = new List<PromoPromotion>();
            foreach (var item in datos)
            {
                list.Add(new PromoPromotion()
                {
                    PromotionID = int.Parse(item)
                });
            }
            return list;
        }

        private List<PromoPromotionTypeConfigurationPerPromotion> GetListPromotionPerPromotions(string promotion)
        {
            var datos = JsonConvert.DeserializeObject<List<string>>(promotion);
            var list = new List<PromoPromotionTypeConfigurationPerPromotion>();
            foreach (var item in datos)
            {
                list.Add(new PromoPromotionTypeConfigurationPerPromotion()
                {
                    PromotionTypeConfigurationPerPromotionID = int.Parse(item)
                });
            }
            return list;
        }
    }
}
