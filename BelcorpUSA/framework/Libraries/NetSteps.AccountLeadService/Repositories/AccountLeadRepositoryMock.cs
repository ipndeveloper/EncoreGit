using System;
using NetSteps.AccountLeadService.Common.Repositories;

namespace NetSteps.AccountLeadService.Repositories
{
	public class AccountLeadRepositoryMock : IAccountLeadRepository
	{
		#region Implementation of IAccountLeadRepository

		public int? GetLeadCount(int accountId)
		{
			return new int?();
		}

		public void SetLeadCount(int accountId, int amount)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
