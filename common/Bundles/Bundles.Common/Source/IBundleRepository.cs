using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common
{
	using NetSteps.Bundles.Common.Models;

	public interface IBundleRepository
	{
		IKit GetDynamicKitProductByID(int dynamicKitProductID);
		List<IKit> GetDynamicKitProducts(int storeFrontID, bool sort = false, bool sortDescending = false);
	}
}
