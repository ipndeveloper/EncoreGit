using System;
using System.Diagnostics.Contracts;

using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.AccountLocatorServiceRepositoryContracts))]
	public interface IAccountLocatorServiceRepository
	{
		/// <summary>
		/// Searches the database using the arguments.
		/// </summary>
		/// <param name="parameters">The filters to search by.</param>
		/// <param name="geoCodeRange">The geo code range to use to help filter the result set.</param>
		/// <returns>The results of the search.</returns>
		IPaginatedList<IAccountLocatorSearchData> Search(IAccountLocatorSearchParameters parameters, IGeoCodeRange geoCodeRange);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLocatorServiceRepository))]
		abstract class AccountLocatorServiceRepositoryContracts : IAccountLocatorServiceRepository
		{
			public IPaginatedList<IAccountLocatorSearchData> Search(IAccountLocatorSearchParameters parameters, IGeoCodeRange geoCodeRange)
			{
				Contract.Requires<ArgumentNullException>(parameters != default(IAccountLocatorSearchParameters));

				throw new NotImplementedException();
			}
		}
	}
}
