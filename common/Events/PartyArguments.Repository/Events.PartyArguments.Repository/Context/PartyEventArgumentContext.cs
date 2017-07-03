using System.Data.Entity;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.PartyArguments.Repository.Entities;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;

namespace NetSteps.Events.PartyArguments.Repository.Context
{
	[ContainerRegister(typeof(IPartyEventArgumentContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PartyEventArgumentContext : DbContext, IPartyEventArgumentContext
	{
		public PartyEventArgumentContext() : base("name=" + ConnectionStringNames.Core) { }

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public IDbSet<PartyEventArgumentEntity> PartyEventArguments { get; set; }
	}

	public interface IPartyEventArgumentContext : IDbContext
	{
		IDbSet<PartyEventArgumentEntity> PartyEventArguments { get; }
	}
}
