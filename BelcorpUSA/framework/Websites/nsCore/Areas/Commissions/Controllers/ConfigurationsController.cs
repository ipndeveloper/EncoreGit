using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;



namespace nsCore.Areas.Commissions.Controllers
{
	public class ConfigurationsController : BaseCommissionController
    {
        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult Titles()
        {
            return View("TitleSearch");
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                //FilterDateRangePaginatedListParameters<WareHouseMaterialSearchData>
                var titles = Title.ListTitles(new FilterDateRangePaginatedListParameters<TitleSearchData>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                foreach (var title in titles)
                {
                    builder.Append("<tr>");
                    builder
                       .AppendCell(title.TitleID.ToString())
                       .AppendLinkCell("~/Commissions/Configurations/EditTitle/" + title.TitleID, title.TitleCode)
                       .AppendCell(title.ClientName.ToString())
                       .AppendCell(title.ClientCode.ToString())
                       .AppendCell(Translation.GetTerm(title.TermName, title.Name))
                       .AppendCell(title.SortOrder.ToString())
                       ;
                    //builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("AddManualMovement", "Add Manual Movement"), linkCssClass: "btnViewStats", linkID: wareHouseMaterials.WarehouseMaterialID.ToString());
                    builder.Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = titles.TotalPages, page = titles.TotalCount == 0 ? "<tr><td colspan=\"6\">There are no periods</td></tr>" : builder.ToString() });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult EditTitle(int? id)
        {
            try
            {

                var oTitle = id.HasValue ? Title.GetTitleByID((int)id) : new TitleSearchData();
                TempData["CalculationTypes"] = from req in Title.ListCalculationTypes(true)
                                                select new SelectListItem()
                                                {
                                                    Text = req.Value,
                                                    Value = req.Key,
                                                    Selected = false
                                                };

                TempData["Plans"] = from plan in RequirementRule.ListPlans()
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = false
                                    };

                TempData["Titles"] = from plan in Title.ListTitles()
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = false
                                    };

                TempData["TitlePhases"] = from plan in Title.ListTitlePhases()
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = oTitle.TitlePhaseID == Convert.ToInt32(plan.Key)
                                    };
                 
              

                return View(oTitle );
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult SaveTitle(TitleSearchData  pTitle)
        {
            try
            {
                int result=Title.SaveTitle(pTitle);

                return Json(new { result = true, Id = result });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }




        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult Rules()
        {
            TempData["RuleTypes"] = RequirementRule.ListRuleTypes();
            TempData["Plans"] = RequirementRule.ListPlans();

            return View("RulesByPlan");
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetRules(int? RuleTypeID, int? PlanID, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                //FilterDateRangePaginatedListParameters<WareHouseMaterialSearchData>
                var datos = RequirementRule.ListRequirementRules(new RequirementRuleSearchParameters()
                {
                    RuleRequirementID=0,
                    RuleTypeID=RuleTypeID??0,
                    PlanID=PlanID??0,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var row in datos)
                {
                    builder.Append("<tr>");
                    builder
                       .AppendLinkCell("~/Commissions/Configurations/EditRule/" + row.RuleRequirementID, row.RuleName.ToString() )
                       .AppendCell(row.PlanName ?? "")
                       .AppendLinkCell("javascript:void(0);",row.Description ?? "" , linkCssClass: "clsDescription")
                       .AppendCell(row.Value1 == null ? "" : row.Value1.ToString())
                       .AppendCell(row.ValueType1 ?? "")
                       .AppendCell(row.Value2 ==null? "" : row.Value2.ToString())
                       .AppendCell(row.ValueType2 ?? "")
                       .AppendCell(string.IsNullOrEmpty(row.Value3) ? "" :  Convert.ToDecimal(row.Value3).ToString("N", CoreContext.CurrentCultureInfo))
                       .AppendCell(row.ValueType3 ?? "")
                       .AppendCell(string.IsNullOrEmpty(row.Value4) ? "" : Convert.ToDecimal(row.Value4).ToString("N", CoreContext.CurrentCultureInfo))
                       .AppendCell(row.ValueType4 ?? "")
                       ;
                    
                    //builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("AddManualMovement", "Add Manual Movement"), linkCssClass: "btnViewStats", linkID: wareHouseMaterials.WarehouseMaterialID.ToString());
                    builder.Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = datos.TotalPages, page = datos.TotalCount == 0 ? "<tr><td colspan=\"12\">There are no periods</td></tr>" : builder.ToString() });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetRuleTypeByID(int id)
        {
            try
            {
                var datos = RequirementRule.GetRuleTypeByID(id);

                return Json(new { result = true, data = datos });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult EditRule(int? id)
        {
            try
            {
                TempData["RuleTypes"] = from rule in RequirementRule.ListRuleTypes()
                                    select new SelectListItem()
                                    {
                                        Text = rule.Value,
                                        Value = rule.Key,
                                        Selected = false
                                    };
                TempData["Plans"] = from plan in RequirementRule.ListPlans()
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = false
                                    };

                var result = id.HasValue ? RequirementRule.GetRuleByID((int)id) : new RequirementRuleSearchData();
                if (result!=null)
                {
                    result.Value1 =  string.IsNullOrEmpty(result.Value4) ? "" : Convert.ToDecimal(result.Value1).ToString("N", CoreContext.CurrentCultureInfo);
                    result.Value2 = string.IsNullOrEmpty(result.Value4)  ? "" : Convert.ToDecimal(result.Value2).ToString("N", CoreContext.CurrentCultureInfo);
                    result.Value3 = string.IsNullOrEmpty(result.Value4)  ? "" : Convert.ToDecimal(result.Value3).ToString("N", CoreContext.CurrentCultureInfo);
                    result.Value4 = string.IsNullOrEmpty(result.Value4)  ? "" : Convert.ToDecimal(result.Value4).ToString("N", CoreContext.CurrentCultureInfo);
                }
                

                return View(result);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Commissions", "~/Configurations")]
        public virtual ActionResult SaveRule(RequirementRuleSearchData pDato)
        {
            try
            {
                int result = RequirementRule.Save(pDato);

                return Json(new { result = true, Id = result });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



    }
}
