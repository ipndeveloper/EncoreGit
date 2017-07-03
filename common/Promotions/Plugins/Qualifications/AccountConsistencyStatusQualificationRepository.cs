using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IAccountConsistencyStatusQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountConsistencyStatusQualificationRepository : BasePromotionQualificationRepository<IAccountConsistencyStatusQualificationExtension, PromotionQualificationAccountConsistencyStatusList>, IAccountConsistencyStatusQualificationRepository
    {

        protected override PromotionQualificationAccountConsistencyStatusList Convert(IAccountConsistencyStatusQualificationExtension dto)
        {
            PromotionQualificationAccountConsistencyStatusList accountConsistencyStatusList = base.Convert(dto);
            if (dto.AccountConsistencyStatuses != null)
            {
                foreach (var accountConsistencyStatusID in dto.AccountConsistencyStatuses)
                {
                    accountConsistencyStatusList.PromotionQualificationAccountConsistencyStatusListItems.Add(new PromotionQualificationAccountConsistencyStatusListItem() { AccountConsistencyStatusID = accountConsistencyStatusID });
                }
            }
            return accountConsistencyStatusList;
        }

        protected override IAccountConsistencyStatusQualificationExtension Convert(PromotionQualificationAccountConsistencyStatusList entity)
        {
            IAccountConsistencyStatusQualificationExtension extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationAccountConsistencyStatusListItems)
            {
                extension.AccountConsistencyStatuses.Add(item.AccountConsistencyStatusID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationAccountConsistencyStatusListItems" };
            }
        }
    }
}
