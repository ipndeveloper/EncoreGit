using System;
using System.Diagnostics.Contracts;

using NetSteps.Common.Base;

namespace NetSteps.AccountLocatorService.Common
{
	[ContractClass(typeof(Contracts.AccountLocatorServiceAdapterContracts))]
	public interface IAccountLocatorServiceAdapter
	{
		/// <summary>
		/// Search for accounts based on the parameters
		/// </summary>
		/// <param name="searchParameters">Filters used in the search</param>
		/// <returns>An IPaginatedList of results</returns>
		IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorServiceSearchParameters searchParameters);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLocatorServiceAdapter))]
		abstract class AccountLocatorServiceAdapterContracts : IAccountLocatorServiceAdapter
		{
			public IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorServiceSearchParameters searchParameters)
			{
				Contract.Requires<ArgumentNullException>(searchParameters != default(IAccountLocatorServiceSearchParameters));

				throw new NotImplementedException();
			}
		}
	}
}
