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

namespace nsCore.Areas.CTE.Controllers
{
    public class InterestRulesController : BaseController
    {
        //
        // GET: /CTE/InterestRules/

        public ActionResult GenerateRule(int? id)
        {        
            try
            {
                ViewData["Negotiations"] = CTERepository.ListNegotiation();
                ViewData["BaseAmounts"] = CTERepository.ListFineBaseAmounts();

                var rule = id.HasValue ? CTERepository.SearchRulesNegotiations().Find(x => x.FineAndInterestRulesID == id) : new CTERulesNegotiationData();
                return View(rule);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public ActionResult BrowseRules()
        {            
            return View();
        }

        #region Save Rules
        public virtual ActionResult SaveRule(List<CTERulesNegotiationData> rules, string Name,int FineAndInterestRulesID)
        {
            try
            {
                if (rules == null)
                    rules = new List<CTERulesNegotiationData>();
                //foreach (var rule in rules)
                //{
                //    //KLC -CSTI                     
                //    var vrules = new CTERulesNegotiationData
                //    {
                //        Name = Name,
                //        NegotiationLevelID =rule.NegotiationLevelID,
                //        StartDay = rule.StartDay,
                //        EndDay=rule.EndDay,
                //        FinePercentage=rule.FinePercentage,
                //        FineBaseAmountID=rule.FineBaseAmountID,
                //        MinimumAmountForFine=rule.MinimumAmountForFine,
                //        InterestPercentage=rule.InterestPercentage,
                //        InterestBaseAmountID = rule.InterestBaseAmountID
                //    };


                //    CTERepository.SaveRules(vrules);
                //}

                // GYS - EFP
               // ViewData["Negotiations"] = CTERepository.ListNegotiation();
                //ViewData["BaseAmounts"] = CTERepository.ListFineBaseAmounts();
                var negotiations = CTERepository.ListNegotiation();// (ViewData["Negotiations"] as List<CTENegotiationSearchData>);
                var baseAmounts = CTERepository.ListFineBaseAmounts();// (ViewData["BaseAmounts"] as List<CTEFineBaseAmountsData>);

                //foreach()
                rules.ForEach(x => x.NegotiationLevelID = (negotiations.Find(z => z.Name == x.NegotiationLevel).NegotiationLevelID));
                rules.ForEach(x => x.InterestBaseAmountID = (baseAmounts.Find(z => z.Name == x.InterestBaseAmount).FineBaseAmountID));
                rules.ForEach(x => x.FineBaseAmountID = (baseAmounts.Find(z => z.Name == x.FineBaseAmount).FineBaseAmountID));
                rules.ForEach(x => x.FineBaseAmountIDReg = (baseAmounts.Find(z => z.Name == x.FineBaseAmountIDReg).FineBaseAmountID.ToString()));
                bool success = false;

                success = NetSteps.Data.Entities.Business.CTE.Instance.SaveStructuredRule(Name, rules, FineAndInterestRulesID);

                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRulesNegotiation(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            string id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var Negotiations = NetSteps.Data.Entities.Business.CTE.BrowseRulesNegotiation(new CTERulesParameters()
                {
                    FineAndInterestRulesID = Convert.ToInt32(id),
                    PageIndex = page,
                    PageSize = 170,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                bool exist = CTERepository.ValidateFineAndInterestsRule(int.Parse(id));
                foreach (var negotiation in Negotiations)
                {
                    builder.Append("<tr>");
                    builder
                        //.AppendCheckBoxCell(value: exist?"1": "0", disabled: exist)
                        .AppendCheckBoxCell(value: negotiation.FineAndInterestRulesPerNegotiationLevelID.ToString(), disabled: exist)
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
                
                return Json(new { result = true, totalPages = Negotiations.TotalPages, page = Negotiations.TotalCount == 0 ? "<tr><td colspan=\"8\">There are no rules</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRules(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? FineAndInterestRulesID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var rules = NetSteps.Data.Entities.Business.CTE.BrowseRules(new CTERulesParameters()
                {
                    FineAndInterestRulesID = FineAndInterestRulesID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });                
        
                foreach (var rule in rules)
                {
                    builder.Append("<tr>");

                    builder
                        //.AppendCheckBoxCell(value: rule.FineAndInterestRulesID.ToString())
                        .AppendLinkCell("~/CTE/InterestRules/GenerateRule/" + rule.FineAndInterestRulesID, rule.FineAndInterestRulesID.ToString())
                        .AppendCell(rule.Name)
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = rules.TotalPages, page = rules.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

    }
}
