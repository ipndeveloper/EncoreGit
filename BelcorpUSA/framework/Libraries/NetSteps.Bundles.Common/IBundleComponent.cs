using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Bundles.Common.Models;

namespace NetSteps.Bundles.Common
{
	[ContractClass(typeof(Contracts.BundleComponentContracts))]
	public interface IBundleComponent
	{
		bool IsValidKit(List<IBundleItem> itemsOnKit, int kitProductID);
		IEnumerable<ICustomerKitResult> AutoBundleItems(IBundleOrder order, int storeFrontID);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IBundleComponent))]
		abstract class BundleComponentContracts : IBundleComponent
		{
			public bool IsValidKit(List<IBundleItem> itemsOnKit, int kitProductID)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<ICustomerKitResult> AutoBundleItems(IBundleOrder order, int storeFrontID)
			{
				Contract.Requires<ArgumentNullException>(order != null);

				throw new NotImplementedException();
			}
		}
	}
}