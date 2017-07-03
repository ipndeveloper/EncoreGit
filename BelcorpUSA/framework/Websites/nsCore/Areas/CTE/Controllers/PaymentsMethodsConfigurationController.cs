using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Web.Mvc.Helpers;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos


namespace nsCore.Areas.CTE.Controllers
{
    public class PaymentsMethodsConfigurationController : BaseController
    {
        //
        // GET: /CTE/PaymentsMethodsConfiguration/

        public ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult BrowsePayments()
        {
            return View();
        }


        public virtual ActionResult PaymentsRules(int?  ID)
        {
            ViewData["getStatesTaxCache"] = PaymentMethodsRepository.getStates(); // Developed KLC - CSTI (ISSUE 106)
            PaymentConfigurations model = new PaymentConfigurations();

            if (ID != null)
            {
                ViewBag.IsTargetCredit = NetSteps.Data.Entities.Business.PaymentMethods.IsTargetCreditByPaymentConfiguration(ID.Value);
               
            }


            if (ID.HasValue)
            {                
                model = PaymentMethods.EditPaymentConfigurations(int.Parse(ID.ToString()));
                ViewBag.Edit = model;

                var param = new CollectionEntitiesSearchParameter() { PaymentTypeID = 11, CollectionEntityID = "", status = "" };
                var lisCollection = CollectionEntities.SearchDetails(param);
                var resultado = lisCollection.Where(x => x.CollectionEntityID == model.CollectionEntityID).Count();
                ViewBag.IsCreditDisp = resultado > 0 ? true : false;


                var accountTypes=PaymentMethods.EditPaymentConfigurationPerAccount(Convert.ToInt32(ID.ToString()));
                ViewBag.EditAccountTypes = accountTypes;
                ViewBag.EditAccountTypesCount = accountTypes.Count();
                var orderTypes = PaymentMethods.EditPaymentConfigurationPerOrderTypes(Convert.ToInt32(ID.ToString()));
                ViewBag.EditOrderTypes = orderTypes;
                ViewBag.EditOrderTypesCount = orderTypes.Count();
             
                return View();

            }
            else
            {
                ViewBag.Edit = model;
                return View();
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPayments(int page, int pageSize,
            int? FineAndInterestRulesID, int? orderStatus, string collectionEntityName, string daysForPayment, string toleranceValue,
            int? AccountTypeRestrictionId, int? OrderTypeRestrictionId, string state, string city, string county, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {

            if (string.IsNullOrEmpty(collectionEntityName)) collectionEntityName = "";
                        
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var payments = NetSteps.Data.Entities.Business.PaymentMethods.SearchDetails(new PaymentMethodsSearchParameters()
                {
                    RuleID = FineAndInterestRulesID,
                    OrderStatusID = orderStatus,
                    CollectionEntityID = collectionEntityName,
                    DaysPayment = daysForPayment,
                    tolerance = toleranceValue,
                    AccountTypeId = AccountTypeRestrictionId,
                    OrderType = OrderTypeRestrictionId,
                    state = state == null ? "" : state,
                    city = city == null ? "" : city,
                    county = county == null ? "" : county,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var payment in payments)
                {
                    builder.Append("<tr>");
                    builder
                        .AppendLinkCell("~/CTE/PaymentsMethodsConfiguration/PaymentsRules/" + payment.PaymentConfigurationID.ToString(), payment.PaymentConfigurationID.ToString())
                        //.AppendCell(payment.PaymentConfigurationID.ToString())
                        .AppendCell(payment.CollectionEntityName.ToString())
                        .AppendCell(payment.Description.ToString())
                        .AppendCell(payment.FineandInterestRules)
                        .AppendCell(payment.DaysForPayment.ToString())
                        .AppendCell(payment.DaysValidity.ToString())
                         .AppendCell(payment.TolerancePercentage == -1 ? "" : payment.TolerancePercentage.ToString("N",CoreContext.CurrentCultureInfo))
                        //.AppendCell(payment.TolerancePercentage== -1 ? "" :payment.TolerancePercentage.ToString())
                        .AppendCell(payment.ToleranceValue == -1 ? "" : payment.ToleranceValue.ToString())
                        .AppendCell(payment.NumberCuotas == -1 ? "" : payment.NumberCuotas.ToString()) 
                        .AppendCell(payment.UtilizaCredito)
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = payments.TotalPages, page = payments.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no payments</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult IsCreditCard(int CollectionEntityID)
        {

            var param = new CollectionEntitiesSearchParameter() { PaymentTypeID = 11,CollectionEntityID="" , status=""};
            var lisCollection = CollectionEntities.SearchDetails(param);
            var resultado = lisCollection.Where(x => x.CollectionEntityID == CollectionEntityID).Count();


            return Json(new
            {
                result = NetSteps.Data.Entities.Business.PaymentMethods.IsTargetCredit(CollectionEntityID)
                ,
                IsCreditDisp = resultado > 0 ? true : false
            },
                JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetCities(string state)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    Cities = (NetSteps.Data.Entities.TaxCache.SearchCityFromState(state)),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetCounty(string state,string city)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    County = (NetSteps.Data.Entities.TaxCache.SearchCountyFromCity(state, city)),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        //Zones
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetZones(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? ID)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                if (ID.HasValue)
                {
                    var listGeoScope = PaymentMethods.EditPaymentConfigurationGeoScope(Convert.ToInt32(ID.ToString()));

                    foreach (var supportMotive in listGeoScope)
                    {
                        builder.Append("<tr  class=\"Alt hover\">");
                        builder
                            .AppendCheckBoxCell()
                            .AppendCell(supportMotive.Name)
                            .AppendCell(supportMotive.Value)
                            .AppendCell(supportMotive.Except?"true":"false")
                            .Append("</tr>");                        
                    }

                }

                //return Json(new { result = true, totalPages = 0, page = true ? "<tr><td colspan=\"7\">There are no zones</td></tr>" : "" });                
                return Json(new { result = true, totalPages = 0, page = true ? builder.ToString() : "" });                
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult GetScopeLevels(int scopeLevelid)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    scopeLevels = (NetSteps.Data.Entities.Repositories.PaymentMethodsRepository.SearchPaymentsZones(scopeLevelid)).Select(pp => new { Name = pp.Name }),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult SavePaymentsRules(List<PaymentsZonesData> zones, List<int> RestrictAccList, List<int> RestrictOrdList,  //int orderType, int accountType, 
            string toleranceValue, string tolerancePercentage, string daysForPayment, int orderStatus, int rulesID, int paymentID, int paymentConfigurationID, string Description, int? NumberCuotas,
            int? DaysVal, string PaymentCredit)
        { 
            try
            {

                if (paymentConfigurationID != 0)
                {
                    PaymentMethodsRepository.DeletePaymentConfiguration(paymentConfigurationID);
                    PaymentMethodsRepository.UpdatePaymentsConfiguration(paymentID, RestrictOrdList, RestrictAccList, toleranceValue,
                                                                  tolerancePercentage, daysForPayment, orderStatus, rulesID, paymentConfigurationID,
                                                                  Description, NumberCuotas, DaysVal, PaymentCredit);
                }
                else
                {

                    paymentConfigurationID = PaymentMethodsRepository.InsertPaymentsConfiguration(paymentID, RestrictOrdList, RestrictAccList, toleranceValue,
                                                                         tolerancePercentage, daysForPayment, orderStatus, rulesID, Description, NumberCuotas, DaysVal, PaymentCredit);
                }
                if (zones != null)
                {
                    foreach (var pzone in zones)
                    {
                        if (pzone.Name != null)
                        {
                            var zone = new ZonesData
                            {
                                Name = Convert.ToString(pzone.Name),
                                Value = Convert.ToString(pzone.Value),
                                Except = Convert.ToBoolean(pzone.Except)
                            };
                            PaymentMethodsRepository.InsertPaymentsZones(pzone, paymentConfigurationID);
                        }
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

    }
}
