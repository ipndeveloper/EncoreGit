using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Accounts.Common.Models;

namespace NetSteps.Accounts.Common.Repositories
{
	/// <summary>
	/// TODO: Move this to NetSteps.Accounts.Common nuget package. - Lundy
	/// </summary>
	[ContractClass(typeof(Contracts.DownlineRepositoryContracts))]
	public interface IDownlineRepository
	{
		IList<IDownlineAccountData> GetDownline(IGetDownlineParameters parameters);
		IList<IDownlineAccountData> Search(ISearchDownlineParameters parameters);
		IList<int> GetUplineAccountIds(int accountId);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineRepository))]
		internal abstract class DownlineRepositoryContracts : IDownlineRepository
		{
			IList<IDownlineAccountData> IDownlineRepository.GetDownline(IGetDownlineParameters parameters)
			{
				Contract.Requires<ArgumentNullException>(parameters != null);
				Contract.Requires<ArgumentOutOfRangeException>(parameters.RootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(parameters.MaxLevels == null || parameters.MaxLevels >= 0);
				Contract.Ensures(Contract.Result<IList<IDownlineAccountData>>() != null);
				throw new NotImplementedException();
			}

			IList<IDownlineAccountData> IDownlineRepository.Search(ISearchDownlineParameters parameters)
			{
				Contract.Requires<ArgumentNullException>(parameters != null);
				Contract.Requires<ArgumentOutOfRangeException>(parameters.RootAccountId > 0);
				Contract.Requires<ArgumentNullException>(parameters.Query != null);
				Contract.Requires<ArgumentException>(parameters.Query.Length > 0);
				Contract.Ensures(Contract.Result<IList<IDownlineAccountData>>() != null);
				throw new NotImplementedException();
			}

			IList<int> IDownlineRepository.GetUplineAccountIds(int accountId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
				Contract.Ensures(Contract.Result<IList<int>>() != null);
				throw new NotImplementedException();
			}
		}
	}
}
