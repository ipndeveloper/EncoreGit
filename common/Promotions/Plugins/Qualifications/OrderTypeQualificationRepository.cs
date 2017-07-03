using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IOrderTypeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class OrderTypeQualificationRepository : BasePromotionQualificationRepository<IOrderTypeQualificationExtension, PromotionQualificationOrderTypeList>, IOrderTypeQualificationRepository
    {

        protected override PromotionQualificationOrderTypeList Convert(IOrderTypeQualificationExtension dto)
        {
            var orderTypeQualification = base.Convert(dto);
            if (dto.OrderTypes != null)
            {
                foreach (var orderType in dto.OrderTypes)
                {
                    orderTypeQualification.PromotionQualificationOrderTypeListItems.Add(new PromotionQualificationOrderTypeListItem() { OrderTypeID = (short)orderType });
                }
            }
            return orderTypeQualification;
        }

        protected override IOrderTypeQualificationExtension Convert(PromotionQualificationOrderTypeList entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationOrderTypeListItems)
            {
                extension.OrderTypes.Add(item.OrderTypeID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationOrderTypeListItems" };
            }
        }
    }
}
