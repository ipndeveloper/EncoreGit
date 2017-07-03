using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IMarketListQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class MarketListQualificationRepository : BasePromotionQualificationRepository<IMarketListQualificationExtension, PromotionQualificationMarketList>, IMarketListQualificationRepository
    {

        protected override PromotionQualificationMarketList Convert(IMarketListQualificationExtension dto)
        {
            PromotionQualificationMarketList marketList = base.Convert(dto);
            if (dto.Markets != null)
            {
                foreach (var marketID in dto.Markets)
                {
					marketList.PromotionQualificationMarketListItems.Add(new PromotionQualificationMarketListItem() { MarketID = marketID });
                }
            }
			return marketList;
        }

        protected override IMarketListQualificationExtension Convert(PromotionQualificationMarketList entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationMarketListItems)
            {
                extension.Markets.Add(item.MarketID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationMarketListItems" };
            }
        }
    }
}
