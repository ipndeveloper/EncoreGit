using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(ICustomerIsHostQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class CustomerIsHostQualificationRepository : BasePromotionQualificationRepository<IAccountTypeQualificationExtension, PromotionQualificationCustomerIsHost>, ICustomerIsHostQualificationRepository
    {
        public new void DeleteExtension(int PromotionQualificationID, Data.Common.IUnitOfWork unitOfWork)
        {
            // nothing to delete
        }

        public new ICustomerIsHostQualificationExtension RetrieveExtension(Promotions.Common.Model.IPromotionQualification qualification, Data.Common.IUnitOfWork unitOfWork)
        {
            return Create.New<ICustomerIsHostQualificationExtension>();
        }

        public ICustomerIsHostQualificationExtension SaveExtension(ICustomerIsHostQualificationExtension qualificationExtension, Data.Common.IUnitOfWork unitOfWork)
        {
            // nothing to save
            return qualificationExtension;
        }
    }
}
