using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IAccountTypeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountTypeQualificationRepository : BasePromotionQualificationRepository<IAccountTypeQualificationExtension, PromotionQualificationAccountTypeList>, IAccountTypeQualificationRepository
    {

        protected override PromotionQualificationAccountTypeList Convert(IAccountTypeQualificationExtension dto)
        {
            PromotionQualificationAccountTypeList accountTypeList = base.Convert(dto);
            if (dto.AccountTypes != null)
            {
                foreach (var accountTypeID in dto.AccountTypes)
                {
                    accountTypeList.PromotionQualificationAccountTypeListItems.Add(new PromotionQualificationAccountTypeListItem() { AccountTypeID = accountTypeID });
                }
            }
            return accountTypeList;
        }

        protected override IAccountTypeQualificationExtension Convert(PromotionQualificationAccountTypeList entity)
        {
            IAccountTypeQualificationExtension extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationAccountTypeListItems)
            {
                extension.AccountTypes.Add(item.AccountTypeID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationAccountTypeListItems" };
            }
        }
    }
}
