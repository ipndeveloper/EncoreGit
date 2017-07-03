using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IOrderPriceTypeTotalRangeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class OrderPriceTypeTotalRangeQualificationRepository : BasePromotionQualificationRepository<IOrderPriceTypeTotalRangeQualificationExtension, PromotionQualificationOrderPriceTypeTotalRange>, IOrderPriceTypeTotalRangeQualificationRepository
    {

		protected override PromotionQualificationOrderPriceTypeTotalRange Convert(IOrderPriceTypeTotalRangeQualificationExtension dto)
        {
            var extension = base.Convert(dto);
            if (dto.OrderPriceTypeTotalRangesByCurrencyID != null)
            {
                foreach (var currencyID in dto.OrderPriceTypeTotalRangesByCurrencyID.Keys)
                {
                	var priceTypeTotalRange = dto.OrderPriceTypeTotalRangesByCurrencyID[currencyID];
					extension.PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmounts.Add(GetNewPromotionQualificationPriceTypeTotalRangeCurrencyAmount(currencyID, priceTypeTotalRange));
                }
            }
            return extension;
        }

		protected override IOrderPriceTypeTotalRangeQualificationExtension Convert(PromotionQualificationOrderPriceTypeTotalRange entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmounts)
            {
                extension.OrderPriceTypeTotalRangesByCurrencyID.Add(item.CurrencyID, new OrderPriceTypeTotalRange(item.MinimumAmount, item.MaximumAmount));
            }
            return extension;
        }

		protected PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmount GetNewPromotionQualificationPriceTypeTotalRangeCurrencyAmount(int currencyId, IOrderPriceTypeTotalRange priceTypeTotalRange)
		{
			if (priceTypeTotalRange == default(IOrderPriceTypeTotalRange))
			{
				return new PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmount
					{
						CurrencyID = currencyId,
						MinimumAmount = 0,
						MaximumAmount = 0,
					};
			}

			return new PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmount
				{
					CurrencyID = currencyId,
					MinimumAmount = priceTypeTotalRange.Minimum,
					MaximumAmount = priceTypeTotalRange.Maximum
				};
		}

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationOrderPriceTypeTotalRangeCurrencyAmounts" };
            }
        }
    }
}
