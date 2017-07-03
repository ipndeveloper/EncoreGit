using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common
{
	using NetSteps.Bundles.Common.Models;
	using NetSteps.Data.Common.Context;

	public interface IBundleComponent
	{
		bool IsValidKit(List<IBundleItem> itemsOnKit, int kitProductID);
		IEnumerable<ICustomerKitResult> AutoBundleItems(IBundleOrder orderContext, int storeFrontID);
	}
}
