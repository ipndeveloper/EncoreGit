using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Data.Entities.Business
{
    public class OrderPromotionQualification : IOrderPromotionQualification
    {
        /// <summary>
        /// Additional validation rules that are checked in CanAddPromotionCode() method 
        /// before code is added to OrderContext - JHE
        /// </summary>
        public virtual IEnumerable<Func<IOrderContext, string, BasicResponse>> Qualifiers
        {
            get
            {
                return Enumerable.Empty<Func<IOrderContext, string, BasicResponse>>();
            }
        }

        // TODO: Finish this - JHE
        public BasicResponse ApplyPromotion(IOrderContext context, IOrderCustomer customer, string promotionCode)
        {
            var basicResponse = new BasicResponse();
            if (promotionCode.IsNullOrEmpty())
            {
                basicResponse.Success = false;
                basicResponse.Message = Translation.GetTerm("InvalidPromotionCodeMessage", "The promotion could not be applyed. Invalid promotion code: '{0}'", args: promotionCode);
                return basicResponse;
            }

            var canAddPromotionCodeResponse = CanAddPromotionCode(context, customer, promotionCode);
	        if(!canAddPromotionCodeResponse.Success)
	        {
		        return canAddPromotionCodeResponse;
	        }

			if (!context.CouponCodes.Any(existing => existing.AccountID == customer.AccountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase)))
			{
				var couponCode = Create.New<ICouponCode>();
				couponCode.AccountID = customer.AccountID;
				couponCode.CouponCode = promotionCode;
				context.CouponCodes.Add(couponCode);
			}

            //List<IOrderAdjustmentProfile> adjustments = (List<IOrderAdjustmentProfile>)adjustmentService.GetApplicableOrderAdjustments(context);
            // TODO: Finish this - JHE

            basicResponse.Success = true;
            return basicResponse;
        }

        public BasicResponse RemovePromotion(IOrderContext context, IOrderCustomer customer, string promotionCode)
        {
            var basicResponse = new BasicResponse();

			if (!context.CouponCodes.Any(existing => existing.AccountID == customer.AccountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                basicResponse.Success = false;
                basicResponse.Message = Translation.GetTerm("PromotionCodeNotAppliedToOrderMessage", "The promotion could not be removed. Promotion code: '{0}' not found on order.", args: promotionCode);
                return basicResponse;
            }

            // List<IOrderAdjustmentProfile> adjustments = (List<IOrderAdjustmentProfile>)adjustmentService.GetApplicableOrderAdjustments(context);
            // TODO: Finish this - JHE

            var couponCode = context.CouponCodes.SingleOrDefault(code => code.AccountID == customer.AccountID && code.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase));
	        if(couponCode != null)
	        {
		        context.CouponCodes.Remove(couponCode);
	        }

            basicResponse.Success = true;
            return basicResponse;
        }

        public IEnumerable<IPromotion> SearchPromotions(IOrderContext context, string promotionCode, int? accountID = null, Predicate<IPromotion> filter = null)
        {
            var promotionService = Create.New<IPromotionService>();

            var promotions = promotionService.GetPromotions(i =>
                                                                (
                                                                    !accountID.HasValue || i.ValidFor("AccountID", accountID.Value)) &&
                                                                    (String.IsNullOrEmpty(promotionCode) || i.ValidFor("CouponCode", promotionCode) &&
                                                                    (filter == null || filter(i))
                                                                )
                                                            );

            return promotions;
        }

        // TODO: Miche implementation. Move this to Miche overrides once prototyped and functional - JHE
        public BasicResponse CanAddPromotionCode(IOrderContext context, IOrderCustomer customer, string promotionCode)
        {
            var basicResponse = new BasicResponse();

            if (!SearchPromotions(context, promotionCode).Any())
            {
                basicResponse.Success = false;
                basicResponse.Message = Translation.GetTerm("InvalidPromotionCode", "Invalid promotion code");
                return basicResponse;
            }

            foreach (var qualifier in Qualifiers)
            {
                var qualifyingResponse = qualifier(context, promotionCode);
                if (qualifyingResponse.Success == false)
                {
                    return qualifyingResponse;
                }
            }

            basicResponse.Success = true;
            return basicResponse;
        }
    }
}