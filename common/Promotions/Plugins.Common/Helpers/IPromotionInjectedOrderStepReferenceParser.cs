using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.Helpers
{
	public interface IPromotionInjectedOrderStepReferenceParser
	{
		void SetStepReferenceID(string injectedOrderStepReferenceID);
		string GetStepReferenceID();
		int CustomerAccountID { get; set; }
		string ComponentHandlerProviderKey { get; set; }
		int ComponentExtensionID { get; set; }
	}
}
