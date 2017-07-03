using System.Data.Entity;
using NetSteps.Events.EmailEventTemplate.Repository.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;

namespace NetSteps.Events.EmailEventTemplate.Repository.Context
{
	[ContainerRegister(typeof(IEmailEventTemplateContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class EmailEventTemplateContext : DbContext, IEmailEventTemplateContext
	{
		public EmailEventTemplateContext() : base("name=" + ConnectionStringNames.Core) { }

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public IDbSet<EmailEventTemplateEntity> EmailEventTemplates { get; set; }
	}

	public interface IEmailEventTemplateContext : IDbContext
	{
		IDbSet<EmailEventTemplateEntity> EmailEventTemplates { get; }
	} 
}
