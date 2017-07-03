using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class MailDomainRepository : BaseRepository<MailDomain, Int32, NetStepsEntities>, IMailDomainRepository, IDefaultImplementation
	{
		#region Members
		#endregion

		#region Methods

		public IEnumerable<string> LoadInternalDomains()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.MailDomains.Select(a => a.DomainName).ToList();
				}
			});
		}

		public MailDomain LoadDefaultForInternal()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.MailDomains.Where(a => a.IsDefaultForInternalMailAccounts).First();
				}
			});
		}

		#endregion

		#region Private Methods

		private static string _connectionString = string.Empty;
		internal static string GetConnectionString()
		{
			if (_connectionString.IsNullOrEmpty())
			{
				using (NetStepsEntities context = CreateContext())
				{
					IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
                    _connectionString = conn.ConnectionString;
				}
			}

			return _connectionString;
		}

		#endregion
	}
}