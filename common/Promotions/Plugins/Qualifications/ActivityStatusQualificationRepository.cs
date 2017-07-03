using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IActivityStatusQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ActivityStatusQualificationRepository : BasePromotionQualificationRepository<IActivityStatusQualificationExtension, PromotionQualificationActivityStatusList>, IActivityStatusQualificationRepository
    {

        protected override PromotionQualificationActivityStatusList Convert(IActivityStatusQualificationExtension dto)
        {
            PromotionQualificationActivityStatusList activityStatusList = base.Convert(dto);
            if (dto.ActivityStatuses != null)
            {
                foreach (var activityStatusID in dto.ActivityStatuses)
                {
                    activityStatusList.PromotionQualificationActivityStatusListItems.Add(new PromotionQualificationActivityStatusListItem() { ActivityStatusID = activityStatusID });
                }
            }
            return activityStatusList;
        }

        protected override IActivityStatusQualificationExtension Convert(PromotionQualificationActivityStatusList entity)
        {
            IActivityStatusQualificationExtension extension = base.Convert(entity);
            foreach (var item in entity.PromotionQualificationActivityStatusListItems)
            {
                extension.ActivityStatuses.Add(item.ActivityStatusID);
            }
            return extension;
        }

        protected override string[] Includes
        {
            get
            {
                return new string[] { "PromotionQualificationActivityStatusListItems" };
            }
        }
    }
}
