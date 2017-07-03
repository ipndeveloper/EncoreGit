using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;
using E = NetSteps.Accounts.Downline.Service.Entities;

namespace NetSteps.Accounts.Downline.Service.Context
{
	[ContainerRegister(typeof(IDownlineDbContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DownlineDbContext : DbContext, IDownlineDbContext
	{
		public DownlineDbContext() : base("name=" + ConnectionStringNames.Core) { }

		public DownlineDbContext(string nameOrConnectionString) : base(nameOrConnectionString) {  }

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public IDbSet<E.AccountInfoCache> AccountInfoCache { get; set; }
		public IDbSet<E.Account> Accounts { get; set; }
		public IDbSet<E.SponsorHierarchy> SponsorHierarchies { get; set; }
	}
}
