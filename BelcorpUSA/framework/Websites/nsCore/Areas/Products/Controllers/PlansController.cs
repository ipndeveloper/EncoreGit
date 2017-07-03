using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using System.Linq.Expressions;

namespace nsCore.Areas.Products.Controllers
{
    public class PlansController : BaseProductsController
    {
        //
        // GET: /Products/Plans/
        [FunctionFilter("Products", "~/Accounts")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string Name, bool? active)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var plans = Plan.Search(new FilterPaginatedListParameters<PlanSearchData>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,                   
                   
                });
               
                //|| (active != null) ? x.Enabled.Equals(active) : x.Enabled.Equals(true) || x.Enabled.Equals(false)
                 foreach (var plan in plans.Where(x => x.Name.Contains(Name.ToString()) ).ToList())
                {
                    builder.Append("<tr>");

                    //Check always to modify
                    builder.AppendCheckBoxCell(value: plan.PlanID.ToString(), name: "", disabled: (plan.PlanID == 1) ? true : false);

                    builder
                        //.AppendCell(plan.PlanID.ToString())
                        .AppendLinkCell("~/Products/Plans/Edit/" + plan.PlanID, plan.PlanCode)
                        .AppendCell(plan.Name)
                        .AppendCell(plan.Enabled ? Translation.GetTerm("True", "True") : Translation.GetTerm("False", "False"))
                        .AppendCell(plan.DefaultPlan ? Translation.GetTerm("True", "True") : Translation.GetTerm("False", "False"))
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = plans.TotalPages, page = plans.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no plans</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    foreach (var plan in Plan.Search().FindAll(x => items.Contains(x.PlanID)))
                    {
                        if (plan.Enabled != active)
                        {
                            plan.Enabled = active;
                            Plan.UpdateEnabledPlan(plan.PlanID, active);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            return Json(new { result = true });
        }
        
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                var plan = id.HasValue ? Plan.Search().Find(x => x.PlanID == id) : new PlanSearchData();
                
                return View(plan);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(int? planId, string planCode, string name, bool enabled)
        {
            try
            {
                List<PlanSearchData> plans = Plan.Search();

                var plan = planId.HasValue ? plans.Find(x => x.PlanID == planId) : new PlanSearchData();
                
                plan.PlanCode = planCode;
                plan.Name = name;
                
                // Plan.Enabled con DefaultPlan = true, es siempre true
                plan.Enabled = plan.DefaultPlan ? true : enabled;

                if (!planId.HasValue)
                {
                    // Se define como default el registro con PlanID = 1
                    plan.DefaultPlan = plans.Count() == 0 ? true : false;
                }

                plan.TermName = null;

                Plan.Save(plan);

                return Json(new { result = true, planId = plan.PlanID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
