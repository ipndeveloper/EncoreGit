using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Entity;
using E = NetSteps.Accounts.Downline.Service.Entities;

namespace NetSteps.Accounts.Downline.Service.Context
{
	public interface IDownlineDbContext : IDbContext
	{
		Database Database { get; }

		IDbSet<E.AccountInfoCache> AccountInfoCache { get; }
		IDbSet<E.Account> Accounts { get; }
		IDbSet<E.SponsorHierarchy> SponsorHierarchies { get; }
	}
}
