using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(ICustomerPriceTypeTotalRangeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class CustomerPriceTypeTotalRangeQualificationRepository : BasePromotionQualificationRepository<ICustomerPriceTypeTotalRangeQualificationExtension, PromotionQualificationCustomerPriceTypeTotalRange>, ICustomerPriceTypeTotalRangeQualificationRepository
    {

		protected override PromotionQualificationCustomerPriceTypeTotalRange Convert(ICustomerPriceTypeTotalRangeQualificationExtension dto)
        {
            var extension = base.Convert(dto);
            if (dto.CustomerPriceTypeTotalRangesByCurrencyID != null)
            {
                foreach (var currencyID in dto.CustomerPriceTypeTotalRangesByCurrencyID.Keys)
                {
                	var priceTypeTotalRange = dto.CustomerPriceTypeTotalRangesByCurrencyID[currencyID];
					extension.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts.Add(GetNewPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount(currencyID, priceTypeTotalRange));
                }
            }
            extension.ProductPriceTypeID = dto.ProductPriceTypeID;
            return extension;
        }

		protected override ICustomerPriceTypeTotalRangeQualificationExtension Convert(PromotionQualificationCustomerPriceTypeTotalRange entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts)
            {
                extension.CustomerPriceTypeTotalRangesByCurrencyID.Add(item.CurrencyID, new CustomerPriceTypeTotalRange(item.MinimumAmount, item.MaximumAmount));
            }
            extension.ProductPriceTypeID = entity.ProductPriceTypeID;
            return extension;
        }

		protected PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount GetNewPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount(int currencyId, ICustomerPriceTypeTotalRange priceTypeTotalRange)
		{
			if (priceTypeTotalRange == default(ICustomerPriceTypeTotalRange))
			{
				return new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount
					{
						CurrencyID = currencyId,
						MinimumAmount = 0,
						MaximumAmount = 0,
					};
			}

			return new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount
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
                return new string[] { "PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts" };
            }
        }
    }
}
