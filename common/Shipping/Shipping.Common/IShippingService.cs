using System;

namespace NetSteps.Shipping.Common
{
	using System.Collections.Generic;

	using NetSteps.Shipping.Common.Models;

	public interface IShippingService
	{
		List<string> UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels);

		void UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels, Action<UpdatePackageModel, string> reportError, Action<UpdatePackageModel, string> reportStatus, Action<UpdatePackageModel, string> reportBadData);
	}

	namespace Contracts
	{
		using System.Diagnostics.Contracts;

		internal abstract class IShippingServiceContracts : IShippingService
		{
			public List<string> UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels)
			{
				Contract.Requires(updatePackageModels != null);
				Contract.Ensures(Contract.Result<List<string>>() != null);
				throw new NotImplementedException();
			}

			public void UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels, Action<UpdatePackageModel, string> reportError, Action<UpdatePackageModel, string> reportStatus, Action<UpdatePackageModel, string> reportBadData)
			{
				Contract.Requires(updatePackageModels != null);
				throw new NotImplementedException();
			}
		}
	}
}
