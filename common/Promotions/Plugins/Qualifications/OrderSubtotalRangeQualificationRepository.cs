using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IOrderSubtotalRangeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class OrderSubtotalRangeQualificationRepository : BasePromotionQualificationRepository<IOrderSubtotalRangeQualificationExtension, PromotionQualificationOrderSubtotalRange>, IOrderSubtotalRangeQualificationRepository
    {

		protected override PromotionQualificationOrderSubtotalRange Convert(IOrderSubtotalRangeQualificationExtension dto)
        {
            var SubtotalRange = base.Convert(dto);
            if (dto.OrderSubtotalRangesByCurrencyID != null)
            {
                foreach (var currencyID in dto.OrderSubtotalRangesByCurrencyID.Keys)
                {
                	var subtotalRange = dto.OrderSubtotalRangesByCurrencyID[currencyID];
					SubtotalRange.PromotionQualificationOrderSubtotalRangeCurrencyAmounts.Add(GetNewPromotionQualificationSubtotalRangeCurrencyAmount(currencyID, subtotalRange));
                }
            }
            return SubtotalRange;
        }

		protected override IOrderSubtotalRangeQualificationExtension Convert(PromotionQualificationOrderSubtotalRange entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationOrderSubtotalRangeCurrencyAmounts)
            {
                extension.OrderSubtotalRangesByCurrencyID.Add(item.CurrencyID, new OrderSubtotalRange(item.MinimumAmount, item.MaximumAmount));
            }
            return extension;
        }

		protected PromotionQualificationOrderSubtotalRangeCurrencyAmount GetNewPromotionQualificationSubtotalRangeCurrencyAmount(int currencyId, IOrderSubtotalRange subtotalRange)
		{
			if (subtotalRange == default(IOrderSubtotalRange))
			{
				return new PromotionQualificationOrderSubtotalRangeCurrencyAmount
					{
						CurrencyID = currencyId,
						MinimumAmount = 0,
						MaximumAmount = 0,
					};
			}

			return new PromotionQualificationOrderSubtotalRangeCurrencyAmount
				{
					CurrencyID = currencyId,
					MinimumAmount = subtotalRange.Minimum,
					MaximumAmount = subtotalRange.Maximum
				};
		}

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationOrderSubtotalRangeCurrencyAmounts" };
            }
        }
    }
}
