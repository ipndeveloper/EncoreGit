using System.Data.Entity;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.AccountArguments.Repository.Entities;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;

namespace NetSteps.Events.AccountArguments.Repository.Context
{
	[ContainerRegister(typeof(IAccountEventArgumentContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AccountEventArgumentContext : DbContext, IAccountEventArgumentContext
	{
		public AccountEventArgumentContext() : base("name=" + ConnectionStringNames.Core) { }

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public IDbSet<AccountEventArgumentEntity> AccountEventArguments { get; set; }
	}

	public interface IAccountEventArgumentContext : IDbContext
	{
		IDbSet<AccountEventArgumentEntity> AccountEventArguments { get; }
	}
}
