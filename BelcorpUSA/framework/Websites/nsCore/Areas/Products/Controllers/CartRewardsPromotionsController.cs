using System;
using System.Web.Mvc;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using nsCore.Areas.Products.Models.Promotions;
using nsCore.Areas.Products.Models.Promotions.Converters;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;/*R2804 - CGI(JICM)*/
using NetSteps.Data.Entities.Business.Logic;/*R2804 - CGI(JICM)*/
using System.Collections.Generic;/*R2804 - CGI(JICM)*/
using System.Linq;/*R2804 - CGI(JICM)*/
using NetSteps.Data.Entities;/*R2804 - CGI(JICM)*/
using NetSteps.Common.Globalization; /*R2804 - CGI(JICM)*/
namespace nsCore.Areas.Products.Controllers
{
    public class CartRewardsPromotionsController : PromotionsController
    {
        /*R2804 - CGI(JICM) - INTEGRACION BR - INI*/
        public ActionResult Edit(int? id)
        {
            var model = Create.New<ICartRewardsPromotionModel>();
            if (id.HasValue)
            {
                var promo = Create.New<IPromotionService>().GetPromotion<IOrderPromotionDefaultCartRewards>(id.Value);
                var converter = Create.New<IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>>();
                model = converter.Convert(promo);
                //var promotion = PromoPromotionLogic.Instance.GetById(id.Value);
                //if (promotion != null)
                //{
                //    ViewData["cumulative"] = promotion.Cumulative;
                //    ViewData["conditionProductPriceTypeId"] = promotion.ConditionProductPriceTypeId;
                //    ViewData["rewardProductPriceTypeId"] = promotion.RewardProductPriceTypeId;
                //}
            }
            return View(model);
        }
        /*R2804 - CGI(JICM) - INTEGRACION BR - INI*/

        /*R2804 - CGI(JICM) - INTEGRACION BR - INI*/
        public ActionResult Save(CartRewardsPromotionModel model)
        {
            try
            {
                // save promotion!
                var service = Create.New<IPromotionService>();
                var converter = Create.New<IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>>();
                var promotion = converter.Convert(model);
                var rewards = promotion.PromotionRewards.Values;


                if (model.PromotionID > 0)
                {
                    var promotionStatus = Create.New<IPromotionState>();
                    var promotionUpdated = service.UpdatePromotion(promotion, out promotionStatus);

                    //actualizar campo cumulative
                    if (promotionStatus.IsValid)
                    {
                        //UpdateCumulative(promotionUpdated.PromotionQualifications, (ICustomerQVRangeCartCondition)(model.CartCondition));
                        //UpdateRewardEffectProductPriceType(promotionUpdated.PromotionID, (ICartDiscountCartReward)(model.CartReward));
                        ////UpdateCumulative(promotionUpdated.PromotionQualifications, (ISingleProductCartCondition)(model.CartCondition));
                        ////UpdateRewardEffectProductPriceType(promotionUpdated.PromotionID, (ICartDiscountCartReward)(model.CartReward));
                    }
                    else
                    {
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
                    }
                }
                else
                {
                    var promotionStatus = Create.New<IPromotionState>();
                    var promotionAdded = service.AddPromotion(promotion, out promotionStatus);
                    var promotionRewardID = promotionAdded.PromotionRewards.Select(m => m.Value.PromotionRewardID);

                    //insertar campo cumulative
                    if (promotionStatus.IsValid)
                    {
                        //UpdateCumulative(promotionAdded.PromotionQualifications, (ICustomerQVRangeCartCondition)(model.CartCondition));
                        //InsertRewardEffectProductPriceType(promotionAdded.PromotionID, (ICartDiscountCartReward)(model.CartReward));
                        ////UpdateCumulative(promotionAdded.PromotionQualifications, (ISingleProductCartCondition)(model.CartCondition));
                        ////InsertRewardEffectProductPriceType(promotionAdded.PromotionID, (IAddProductsToCartReward)(model.CartReward));
                    }
                    else
                    {
                        throw new Exception(String.Concat(promotionStatus.ConstructionErrors));
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



        /// <summary>
        /// Inserta en la nueva tabla que indica a que tipo de precio se afectara dicho porcentaje de descuento
        /// </summary>
        /// <param name="promotionID"></param>
        /// <param name="iCartDiscountCartReward"></param>
        //private void InsertRewardEffectProductPriceType(int promotionID, ICartDiscountCartReward iCartDiscountCartReward)
        private void InsertRewardEffectProductPriceType(int promotionID, IAddProductsToCartReward iCartDiscountCartReward)
        {
            PromoPromotionLogic.Instance.InsertPromotionRewardEffectApplyOrderItemPropertyValues(promotionID, iCartDiscountCartReward.ProductPriceTypeID);
        }

        /// <summary>
        /// Actualiza el tipo de precio que se vera afecto el porcentaje
        /// </summary>
        /// <param name="promotionID"></param>
        /// <param name="iCartDiscountCartReward"></param>
        private void UpdateRewardEffectProductPriceType(int promotionID, ICartDiscountCartReward iCartDiscountCartReward)
        {
            PromoPromotionLogic.Instance.UpdatePromotionRewardEffectApplyOrderItemPropertyValues(promotionID, iCartDiscountCartReward.ProductPriceTypeID);
        }

        /// <summary>
        /// Actualiza el campo Cumulative de los requisitos para la promocion
        /// </summary>
        /// <param name="iDictionary"></param>
        /// <param name="iCustomerQVRangeCartCondition"></param>
        //private void UpdateCumulative(IDictionary<string, IPromotionQualificationExtension> iDictionary, ICustomerQVRangeCartCondition iCustomerQVRangeCartCondition)
        private void UpdateCumulative(IDictionary<string, IPromotionQualificationExtension> iDictionary, ISingleProductCartCondition iCustomerQVRangeCartCondition)
        {
            foreach (var item in iDictionary)
            {
                PromoPromotionLogic.Instance.UpdatePromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts(item.Value.PromotionQualificationID, iCustomerQVRangeCartCondition.Cumulative);
            }
        }

        [HttpGet]
        public virtual ActionResult ListProductPriceTypes()
        {
            try
            {
                var data = ProductPriceTypeLogic.Instance.ListProductPriceTypes();
                var newListData = new List<ProductPriceType>();

                foreach (var item in data)
                {
                    var newData = new ProductPriceType();
                    newData.ProductPriceTypeID = item.ProductPriceTypeID;
                    newData.TermName = Translation.GetTerm(item.TermName, "");

                    newListData.Add(newData);
                }
                return Json(newListData.Select(p => new { ProductPriceTypeID = p.ProductPriceTypeID, TermName = p.TermName }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        /*R2804 - CGI(JICM) - INTEGRACION BR - FIN*/
    }
}