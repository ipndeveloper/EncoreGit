using NetSteps.Foundation.Entity;
using E = NetSteps.Communication.Services.Entities;
using NetSteps.Communication.Common;
using System.Linq.Expressions;
using System;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.Services
{
    public interface IMessageAccountAlertRepository : IEntityModelRepository<E.MessageAccountAlert, IMessageAccountAlert, ICommunicationContext>
    {
    }

    [ContainerRegister(typeof(IMessageAccountAlertRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class MessageAccountAlertRepository : EntityModelRepository<E.MessageAccountAlert, IMessageAccountAlert, ICommunicationContext>, IMessageAccountAlertRepository
    {
        public override Expression<Func<E.MessageAccountAlert, bool>> GetPredicateForModel(IMessageAccountAlert model)
        {
            return a => a.AccountAlertId == model.AccountAlertId;
        }

        public override void UpdateEntity(E.MessageAccountAlert e, IMessageAccountAlert m)
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
            e.Message = m.Message;
        }

        public override void UpdateModel(IMessageAccountAlert m, E.MessageAccountAlert e)
        {
            m.AccountAlertId = e.AccountAlertId;
            m.AccountId = e.AccountAlert.AccountId;
            m.CreatedDateUtc = e.AccountAlert.CreatedDateUtc;
            m.ExpirationDateUtc = e.AccountAlert.ExpirationDateUtc;
            m.DismissedDateUtc = e.AccountAlert.DismissedDateUtc;
            m.AccountAlertDisplayKindId = e.AccountAlert.AccountAlertDisplayKindId;
            m.Message = e.Message;
        }
    }
}
