using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Accounts.Downline.Common.Models;

namespace NetSteps.Accounts.Downline.Common.Repositories
{
	/// <summary>
	/// A repository for retrieving downline-related data.
	/// </summary>
	[ContractClass(typeof(Contracts.DownlineRepositoryContracts))]
	public interface IDownlineRepository
	{
		/// <summary>
		/// Returns basic account data for a specified account's downline.
		/// </summary>
		/// <param name="context">The specification parameters for getting downline data.</param>
		IList<IDownlineData> GetDownline(IGetDownlineContext context);

		/// <summary>
		/// Returns basic account data for matching accounts in a specified account's downline.
		/// </summary>
		/// <param name="context">The specification parameters for searching downline data.</param>
		IList<IDownlineData> Search(ISearchDownlineContext context);

		/// <summary>
		/// Returns basic account data for matching accounts marked as business entities in a specified account's downline. 
		/// </summary>
		/// <returns>The specification parameters for searching downline data.></returns>
		IList<IBusinessEntityDownlineData> SearchBusinessEntities(ISearchDownlineContext context);

		/// <summary>
		/// Returns all account IDs in the specified account's upline.
		/// </summary>
		/// <param name="accountId">The account ID whose upline will be returned.</param>
		IList<int> GetUplineAccountIds(int accountId);

		/// <summary>
		/// Returns extended data related to an account inside another account's downline.
		/// </summary>
		/// <param name="rootAccountId">The root account ID whose downline contains the specified account.</param>
		/// <param name="accountId">The account ID of the person whose info will be returned.</param>
		IDownlineAccountInfo GetDownlineAccountInfo(int rootAccountId, int accountId);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineRepository))]
		internal abstract class DownlineRepositoryContracts : IDownlineRepository
		{
			IList<IDownlineData> IDownlineRepository.GetDownline(IGetDownlineContext context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentOutOfRangeException>(context.RootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(context.MaxLevels == null || context.MaxLevels >= 0);
				Contract.Ensures(Contract.Result<IList<IDownlineData>>() != null);
				throw new NotImplementedException();
			}

			IList<IDownlineData> IDownlineRepository.Search(ISearchDownlineContext context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentOutOfRangeException>(context.RootAccountId > 0);
				Contract.Requires<ArgumentNullException>(context.Query != null);
				Contract.Requires<ArgumentException>(context.Query.Length > 0);
				Contract.Ensures(Contract.Result<IList<IDownlineData>>() != null);
				throw new NotImplementedException();
			}

			IList<IBusinessEntityDownlineData> IDownlineRepository.SearchBusinessEntities(ISearchDownlineContext context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentOutOfRangeException>(context.RootAccountId > 0);
				Contract.Requires<ArgumentNullException>(context.Query != null);
				Contract.Requires<ArgumentException>(context.Query.Length > 0);
				Contract.Ensures(Contract.Result<IList<IDownlineData>>() != null);
				throw new NotImplementedException();
			}

			IList<int> IDownlineRepository.GetUplineAccountIds(int accountId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
				Contract.Ensures(Contract.Result<IList<int>>() != null);
				throw new NotImplementedException();
			}

			IDownlineAccountInfo IDownlineRepository.GetDownlineAccountInfo(int rootAccountId, int accountId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
				throw new NotImplementedException();
			}
		}
	}
}
