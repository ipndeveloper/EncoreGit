using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Entity;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services
{
    public interface IPromotionAccountAlertRepository : IEntityModelRepository<E.PromotionAccountAlert, IPromotionAccountAlert, ICommunicationContext>
    {
    }

    [ContainerRegister(typeof(IPromotionAccountAlertRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PromotionAccountAlertRepository : EntityModelRepository<E.PromotionAccountAlert, IPromotionAccountAlert, ICommunicationContext>, IPromotionAccountAlertRepository
    {
        public override Expression<Func<E.PromotionAccountAlert, bool>> GetPredicateForModel(IPromotionAccountAlert m)
        {
            return e => e.AccountAlertId == m.AccountAlertId;
        }

        public override void UpdateEntity(E.PromotionAccountAlert e, IPromotionAccountAlert m)
        {
            if (e.AccountAlert == null)
            {
                e.AccountAlert = new E.AccountAlert();
            }

            e.AccountAlert.AccountId = m.AccountId;
            e.AccountAlert.CreatedDateUtc = m.CreatedDateUtc;
            e.AccountAlert.ExpirationDateUtc = m.ExpirationDateUtc;
            e.AccountAlert.DismissedDateUtc = m.DismissedDateUtc;
            e.AccountAlert.AccountAlertDisplayKindId = m.AccountAlertDisplayKindId;
            e.AccountAlert.ProviderKey = m.ProviderKey;
            e.PromotionId = m.PromotionId;
        }

        public override void UpdateModel(IPromotionAccountAlert m, E.PromotionAccountAlert e)
        {
            m.AccountAlertId = e.AccountAlertId;
            m.AccountId = e.AccountAlert.AccountId;
            m.CreatedDateUtc = e.AccountAlert.CreatedDateUtc;
            m.ExpirationDateUtc = e.AccountAlert.ExpirationDateUtc;
            m.DismissedDateUtc = e.AccountAlert.DismissedDateUtc;
            m.AccountAlertDisplayKindId = e.AccountAlert.AccountAlertDisplayKindId;
            m.PromotionId = e.PromotionId;
        }
    }
}
