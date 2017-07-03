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
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.CTE.Controllers
{
    public class RenegotiationMethodsController : BaseController
    {
        

        public ActionResult BrowseRenegotiation()
        {
            return View();
        }

        #region Save Rules
        public virtual ActionResult SaveRenegotiation(RenegotiationSearchData Renegotiation)//(List<RenegotiationDetSearchData> Details, RenegotiationSearchData Renegotiation)
        {
            try
            {

                //if (Details == null)
                //    Details = new List<RenegotiationDetSearchData>();
                
                var list = AccountPropertiesBusinessLogic.GetFineBaseAmounts();

                //Details.ForEach(x => x.FineBaseAmountID = (list.Where(z => z.Value == x.FineBaseAmountDesc).First().Key));
                bool success = false;

                success = RenegotiationMethods.SaveStructuredReng(Renegotiation);//, Details);

                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        //
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRenegotiationMethods(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string desc)        
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var ListRenegotiationMethods = RenegotiationMethods.BrowseRenegotiation(new RenegotiationSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    DescriptionRenegotiation = desc

                });

                string S = Translation.TermTranslation.GetTerm("CalendarDays", "Calendar Days");
                string N = Translation.TermTranslation.GetTerm("ValidSatSundHol", "Valid Saturdays, Sundays and holidays");

           
                foreach (var reneg in ListRenegotiationMethods)
                {

                    string checModifiesValues = reneg.ModifiesValues == "S" ? "checked=\"checked\"" : "";
                    string checModifiesDates = reneg.ModifiesDates == "S" ? "checked=\"checked\"" : "";

                    builder.Append("<tr>");
                    builder
                        .AppendLinkCell("~/CTE/RenegotiationMethods/Renegotiation/" + reneg.RenegotiationConfigurationID, reneg.RenegotiationConfigurationID.ToString())
                        .AppendCell(reneg.DescriptionRenegotiation)
                        .AppendCell(reneg.SharesNumber.ToString())
                        .AppendCell(reneg.Site)
                        //.AppendCell(reneg.FineAndInterestRules.ToString())
                        //.AppendLinkCell("~/CTE/RenegotiationMethods/Renegotiation/" + reneg.RenegotiationConfigurationID, reneg.FineAndInterestRules.ToString())
                        .AppendCell("<a href='javascript:void(0)' onclick='ViewDetails(" + reneg.FineAndInterestRuleID +")'> " + reneg.FineAndInterestRules + "</a>")


                        //.AppendCell(reneg.DayExpiration.ToString())
                        .AppendCell(reneg.DayValidate.ToString())
                        
                        //.AppendCell(reneg.RegFinePercentage.ToString())
                        //.AppendCell(reneg.RegInteresPercentage.ToString())
                        .AppendCell(reneg.FirstSharesday.ToString())
                        //.AppendCell(reneg.SkillfulCalendarFirst)
                        .AppendCell(reneg.SkillfulCalendarFirst == "S" ? S : N)
                        .AppendCell(reneg.SharesInterval.ToString())
                        .AppendCell(reneg.SkillfulRemainingCalendar == "S" ? S : N)
                        //.AppendCell(reneg.ModifiesDates)
                        //.AppendCheckBoxCell(reneg.ModifiesDates == "S" ? "checked='checked'" : "","disabled='true'", cssClass: reneg.ModifiesDates, name: reneg.ModifiesDates, disabled: false)
                        .Append("<td><input type=\"checkbox\"   class=\" " + reneg.ModifiesDates + " \"   " + checModifiesDates + "   disabled=\"true\" /> </td>")

                        //.AppendCell(reneg.ModifiesValues)
                        //.AppendCheckBoxCell(reneg.ModifiesValues == "S" ? "checked='checked'" : "", "disabled='true'", cssClass: reneg.ModifiesValues, name: reneg.ModifiesValues, disabled: false)
                        .Append("<td><input type=\"checkbox\"   class=\" " + reneg.ModifiesValues + " \"   " + checModifiesValues + "   disabled=\"true\" /> </td>")
                  
                       .Append("</tr>");

                    ++count;
                }

                return Json(new { result = true, totalPages = ListRenegotiationMethods.TotalPages, page = ListRenegotiationMethods.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Renegotiation(int? id = 0)
        {
            RenegotiationSearchData RenegotiationData = new RenegotiationSearchData();
            try
            {

                RenegotiationData = RenegotiationMethods.DataRenegotiation(Convert.ToInt32(id)) ?? new RenegotiationSearchData();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
            ViewBag.Edit = RenegotiationData;
            return View();
        }

       
        //[OutputCache(CacheProfile = "PagedGridData")]
        //public virtual ActionResult GetRenegotiationDetails(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int ID=0)
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        int count = 0;

        //        var ListRenegotiationDet = RenegotiationMethods.BrowseRenegotiationDet(new RenegotiationDetSearchParameters()
        //        {
        //            PageIndex = page,
        //            PageSize = pageSize,
        //            OrderBy = orderBy,
        //            OrderByDirection = orderByDirection,
        //            RenegotiationConfigurationID = ID
        //        });

        //        foreach (var reneg in ListRenegotiationDet)
        //        {
        //            builder.Append("<tr>");

        //            builder
        //                .AppendCheckBoxCell(value: reneg.RenegotiationConfigurationDetailsID.ToString(), name: reneg.RenegotiationConfigurationDetailsID.ToString())                                                
        //                .AppendCell(reneg.OpeningDay.ToString())
        //                .AppendCell(reneg.FinalDay.ToString())
        //                .AppendCell(reneg.FineBaseAmountDesc)
        //                .AppendCell(reneg.Discount.ToString())
        //                .Append("</tr>");
        //            ++count;
        //        }

        //        return Json(new { result = true, totalPages = ListRenegotiationDet.TotalPages, page = ListRenegotiationDet.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no details</td></tr>" : builder.ToString() });
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}

      
        public virtual ActionResult RulesDetailsModal(int FineAndInterestRulesID)
        {
            
            RenegotiationSearchData data = new RenegotiationSearchData();
            data.FineAndInterestRules = FineAndInterestRulesID.ToString();
            
            return View(data);
        }

       
        public virtual ActionResult GetRulesDetailsModal(
             //int page, int pageSize, 
            string FineAndInterestRulesID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var Negotiations = CTERepository.BrowseRulesNegotiation().FindAll(x => x.FineAndInterestRulesID == int.Parse(FineAndInterestRulesID)).OrderBy(z=>z.OpeningDay).OrderBy(y=>y.Negotiation);

                
                foreach (var negotiation in Negotiations)
                {
                    builder.Append("<tr>");
                    builder
                        //.AppendCheckBoxCell(value: exist ? "1" : "0", disabled: exist)
                        .AppendCell(negotiation.Negotiation)
                        .AppendCell(negotiation.OpeningDay.ToString())
                        .AppendCell(negotiation.FinalDay.ToString())
                        .AppendCell(negotiation.FinePercentage.ToString())
                        .AppendCell(negotiation.AppliedValue)
                        .AppendCell(negotiation.MinimumDebt.ToString())
                        .AppendCell(negotiation.InterestPercentage.ToString())
                        .AppendCell(negotiation.Interest.ToString())
                        .AppendCell(negotiation.Discount)
                        .AppendCell(negotiation.FineBaseAmountIDReg)
                        .Append("</tr>");
                    ++count;
                }
                //return View(builder.ToString());
                //return Json(new { result = true, totalPages = Negotiations.TotalPages, page = Negotiations.TotalCount == 0 ? "<tr><td colspan=\"8\">There are no rules</td></tr>" : builder.ToString() });
                return Json(new { result = true, Items = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
