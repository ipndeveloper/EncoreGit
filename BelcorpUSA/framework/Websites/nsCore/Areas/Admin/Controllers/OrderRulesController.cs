using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Extensions;
using nsCore.Controllers;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using Newtonsoft.Json.Linq;
using nsCore.Models;
using NetSteps.Common.Globalization;
using OrderRules.Core.Model;
using OrderRules.Service.DTO.Converters;
using OrderRules.Service.DTO;

using OrderRules.Data.UnitOfWork.Interface;
using OrderRules.Data.Repository.Interface;
using OrderRules.Service.Interface;
using System.Text;

namespace nsCore.Areas.Admin.Controllers
{
    public class OrderRulesController : Controller
    {
        //
        // GET: /Admin/OrderRules/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderPreCondition()
        {

            var orderStatuses = new OrderStatusesBusinessLogic();
            //var ticketPaymentStatuses = new TicketPaymentStatusesBusinessLogic();
            var expirationPaymentStatuses = new ExpirationStatusLogic();
            var negotiationLevelStatuses = new NegotiationLevelBusinessLogic();


            int[] statusOrderIdList =
            {
                (int) ConstantsGenerated.OrderStatus.Pending,
                (int) ConstantsGenerated.OrderStatus.Paid,
                (int) ConstantsGenerated.OrderStatus.Cancelled,
                (int) ConstantsGenerated.OrderStatus.CancelledPaid,
                (int) ConstantsGenerated.OrderStatus.CreditCardDeclined
            };

            string arrayOfStatusOrderId = statusOrderIdList.Aggregate<int, string>(String.Empty, (x, y) => (x.Length > 0 ? x + "," : x) + y.ToString(""));

            ViewBag.orderStatusListPerDay = orderStatuses.GetOrderStatusesByOrderStatusIdArray(arrayOfStatusOrderId);
            ViewBag.orderStatusWithoutPayment = orderStatuses.GetOrderStatusesByOrderStatusIdArray(((int)ConstantsGenerated.OrderStatus.PendingPerPaidConfirmation).ToString(""));
            //ViewBag.ticketPaymentStatuses = ticketPaymentStatuses.GetTicketPaymentStatusesByArrayId("ALL");
            ViewBag.expirationStatuses = expirationPaymentStatuses.GetAllExpirationStatus();
            ViewBag.negotiationLevels = negotiationLevelStatuses.GetAllNegotiationLevel();
            
            var orderRules = new OrdersRulesBusinessLogic();
            ViewBag.HasOrders = orderRules.HasOrderRules();

            

            return View();
        }

        [HttpPost]
        public ActionResult Save(JsonDynamicWrapper json)
        {
            dynamic model = System.Web.Helpers.Json.Decode(json.data);

            try
            {
                var ordersRulesConfig = new OrderRulesConfigurationBusinessLogic();
                ordersRulesConfig.OrderRulesConfiguration(model);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            
            
        }

        public ActionResult GetOrderRulesConfiguration()
        {
            var ordersRulesConfig = new OrderRulesConfigurationBusinessLogic();

            return Json(ordersRulesConfig.GetOrderRulesConfiguration(), JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult Edit(int? id)
        {
            RulesDTO model = null;
            if (id.HasValue)
            {
                var rule = Create.New<IOrderRulesService>().GetRuleById(id.Value);
                if (rule != null)
                {
                    var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
                    model = ordeRuleConverter.Convert(rule);
                    TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == model.TermName && tt.LanguageID == CoreContext.CurrentLanguageID);
                    if (translation == default(TermTranslation))
                    {
                        translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == model.TermName);
                        if (translation == default(TermTranslation))
                            model.TermContent = Translation.GetTerm("TextDefaultRule", "Text");
                        else
                            model.TermContent = translation.Term;
                    }
                    else
                    {
                        model.TermContent = translation.Term;
                    }
                }
            }
            else
            {
                model = NewModelRuleDTOEmpty();

            }
            return View(model);
        }

        public ActionResult GetPartialForRule(int? RuleID = null)
        {
            try
            {
                RulesDTO model = null;
                if (RuleID.HasValue && RuleID != 0)
                {
                    var rule = Create.New<IOrderRulesService>().GetRuleById(RuleID.Value);
                    var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
                    model = ordeRuleConverter.Convert(rule);
                }
                else
                {
                    model = NewModelRuleDTOEmpty();
                }
                return PartialView("_OrderRulesOptions", model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual IEnumerable<int> DiscountedPriceTypesToDisplay
        {
            get
            {
                var service = Create.New<IPriceTypeService>();
                return service.GetCurrencyPriceTypes().Select(pt => pt.PriceTypeID);
            }
        }

        protected virtual int GetDefaultCurrencyIDFromMarket(int marketID)
        {
            return Market.Load(marketID).GetDefaultCurrencyID();
        }

        private RulesDTO NewModelRuleDTOEmpty()
        {
            RulesDTO model = new RulesDTO();
            model.RuleValidationsDTO.Add(new RuleValidationsDTO());
            model.RuleValidationsDTO.FirstOrDefault().AccountIDs = new List<int>();
            model.RuleValidationsDTO.FirstOrDefault().AccountTypeIDs = new List<short>();
            model.RuleValidationsDTO.FirstOrDefault().ProductIDs = new List<int>();
            model.RuleValidationsDTO.FirstOrDefault().ProductTypeIDs = new List<int>();
            model.RuleValidationsDTO.FirstOrDefault().OrderTypeIDs = new List<short>();
            model.RuleValidationsDTO.FirstOrDefault().StoreFrontIDs = new List<int>();
            model.TermContent = Translation.GetTerm("TextDefaultRule", "Text");
            return model;
        }

        [ValidateInput(false)]
        public ActionResult SaveOrderRule(RulesDTO model, int LanguageID)
        {
            try
            {
                string parsedTerm = "";
                var ruleService = Create.New<IOrderRulesService>();
                var rule = Create.New<Rules>();
                model.RuleStatus = (int)RuleStatus.Active;
                var ruleStatuses = Create.New<IRuleStatusesRepository>().GetByID((int)RuleStatus.Active);
                model.RuleStatuses = ruleStatuses;
                var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
                rule = ordeRuleConverter.Convert(model);
                parsedTerm = model.TermContent == null ? " " : model.TermContent.Replace("\n", "|n").TrimEnd("\n");
                rule.TermName = SaveTermTranslation("OrderRuleTerm_" + rule.RuleID.ToString(), LanguageID, parsedTerm);
                if (rule.RuleID == 0)
                {
                    ruleService.CreateRule(rule);
                }
                else
                {
                    ruleService.UpdateRule(rule);
                }
                ruleService.Commit();
                
                
                //ruleService.UpdateRule(rule);
                //ruleService.Commit();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private string SaveTermTranslation(string pTermName, int pLanguageID, string pTermContent)
        {
            string result = null;
            try
            {
                TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == pTermName && tt.LanguageID == pLanguageID);

                if (translation == default(TermTranslation))
                {
                    translation = new TermTranslation()
                    {
                        Active = true,
                        LanguageID = pLanguageID,
                        LastUpdatedUTC = DateTime.UtcNow,
                        Term = pTermContent,
                        TermName = pTermName
                    };
                }
                else
                {
                    //Found another... just modify it.
                    translation.StartEntityTracking();
                    translation.Term = pTermContent;
                    translation.Active = true;
                    translation.LastUpdatedUTC = DateTime.UtcNow;
                }
                translation.Save();
                result = pTermName;
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPaginatedOrderRule(bool? status, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {

                 RulesDTO model = null;
          
                    var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
                   



                var rules = this.GetFilteredOrderRuleList(orderBy, orderByDirection, status);
                if (!rules.Any())
                {
                    return Json(new { result = true, totalPages = 0, page = String.Format("<tr><td colspan=\"6\">{0}</td></tr>", Translation.GetTerm("ThereAreNoRules", "There are no rules")) }, JsonRequestBehavior.AllowGet);
                }

                var builder = new StringBuilder();

                foreach (var rule in rules.Skip(page * pageSize).Take(pageSize))
                {
                    string editUrl = string.Format("~/Admin/OrderRules/Edit/{0}", rule.RuleID);
                    model = ordeRuleConverter.Convert(rule);
                    builder.Append("<tr>")
                        .AppendCheckBoxCell(value: rule.RuleID.ToString())
                        .AppendCell(rule.RuleID.ToString())
                        .AppendLinkCell(editUrl, rule.Name)
                       // .AppendCell(model.RuleValidationsDTO.FirstOrDefault().CustomerPriceSubTotalDTO.FirstOrDefault().MinimumAmount.ToString())
                        .AppendCell(rule.StartDate.HasValue ? rule.StartDate.Value.ToString("g", CoreContext.CurrentCultureInfo) : string.Empty)
                        .AppendCell(rule.EndDate.HasValue ? rule.EndDate.Value.ToString("g", CoreContext.CurrentCultureInfo) : string.Empty)
                        .AppendCell(rule.RuleStatus == (int)RuleStatus.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                        .Append("</tr>");
                }
              //  return Json(new { result = true, totalPages = managamentsledger.TotalPages, page = managamentsledger.TotalCount == 0 ? String.Format("<tr><td colspan=\"6\">{0}</td></tr>", Translation.GetTerm("ThereAreNoManagmentLedger", "There are not managment ledger")) : builder.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new { result = true, totalPages = Math.Ceiling(rules.Count() / pageSize.ToDouble()), page = builder.ToString() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual IEnumerable<Rules> GetFilteredOrderRuleList(string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? status)
        {
            IEnumerable<Rules> rules = null;
            if (status == null)
                rules = Create.New<IOrderRulesService>().GetRules();
            else if (Convert.ToBoolean(status))
                rules = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Active);
            else
                rules = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Inactive);

            switch (orderBy)
            {
                case "Description":
                    rules = rules.OrderBy(p => p.Name);
                    break;
                case "StartDate":
                    rules = rules.OrderBy(p => p.StartDate);
                    break;
                case "EndDate":
                    rules = rules.OrderBy(p => p.EndDate);
                    break;
                case "OrderRuleStatusTypeID":
                    rules = rules.OrderBy(p => p.RuleStatus);
                    break;
            }
            if (orderByDirection == NetSteps.Common.Constants.SortDirection.Descending)
            {
                rules = rules.Reverse();
            }

            return rules;
        }

        public virtual ActionResult DeleteRules(List<int> items)
        {
            try
            {
                if (items != null)
                {
                    foreach (var ruleID in items)
                    {
                        var service = Create.New<IOrderRulesService>();
                        var rule = service.GetRuleById(ruleID);
                        var translations = TermTranslation.Repository.Where(tt => tt.TermName == rule.TermName);
                        foreach (var item in translations)
                        {
                            item.Delete();
                        }
                        service.DeleteRule(rule.RuleID);
                        service.Commit();
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

        public virtual ActionResult ChangeRules(List<int> items, bool active)
        {
            try
            {
                if (items != null)
                {
                    foreach (var ruleID in items)
                    {
                        var service = Create.New<IOrderRulesService>();
                        var rule = service.GetRuleById(ruleID);
                        var ruleStatuses = active ? Create.New<IRuleStatusesRepository>().GetByID((int)RuleStatus.Active) :
                                                Create.New<IRuleStatusesRepository>().GetByID((int)RuleStatus.Inactive);
                        rule.RuleStatuses = ruleStatuses;
                        rule.RuleStatus = ruleStatuses.RuleStatusID;
                        service.UpdateRule(rule);
                        service.Commit();
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
