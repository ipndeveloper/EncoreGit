namespace nsCore.Areas.Commissions.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Web.Extensions;
    using NetSteps.Web.Mvc.Helpers;
    using nsCore.Controllers;

    /// <summary>
    /// Bonus Requirement Controller
    /// </summary>
    public class BonusRequirementController : BaseController
    {
        /// <summary>
        /// Main page
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Bonus Requirement by filters
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="planId"></param>
        /// <param name="bonusTypeId"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDirection"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetByFilters(int page, int pageSize, int? planId, int? bonusTypeId, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                page++;
                StringBuilder builder = new StringBuilder();
                
                IEnumerable<BonusRequirement> bonusRequirements = orderByDirection ==NetSteps.Common.Constants.SortDirection.Ascending ?
                    bonusRequirements = BonusRequirementLogic.Instance.GetAllByFilters(planId ?? 1, bonusTypeId ?? 1, page, pageSize).OrderBy(orderBy):
                    bonusRequirements = BonusRequirementLogic.Instance.GetAllByFilters(planId ?? 1, bonusTypeId ?? 1, page, pageSize).OrderByDescending(orderBy);

                int rowCount = 0;
                if (bonusRequirements.ToList(m=>m.BonusRequirementId).Count > 0)
                {                    
                    foreach (var bonusRequirement in bonusRequirements)
                    {                        
                        builder.Append("<tr>")
                            .AppendCheckBoxCell(id:"BonusRequirementId", value: bonusRequirement.BonusRequirementId.ToString())
                            .AppendLinkCell("~/Commissions/BonusRequirement/Edit/" + bonusRequirement.BonusRequirementId, bonusRequirement.BonusRequirementId.ToString())
                            .AppendCell(bonusRequirement.BonusTypeId.ToString())
                            .AppendCell(bonusRequirement.BonusTypeName)
                            .AppendCell(bonusRequirement.PlanName)
                             .AppendCell(bonusRequirement.BonusAmount.Value.ToString("N",CoreContext.CurrentCultureInfo))

                              .AppendCell(bonusRequirement.BonusPercent.Value.ToString("N",CoreContext.CurrentCultureInfo))
                            .AppendCell(bonusRequirement.BonusMaxAmount.Value.ToString("N",CoreContext.CurrentCultureInfo))
                            .AppendCell(bonusRequirement.BonusMaxPercent.Value.ToString("N",CoreContext.CurrentCultureInfo))



                            //.AppendCell(bonusRequirement.BonusAmount.ToString())
                            //.AppendCell(bonusRequirement.BonusPercent.ToString())
                            //.AppendCell(bonusRequirement.BonusMaxAmount.ToString())
                            //.AppendCell(bonusRequirement.BonusMaxPercent.ToString())
                            .AppendCell(bonusRequirement.MinTitleId.ToString())
                            .AppendCell(bonusRequirement.MaxTitleId.ToString())
                            .AppendCell(bonusRequirement.BonusMinAmount.ToString())
                            .AppendCell(bonusRequirement.PayMonth.ToString())
                            .AppendCell(bonusRequirement.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .Append("</tr>");                       
                        rowCount = bonusRequirement.RowsCount;
                    }

                    int totalPages = (rowCount / pageSize);
                    if ((rowCount % pageSize) > 0)
                        totalPages++;
                    
                    return Json(new { result = true, totalPages = totalPages, page = builder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Create  new Bonus Requirement
        /// </summary>
        /// <returns>New view</returns>
        public virtual ActionResult New()
        {
            ViewData["IsNew"] = true;
            var model = new BonusRequirement();
            model.EffectiveDate = DateTime.Now;

            return View(model);
        }

        /// <summary>
        /// Get Bonus Requirement By Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        /// <returns>Edit View</returns>
        public virtual ActionResult Edit(int id) 
        {
            ViewData["IsNew"] = false;

            var model = BonusRequirementLogic.Instance.GetById(id);
            model.PlanName = SmallCollectionCache.Instance
                .BonusTypes
                .Where(m => m.BonusTypeId == model.BonusTypeId).FirstOrDefault().PlanName;
            model.BonusAmount = model.BonusAmount.Value.ToString("N", CoreContext.CurrentCultureInfo).ToDecimal();
            model.BonusMaxAmount = model.BonusMaxAmount.Value.ToString("N", CoreContext.CurrentCultureInfo).ToDecimal();
            model.BonusPercent = model.BonusPercent.Value.ToString("N", CoreContext.CurrentCultureInfo).ToDecimal();
            model.BonusMaxPercent = model.BonusMaxPercent.Value.ToString("N", CoreContext.CurrentCultureInfo).ToDecimal();
            return View("~/Areas/Commissions/Views/BonusRequirement/Edit.aspx", model);
        }

        /// <summary>
        /// Save Bonus Requirement
        /// </summary>
        /// <param name="model">Bonus Requirement Model</param>
        /// <param name="dateEffective">Effective Date</param>
        /// <param name="isnew">True if model is new, False if is for update</param>
        /// <returns>json confirm</returns>
        public virtual ActionResult Save(BonusRequirement model, string dateEffective, bool isnew)
        {
            try
            {
                DateTime effectiveDate;
                if (!DateTime.TryParse(dateEffective, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out effectiveDate))
                    throw new Exception(Translation.GetTerm("InvalidDate","Invalid Date"));

                model.EffectiveDate = effectiveDate;

                if (isnew)
                {
                    BonusRequirementLogic.Instance.Insert(model);
                }
                else
                {
                    BonusRequirementLogic.Instance.Update(model);
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Delete Bonus Requirement
        /// </summary>
        /// <param name="items">List of Ids</param>
        /// <returns>Json confirm</returns>
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                foreach (var id in items)
                {
                    BonusRequirementLogic.Instance.Delete(id);                    
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Get Bunus Type By Plan
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <returns>Json dictionary BonusTypeId - BunusTypeName</returns>
        public virtual ActionResult BonusTypeByPlan(int planId)
        {
             var data = SmallCollectionCache.Instance
                 .BonusTypes
                 .Where(m=>m.PlanId == planId)
                 .ToDictionary(bt => bt.BonusTypeId.ToString(), bt => bt.Name);

             return new JsonResult() { Data = data };
        }

        /// <summary>
        /// Gets Plan Name by Bonus Type
        /// </summary>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <returns>Json Result</returns>
        public virtual ActionResult PlanByBonusType(int? bonusTypeId)
        {
            var data = SmallCollectionCache.Instance
                 .BonusTypes
                 .Where(m => m.BonusTypeId == bonusTypeId).FirstOrDefault();

            if (data == null)
                return new JsonResult() { Data = string.Empty };

            return new JsonResult() { Data = data.PlanName };
        }
    }
}