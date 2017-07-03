using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(ICustomerSubtotalRangeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class CustomerSubtotalRangeQualificationRepository : BasePromotionQualificationRepository<ICustomerSubtotalRangeQualificationExtension, PromotionQualificationCustomerSubtotalRange>, ICustomerSubtotalRangeQualificationRepository
    {

		protected override PromotionQualificationCustomerSubtotalRange Convert(ICustomerSubtotalRangeQualificationExtension dto)
        {
            var SubtotalRange = base.Convert(dto);
            if (dto.CustomerSubtotalRangesByCurrencyID != null)
            {
                foreach (var currencyID in dto.CustomerSubtotalRangesByCurrencyID.Keys)
                {
                	var subtotalRange = dto.CustomerSubtotalRangesByCurrencyID[currencyID];
					SubtotalRange.PromotionQualificationCustomerSubtotalRangeCurrencyAmounts.Add(GetNewPromotionQualificationCustomerSubtotalRangeCurrencyAmount(currencyID, subtotalRange));
                }
            }
            return SubtotalRange;
        }

		protected override ICustomerSubtotalRangeQualificationExtension Convert(PromotionQualificationCustomerSubtotalRange entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationCustomerSubtotalRangeCurrencyAmounts)
            {
                extension.CustomerSubtotalRangesByCurrencyID.Add(item.CurrencyID, new CustomerSubtotalRange(item.MinimumAmount, item.MaximumAmount));
            }
            return extension;
        }

		protected PromotionQualificationCustomerSubtotalRangeCurrencyAmount GetNewPromotionQualificationCustomerSubtotalRangeCurrencyAmount(int currencyId, ICustomerSubtotalRange subtotalRange)
		{
			if (subtotalRange == default(ICustomerSubtotalRange))
			{
				return new PromotionQualificationCustomerSubtotalRangeCurrencyAmount
					{
						CurrencyID = currencyId,
						MinimumAmount = 0,
						MaximumAmount = 0,
					};
			}

			return new PromotionQualificationCustomerSubtotalRangeCurrencyAmount
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
                return new string[] { "PromotionQualificationCustomerSubtotalRangeCurrencyAmounts" };
            }
        }
    }
}
