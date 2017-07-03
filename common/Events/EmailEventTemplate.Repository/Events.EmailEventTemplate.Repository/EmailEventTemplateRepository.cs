using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Events.EmailEventTemplate.Repository.Context;
using NetSteps.Events.EmailEventTemplate.Repository.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.EmailEventTemplate.Common;

namespace NetSteps.Events.EmailEventTemplate.Repository
{
	[ContainerRegister(typeof(IEmailEventTemplateRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EmailEventTemplateRepository : NetSteps.Foundation.Entity.EntityModelRepository<EmailEventTemplateEntity, IEmailEventTemplate, IEmailEventTemplateContext>, IEmailEventTemplateRepository
	{
		public int GetEmailTemplateIdByEventTypeID(int eventTypeID)
		{
			using (var context = Create.New<IEmailEventTemplateContext>())
			{
				IEmailEventTemplate current = FirstOrDefault(context, e => e.EventTypeID == eventTypeID);
				return current != null ? current.EmailTemplateID : 0;
			}
		}

		public override Expression<Func<EmailEventTemplateEntity, bool>> GetPredicateForModel(IEmailEventTemplate model)
		{
			return m => m.EmailEventEmailTemplateID == model.EmailEventEmailTemplateID;
		}

		public override void UpdateEntity(EmailEventTemplateEntity entity, IEmailEventTemplate model)
		{
			entity.EmailTemplateID = model.EmailTemplateID;
			entity.EventTypeID = model.EventTypeID;
		}

		public override void UpdateModel(IEmailEventTemplate model, EmailEventTemplateEntity entity)
		{
			model.EmailEventEmailTemplateID = entity.EmailEventEmailTemplateID;
			model.EmailTemplateID = entity.EmailTemplateID;
			model.EventTypeID = entity.EventTypeID;
		}
	}
}
