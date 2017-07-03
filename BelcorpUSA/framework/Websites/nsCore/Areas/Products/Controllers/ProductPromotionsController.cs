using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Products.Models.Promotions;
using nsCore.Areas.Products.Models.Promotions.Base;
using nsCore.Areas.Products.Models.Promotions.Converters;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.Products.Controllers
{
    public enum AdjustmentType
    {
        PercentOff,
        FlatDiscount
    }

    public class ProductPromotionsController : BaseProductsController
    {
        /// <summary>
        /// List of price types besides retail that promotional discounts are applied to.  This varies per client. 
        /// </summary>
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

        public ActionResult Edit(int? id)
        {
            PriceAdjustmentPromotionModel model = null;
            if (id.HasValue)
            {
                var promo = Create.New<IPromotionService>().GetPromotion(id.Value);
                if (promo.PromotionKind == PromotionKindNames.ProductFlatDiscount)
                {
                    model = Create.New<IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>>().Convert((IProductPromotionFlatDiscount)promo);
                    ViewBag.AdjustmentTypeID = (int)AdjustmentType.FlatDiscount;
                    TempData["OrConditionFlatDiscount"] = PromoPromotionLogic.Instance.ExistsOrCondition(Convert.ToInt32(id));

                    var first = PromoPromotionLogic.Instance.ExistsAndConditionQVTotal(Convert.ToInt32(id)).First();
                    TempData["AndConditionQvTotalFlatDiscount"] = first.Key;
                    TempData["QvMinFlatDiscount"] = first.Value.First().Key;
                    TempData["QvMaxFlatDiscount"] = first.Value.First().Value;
                }
                else if (promo.PromotionKind == PromotionKindNames.ProductPercentDiscount)
                {
                    model = Create.New<IPromotionModelConverter<PercentOffPromotionModel, IProductPromotionPercentDiscount>>().Convert((IProductPromotionPercentDiscount)promo);
                    ViewBag.AdjustmentTypeID = (int)AdjustmentType.PercentOff;
                    TempData["OrConditionPercentOff"] = PromoPromotionLogic.Instance.ExistsOrCondition(Convert.ToInt32(id));

                    var first = PromoPromotionLogic.Instance.ExistsAndConditionQVTotal(Convert.ToInt32(id)).First();
                    TempData["AndConditionQvTotalPercentOff"] = first.Key;
                    TempData["QvMinPercentOff"] = first.Value.First().Key;
                    TempData["QvMaxPercentOff"] = first.Value.First().Value;
                }
            }
            else
            {
                model = new PercentOffPromotionModel();
            }
            return View(model);
        }

        public ActionResult GetPartialForAdjustmentType(AdjustmentType adjustmentType, int? promotionID = null)
        {
            try
            {
                var inventory = Create.New<InventoryBaseRepository>();
                PriceAdjustmentPromotionModel model = null;
                if (promotionID.HasValue && promotionID != 0)
                {
                    var promo = Create.New<IPromotionService>().GetPromotion(promotionID.Value);
                    if (typeof(IProductPromotionFlatDiscount).IsAssignableFrom(promo.GetType()))
                    {
                        var converter = Create.New<IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>>();
                        model = converter.Convert((IProductPromotionFlatDiscount)promo);
                        ((FlatDiscountPromotionModel)model).PromotionProducts.ForEach(p => p.LoadResources(inventory.GetProduct(p.ProductID), DiscountedPriceTypesToDisplay, GetDefaultCurrencyIDFromMarket(CoreContext.CurrentMarketId)));
                    }
                    else if (typeof(IProductPromotionPercentDiscount).IsAssignableFrom(promo.GetType()))
                    {
                        var converter = Create.New<IPromotionModelConverter<PercentOffPromotionModel, IProductPromotionPercentDiscount>>();
                        model = converter.Convert((IProductPromotionPercentDiscount)promo);
                        ((PercentOffPromotionModel)model).PromotionProducts.ForEach(p => p.LoadResources(inventory.GetProduct(p.ProductID), DiscountedPriceTypesToDisplay, GetDefaultCurrencyIDFromMarket(CoreContext.CurrentMarketId)));
                    }
                }
                else
                {
                    switch (adjustmentType)
                    {
                        case AdjustmentType.PercentOff:
                            model = new PercentOffPromotionModel();
                            break;
                        case AdjustmentType.FlatDiscount:
                            model = new FlatDiscountPromotionModel();
                            break;
                        default:
                            throw new InvalidOperationException(string.Format("View for adjustment type {0} not found", adjustmentType));
                    }
                }
                switch (adjustmentType)
                {
                    case AdjustmentType.PercentOff:
                        return PartialView("_PercentOff", model);
                    case AdjustmentType.FlatDiscount:
                        return PartialView("_FlatDiscount", model);
                    default:
                        throw new InvalidOperationException(string.Format("View for adjustment type {0} not found", adjustmentType));
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult QuickAddProduct(int productID, int? marketID = null)
        {
            try
            {
                int currencyID = GetDefaultCurrencyIDFromMarket(marketID.HasValue ? marketID.Value : CoreContext.CurrentMarketId);
                var product = Inventory.GetProduct(productID);
                var model = new PromotionProductModel();
                return Json(new { result = true, product = model.LoadResources(product, DiscountedPriceTypesToDisplay, currencyID) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(Duration = 120, VaryByParam = "catalogID;marketID")]
        public virtual ActionResult GetProductInformationFromCatalog(int catalogID, int? marketID = null)
        {
            try
            {
                int currencyID = GetDefaultCurrencyIDFromMarket(marketID.HasValue ? marketID.Value : CoreContext.CurrentMarketId);
                var catalog = Catalog.LoadFull(catalogID);

                //do not display variant templates as eligible products for rewards
                var productList = Inventory.GetProducts(catalog.CatalogItems.Select(ci => ci.ProductID)).Where(p => !p.IsVariantTemplate);
                List<PromotionProductModel> products = new List<PromotionProductModel>();
                foreach (var productItem in productList)
                {
                    var model = new PromotionProductModel();
                    model.LoadResources(productItem, DiscountedPriceTypesToDisplay, currencyID);
                    products.Add(model);
                }
                return Json(new { result = true, products });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return JsonError(exception.PublicMessage);
            }
        }

        [OutputCache(Duration = 120)]
        public virtual ActionResult GetAvailableCatalogs()
        {
            try
            {
                int langId = ApplicationContext.Instance.CurrentLanguageID;
                var catalogs = Catalog.LoadAll().Where(c => c.Translations.Any(t => t.LanguageID == langId));
                return Json(new { result = true, catalogs = catalogs.ToDictionary(c => c.CatalogID.ToString(), c => c.Translations.FirstOrDefault(t => t.LanguageID == langId).Name) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return JsonError(exception.PublicMessage);
            }
        }

        public ActionResult SaveFlatDiscountPromotion(FlatDiscountPromotionModel model)
        {
            try
            {
                var promotionService = Create.New<IPromotionService>();

                IProductPromotionFlatDiscount promo;
                if (model.PromotionID == 0)
                {
                    promo = Create.New<IProductPromotionFlatDiscount>();
                }
                else
                {
                    promo = Create.New<IPromotionService>().GetPromotion<IProductPromotionFlatDiscount>(model.PromotionID);
                }

                var converter = Create.New<IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>>();
                promo = converter.Convert(model);
                if (promo.PromotionID == 0)
                {
                    promo.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                    var promotionStatus = Create.New<IPromotionState>();
                    promotionService.AddPromotion(promo, out promotionStatus);
                    if (!promotionStatus.IsValid)
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
                    else
                    {
                        if (model.OrCondition) PromoPromotionLogic.Instance.InsOrCondition(promo.PromotionID);
                        if (model.AndConditionQVTotal) PromoPromotionLogic.Instance.InsAndConditionQVTotal(promo.PromotionID, model.QvMin, model.QvMax);
                    }
                }
                else
                {
                    var promotionStatus = Create.New<IPromotionState>();
                    promotionService.UpdatePromotion(promo, out promotionStatus);
                    if (!promotionStatus.IsValid)
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
                    else
                    {
                        if (model.OrCondition) PromoPromotionLogic.Instance.InsOrCondition(promo.PromotionID);
                        if (model.AndConditionQVTotal) PromoPromotionLogic.Instance.InsAndConditionQVTotal(promo.PromotionID, model.QvMin, model.QvMax);
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

        public ActionResult SavePercentOffPromotion(PercentOffPromotionModel model)
        {
            try
            {
                var promotionService = Create.New<IPromotionService>();

                IProductPromotionPercentDiscount promo;
                if (model.PromotionID == 0)
                {
                    promo = Create.New<IProductPromotionPercentDiscount>();
                }
                else
                {
                    promo = promotionService.GetPromotion<IProductPromotionPercentDiscount>(model.PromotionID);
                }

                var converter = Create.New<IPromotionModelConverter<PercentOffPromotionModel, IProductPromotionPercentDiscount>>();
                promo = converter.Convert(model);
                if (promo.PromotionID == 0)
                {
                    promo.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
                    var promotionStatus = Create.New<IPromotionState>();
                    promotionService.AddPromotion(promo, out promotionStatus);
                    if (!promotionStatus.IsValid)
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
                    else
                    {
                        if (model.OrCondition) PromoPromotionLogic.Instance.InsOrCondition(promo.PromotionID);
                        if (model.AndConditionQVTotal) PromoPromotionLogic.Instance.InsAndConditionQVTotal(promo.PromotionID, model.QvMin, model.QvMax);
                    }
                }
                else
                {
                    var promotionStatus = Create.New<IPromotionState>();
                    promotionService.UpdatePromotion(promo, out promotionStatus);
                    if (!promotionStatus.IsValid)
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
                    else
                    {
                        if (model.OrCondition) PromoPromotionLogic.Instance.InsOrCondition(promo.PromotionID);
                        if (model.AndConditionQVTotal) PromoPromotionLogic.Instance.InsAndConditionQVTotal(promo.PromotionID, model.QvMin, model.QvMax);
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