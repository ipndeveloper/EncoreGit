using System;

using NetSteps.AccountLeadService.Common;

namespace NetSteps.AccountLeadService.Services
{
	public class AccountLeadServiceMock : IAccountLeadService
	{
		#region Implementation of IAccountLeadService

		public int GetLeadCount(int accountId)
		{
			return 1;
		}

		public void SetLeadCount(int accountId, int amount)
		{
			throw new NotImplementedException();
		}

		public void IncrementLeadCount(int accountId, int amount = 1)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
