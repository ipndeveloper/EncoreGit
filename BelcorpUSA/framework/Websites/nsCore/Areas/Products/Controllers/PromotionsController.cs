using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Products.Controllers
{
	public class PromotionsController : BaseProductsController
	{

		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult Index()
		{
			return View();
		}

		protected virtual string GetControllerName(string promotionKind)
		{
			string controllerName = null;
			if (promotionKind == PromotionKindNames.ProductFlatDiscount || promotionKind == PromotionKindNames.ProductPercentDiscount)
			{
				controllerName = "ProductPromotions";
			}
			else if (promotionKind == PromotionKindNames.OrderDefaultCartRewards)
			{
				controllerName = "CartRewardsPromotions";
			}
			else
			{
				throw new InvalidOperationException(Translation.GetTerm("InvalidPromotionType", "Invalid promotion type retrieved"));
			}
			return controllerName;
		}
        protected virtual IEnumerable<IPromotion> GetFilteredPromotionList(string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? status)
		{
            IEnumerable<IPromotion> promotions = null;
            if(status == null)
			    promotions = Create.New<IPromotionService>().GetPromotions(PromotionStatus.Disabled | PromotionStatus.Enabled,
				                                                           x => x.PromotionKind == PromotionKindNames.ProductFlatDiscount
					                                                       || x.PromotionKind == PromotionKindNames.ProductPercentDiscount
					                                                       || x.PromotionKind == PromotionKindNames.OrderDefaultCartRewards);
            else if(Convert.ToBoolean(status))
                promotions = Create.New<IPromotionService>().GetPromotions(PromotionStatus.Enabled,
                                                                           x => x.PromotionKind == PromotionKindNames.ProductFlatDiscount
                                                                           || x.PromotionKind == PromotionKindNames.ProductPercentDiscount
                                                                           || x.PromotionKind == PromotionKindNames.OrderDefaultCartRewards);
            else
                promotions = Create.New<IPromotionService>().GetPromotions(PromotionStatus.Disabled,
                                                                           x => x.PromotionKind == PromotionKindNames.ProductFlatDiscount
                                                                           || x.PromotionKind == PromotionKindNames.ProductPercentDiscount
                                                                           || x.PromotionKind == PromotionKindNames.OrderDefaultCartRewards);
            switch (orderBy)
            {
                case "Description": 
                    promotions = promotions.OrderBy(p => p.Description);
                    break;
                case "StartDate":
                    promotions = promotions.OrderBy(p => p.StartDate);
                    break;
                case "EndDate":
                    promotions = promotions.OrderBy(p => p.EndDate);
                    break;
                case "PromotionKind":
                    promotions = promotions.OrderBy(p => p.PromotionKind);
                    break;
                case "PromotionStatusTypeID":
                    promotions = promotions.OrderBy(p => p.PromotionStatusTypeID);
                    break;
            }
            if (orderByDirection == NetSteps.Common.Constants.SortDirection.Descending)
            {
                promotions = promotions.Reverse();
            }

            return promotions;
		}
		[OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(bool? status, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
				var promotions = this.GetFilteredPromotionList(orderBy, orderByDirection,status);
				if (!promotions.Any())
				{
					return Json(new { result = true, totalPages = 0, page = String.Format("<tr><td colspan=\"6\">{0}</td></tr>", Translation.GetTerm("ThereAreNoPromotions", "There are no promotions")) });
				}

				var builder = new StringBuilder();

				foreach (var promotion in promotions.Skip(page * pageSize).Take(pageSize))
				{
					string controllerName = GetControllerName(promotion.PromotionKind);

					var promoStatus = (PromotionStatus)promotion.PromotionStatusTypeID;
					string editUrl = string.Format("~/Products/{0}/Edit/{1}", controllerName, promotion.PromotionID);
					builder.Append("<tr>")
						.AppendCheckBoxCell(value: promotion.PromotionID.ToString())
                        .AppendCell(promotion.PromotionID.ToString())
						.AppendLinkCell(editUrl, promotion.Description)
						.AppendCell(promotion.StartDate.HasValue ? promotion.StartDate.Value.ToString("g", CoreContext.CurrentCultureInfo) : string.Empty)
						.AppendCell(promotion.EndDate.HasValue ? promotion.EndDate.Value.ToString("g", CoreContext.CurrentCultureInfo) : string.Empty)
						.AppendCell(promoStatus == PromotionStatus.Enabled ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
						.AppendCell(promotion.PromotionKind)
						.Append("</tr>");
				}

				return Json(new { result = true, totalPages = Math.Ceiling(promotions.Count() / pageSize.ToDouble()), page = builder.ToString() });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult DeletePromotions(List<int> items)
		{
			try
			{
				foreach (var promotionID in items)
				{
					var service = Create.New<IPromotionService>();
					var promotion = service.GetPromotion(promotionID);
					promotion.PromotionStatusTypeID = (int)PromotionStatus.Archived;
					var promotionStatus = Create.New<IPromotionState>();
					service.UpdatePromotion(promotion, out promotionStatus);
					if (!promotionStatus.IsValid)
						throw new Exception(String.Concat(promotionStatus.ConstructionErrors)); ;
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public virtual ActionResult ChangePromotionStatus(List<int> items, bool active)
		{
			try
			{
				if (items != null)
				{
					foreach (var promotionID in items)
					{
						var service = Create.New<IPromotionService>();
						var promotion = service.GetPromotion(promotionID);
						promotion.PromotionStatusTypeID = active ? (int)PromotionStatus.Enabled : (int)PromotionStatus.Disabled;
						var promotionStatus = Create.New<IPromotionState>();
						service.UpdatePromotion(promotion, out promotionStatus);
						if (!promotionStatus.IsValid)
							throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
					}
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Search(string query)
		{
			try
			{
				query = query.ToCleanString();
				var searchResults = Product.SlimSearch(query).Where(p => !p.IsVariantTemplate);
				return Json(searchResults.Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name }));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
