using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IAccountListQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountListQualificationRepository : BasePromotionQualificationRepository<IAccountListQualificationExtension, PromotionQualificationAccountList>, IAccountListQualificationRepository
    {

        protected override PromotionQualificationAccountList Convert(IAccountListQualificationExtension dto)
        {
            PromotionQualificationAccountList accountList = base.Convert(dto);
            if (dto.AccountNumbers != null)
            {
                foreach (int accountID in dto.AccountNumbers)
                {
                    accountList.PromotionQualificationAccountListItems.Add(new PromotionQualificationAccountListItem() { AccountID = accountID });
                }
            }
            return accountList;
        }

        protected override IAccountListQualificationExtension Convert(PromotionQualificationAccountList entity)
        {

            IAccountListQualificationExtension extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationAccountListItems)
            {
                extension.AccountNumbers.Add(item.AccountID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationAccountListItems" };
            }
        }
    }
}
